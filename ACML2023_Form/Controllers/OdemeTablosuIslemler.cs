using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class OdemeTablosuIslemler : OdemeTablosuIslemlerBase
    {
        public OdemeTablosuIslemler() : base() { }

        public OdemeTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public virtual SurecBilgiModel OdemeDurumGuncelle(OdemeTablosuModel GuncelKayit)
        {
            VTIslem.SetCommandText("UPDATE OdemeTablosu SET Durum=@Durum, OdemeTarihi=@OdemeTarihi, OdemeParametreleri=@OdemeParametreleri WHERE OdemeID=@OdemeID");
            VTIslem.AddWithValue("Durum", GuncelKayit.Durum);
            VTIslem.AddWithValue("OdemeTarihi", GuncelKayit.OdemeTarihi.Value);
            VTIslem.AddWithValue("OdemeParametreleri", GuncelKayit.OdemeParametreleri);
            VTIslem.AddWithValue("OdemeID", GuncelKayit.OdemeID);
            return VTIslem.ExecuteNonQuery();
        }

        public virtual SurecBilgiModel SecureHashGuncelle(string OdemeID, string Hash)
        {
            VTIslem.SetCommandText("UPDATE OdemeTablosu SET SecureHash=@SecureHash WHERE OdemeID=@OdemeID");
            VTIslem.AddWithValue("SecureHash", Hash);
            VTIslem.AddWithValue("OdemeID", OdemeID);
            return VTIslem.ExecuteNonQuery();
        }

        public override SurecVeriModel<OdemeTablosuModel> KayitBilgisi(string OdemeID)
        {
            int
                OdemeIndex = 0,
                OdemeTipiIndex = OdemeIndex + OdemeTablosuModel.OzellikSayisi,
                KatilimciIndex = OdemeTipiIndex + OdemeTipiTablosuModel.OzellikSayisi,
                KatilimciTipiIndex = KatilimciIndex + KatilimciTablosuModel.OzellikSayisi;


            VTIslem.SetCommandText($"SELECT {OdemeTablosuModel.SQLSutunSorgusu}, {OdemeTipiTablosuModel.SQLSutunSorgusu}, {KatilimciTablosuModel.SQLSutunSorgusu}, {KatilimciTipiTablosuModel.SQLSutunSorgusu} FROM ( ( ( OdemeTablosu INNER JOIN OdemeTipiTablosu ON OdemeTablosu.OdemeTipiID = OdemeTipiTablosu.OdemeTipiID ) INNER JOIN KatilimciTablosu ON OdemeTablosu.KatilimciID = KatilimciTablosu.KatilimciID ) INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) WHERE OdemeID = @OdemeID");
            VTIslem.AddWithValue("OdemeID", OdemeID);
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                while (SModel.Reader.Read())
                {
                    if (KayitBilgisiAl(OdemeIndex, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                    {
                        SDataModel.Veriler.OdemeTipiBilgisi = new OdemeTipiTablosuIslemler().KayitBilgisiAl(OdemeTipiIndex, SModel.Reader).Veriler;
                        SDataModel.Veriler.KatilimciBilgisi = new KatilimciTablosuIslemler().KayitBilgisiAl(KatilimciIndex, SModel.Reader).Veriler;
                        SDataModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi = new KatilimciTipiTablosuIslemler().KayitBilgisiAl(KatilimciTipiIndex, SModel.Reader).Veriler;
                    }
                }
                if (SDataModel is null)
                {
                    SDataModel = new SurecVeriModel<OdemeTablosuModel>
                    {
                        Sonuc = Sonuclar.VeriBulunamadi,
                        KullaniciMesaji = "Belirtilen kayýt bulunamamýþtýr",
                        HataBilgi = new HataBilgileri
                        {
                            HataAlinanKayitID = 0,
                            HataKodu = 0,
                            HataMesaji = "Belirtilen kayýt bulunamamýþtýr"
                        }
                    };
                }
            }
            else
            {
                SDataModel = new SurecVeriModel<OdemeTablosuModel>
                {
                    Sonuc = SModel.Sonuc,
                    KullaniciMesaji = SModel.KullaniciMesaji,
                    HataBilgi = SModel.HataBilgi
                };
            }
            VTIslem.CloseConnection();
            return SDataModel;
        }
    }
}
