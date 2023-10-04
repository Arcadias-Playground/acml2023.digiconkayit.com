using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class OdemeTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<OdemeTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<OdemeTablosuModel> SDataModel;
		public SurecVeriModel<IList<OdemeTablosuModel>> SDataListModel;

		public OdemeTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public OdemeTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(OdemeTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO OdemeTablosu (OdemeID, OdemeTipiID, KatilimciID, Durum, OdemeTarihi, OdemeParametreleri, SecureHash, EklenmeTarihi) VALUES (@OdemeID, @OdemeTipiID, @KatilimciID, @Durum, @OdemeTarihi, @OdemeParametreleri, @SecureHash, @EklenmeTarihi)");
			VTIslem.AddWithValue("OdemeID", YeniKayit.OdemeID);
			VTIslem.AddWithValue("OdemeTipiID", YeniKayit.OdemeTipiID);
			VTIslem.AddWithValue("KatilimciID", YeniKayit.KatilimciID);
			VTIslem.AddWithValue("Durum", YeniKayit.Durum);

			if(YeniKayit.OdemeTarihi is null)
				VTIslem.AddWithValue("OdemeTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("OdemeTarihi", YeniKayit.OdemeTarihi);

			VTIslem.AddWithValue("OdemeParametreleri", YeniKayit.OdemeParametreleri);
			VTIslem.AddWithValue("SecureHash", YeniKayit.SecureHash);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(OdemeTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE OdemeTablosu SET OdemeTipiID=@OdemeTipiID, KatilimciID=@KatilimciID, Durum=@Durum, OdemeTarihi=@OdemeTarihi, OdemeParametreleri=@OdemeParametreleri, SecureHash=@SecureHash, EklenmeTarihi=@EklenmeTarihi WHERE OdemeID=@OdemeID");
			VTIslem.AddWithValue("OdemeTipiID", GuncelKayit.OdemeTipiID);
			VTIslem.AddWithValue("KatilimciID", GuncelKayit.KatilimciID);
			VTIslem.AddWithValue("Durum", GuncelKayit.Durum);

			if(GuncelKayit.OdemeTarihi is null)
				VTIslem.AddWithValue("OdemeTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("OdemeTarihi", GuncelKayit.OdemeTarihi.Value);

			VTIslem.AddWithValue("OdemeParametreleri", GuncelKayit.OdemeParametreleri);
			VTIslem.AddWithValue("SecureHash", GuncelKayit.SecureHash);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("OdemeID", GuncelKayit.OdemeID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(string OdemeID)
		{
			VTIslem.SetCommandText("DELETE FROM OdemeTablosu WHERE OdemeID=@OdemeID");
			VTIslem.AddWithValue("OdemeID", OdemeID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<OdemeTablosuModel> KayitBilgisi(string OdemeID)
		{
			VTIslem.SetCommandText($"SELECT {OdemeTablosuModel.SQLSutunSorgusu} FROM OdemeTablosu WHERE OdemeID = @OdemeID");
			VTIslem.AddWithValue("OdemeID", OdemeID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				while (SModel.Reader.Read())
				{
					KayitBilgisiAl();
				}
				if (SDataModel is null)
				{
					SDataModel = new SurecVeriModel<OdemeTablosuModel>
					{
						Sonuc = Sonuclar.VeriBulunamadi,
						KullaniciMesaji = "Belirtilen kayıt bulunamamıştır",
						HataBilgi = new HataBilgileri
						{
							HataAlinanKayitID = 0,
							HataKodu = 0,
							HataMesaji = "Belirtilen kayıt bulunamamıştır"
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

		public virtual SurecVeriModel<IList<OdemeTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {OdemeTablosuModel.SQLSutunSorgusu} FROM OdemeTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<OdemeTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<OdemeTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<OdemeTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<OdemeTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<OdemeTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<OdemeTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new OdemeTablosuModel
					{
						OdemeID = SModel.Reader.GetString(0),
						OdemeTipiID = SModel.Reader.GetInt32(1),
						KatilimciID = SModel.Reader.GetInt32(2),
						Durum = SModel.Reader.GetBoolean(3),
						OdemeTarihi = SModel.Reader.IsDBNull(4) ? null : new DateTime?(SModel.Reader.GetDateTime(4)),
						OdemeParametreleri = SModel.Reader.GetString(5),
						SecureHash = SModel.Reader.GetString(6),
						EklenmeTarihi = SModel.Reader.GetDateTime(7),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<OdemeTablosuModel>{
					Sonuc = Sonuclar.Basarisiz,
					KullaniciMesaji = "Veri bilgisi çekilirken hatalı atama yapılmaya çalışıldı",
					HataBilgi = new HataBilgileri{
						HataMesaji = string.Format(@"{0}", ex.Message.Replace("'", "ʼ")),
						HataKodu = ex.HResult,
						HataAlinanKayitID = SModel.Reader.GetValue(0)
					}
				};
			}
			catch (Exception ex)
			{
				SDataModel = new SurecVeriModel<OdemeTablosuModel>{
					Sonuc = Sonuclar.Basarisiz,
					KullaniciMesaji = "Veri bilgisi çekilirken hatalı atama yapılmaya çalışıldı",
					HataBilgi = new HataBilgileri{
						HataMesaji = string.Format(@"{0}", ex.Message.Replace("'", "ʼ")),
						HataKodu = ex.HResult,
						HataAlinanKayitID = SModel.Reader.GetValue(0)
					}
				};
			}
			return SDataModel;
		}

		public virtual SurecVeriModel<OdemeTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<OdemeTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new OdemeTablosuModel{
						OdemeID = Reader.GetString(Baslangic + 0),
						OdemeTipiID = Reader.GetInt32(Baslangic + 1),
						KatilimciID = Reader.GetInt32(Baslangic + 2),
						Durum = Reader.GetBoolean(Baslangic + 3),
						OdemeTarihi = Reader.IsDBNull(Baslangic + 4) ? null : new DateTime?(Reader.GetDateTime(Baslangic + 4)),
						OdemeParametreleri = Reader.GetString(Baslangic + 5),
						SecureHash = Reader.GetString(Baslangic + 6),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 7),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<OdemeTablosuModel>{
					Sonuc = Sonuclar.Basarisiz,
					KullaniciMesaji = "Veri bilgisi çekilirken hatalı atama yapılmaya çalışıldı",
					HataBilgi = new HataBilgileri{
						HataMesaji = string.Format(@"{0}", ex.Message.Replace("'", "ʼ")),
						HataKodu = ex.HResult,
						HataAlinanKayitID = Reader.GetValue(0)
					}
				};
			}
			catch (Exception ex)
			{
				SDataModel = new SurecVeriModel<OdemeTablosuModel>{
					Sonuc = Sonuclar.Basarisiz,
					KullaniciMesaji = "Veri bilgisi çekilirken hatalı atama yapılmaya çalışıldı",
						HataBilgi = new HataBilgileri{
						HataMesaji = string.Format(@"{0}", ex.Message.Replace("'", "ʼ")),
						HataKodu = ex.HResult,
						HataAlinanKayitID = Reader.GetValue(0)
					}
				};
			}
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<OdemeTablosuModel>> OdemeTipiBilgileri(int OdemeTipiID)
		{
			VTIslem.SetCommandText($"SELECT {OdemeTablosuModel.SQLSutunSorgusu} FROM OdemeTablosu WHERE OdemeTipiID=@OdemeTipiID");
			VTIslem.AddWithValue("OdemeTipiID", OdemeTipiID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<OdemeTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<OdemeTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<OdemeTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<OdemeTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		public virtual SurecVeriModel<OdemeTablosuModel> KatilimciBilgisi(int KatilimciID)
		{
			VTIslem.SetCommandText($"SELECT {OdemeTablosuModel.SQLSutunSorgusu}FROM OdemeTablosu WHERE KatilimciID=@KatilimciID");
			VTIslem.AddWithValue("KatilimciID", KatilimciID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				while (SModel.Reader.Read())
				{
					KayitBilgisiAl();
				}
				if (SDataModel is null)
				{
					SDataModel = new SurecVeriModel<OdemeTablosuModel>
					{
						Sonuc = Sonuclar.VeriBulunamadi,
						KullaniciMesaji = "Belirtilen kayıt bulunamamıştır",
						HataBilgi = new HataBilgileri
						{
							HataAlinanKayitID = 0,
							HataKodu = 0,
							HataMesaji = "Belirtilen kayıt bulunamamıştır"
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