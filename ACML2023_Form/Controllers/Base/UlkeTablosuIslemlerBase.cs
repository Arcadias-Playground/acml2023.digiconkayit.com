using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class UlkeTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<UlkeTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<UlkeTablosuModel> SDataModel;
		public SurecVeriModel<IList<UlkeTablosuModel>> SDataListModel;

		public UlkeTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public UlkeTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(UlkeTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO UlkeTablosu (Ulke, GrupNo, EklenmeTarihi) VALUES (@Ulke, @GrupNo, @EklenmeTarihi)");
			VTIslem.AddWithValue("Ulke", YeniKayit.Ulke);

			if(YeniKayit.GrupNo is null)
				VTIslem.AddWithValue("GrupNo", DBNull.Value);
			else
				VTIslem.AddWithValue("GrupNo", YeniKayit.GrupNo);


			if(YeniKayit.EklenmeTarihi is null)
				VTIslem.AddWithValue("EklenmeTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);

			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(UlkeTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE UlkeTablosu SET GrupNo=@GrupNo, EklenmeTarihi=@EklenmeTarihi WHERE Ulke=@Ulke");

			if(GuncelKayit.GrupNo is null)
				VTIslem.AddWithValue("GrupNo", DBNull.Value);
			else
				VTIslem.AddWithValue("GrupNo", GuncelKayit.GrupNo.Value);


			if(GuncelKayit.EklenmeTarihi is null)
				VTIslem.AddWithValue("EklenmeTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi.Value);

			VTIslem.AddWithValue("Ulke", GuncelKayit.Ulke);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(string Ulke)
		{
			VTIslem.SetCommandText("DELETE FROM UlkeTablosu WHERE Ulke=@Ulke");
			VTIslem.AddWithValue("Ulke", Ulke);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<UlkeTablosuModel> KayitBilgisi(string Ulke)
		{
			VTIslem.SetCommandText($"SELECT {UlkeTablosuModel.SQLSutunSorgusu} FROM UlkeTablosu WHERE Ulke = @Ulke");
			VTIslem.AddWithValue("Ulke", Ulke);
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
					SDataModel = new SurecVeriModel<UlkeTablosuModel>
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
				SDataModel = new SurecVeriModel<UlkeTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<UlkeTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {UlkeTablosuModel.SQLSutunSorgusu} FROM UlkeTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<UlkeTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<UlkeTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<UlkeTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<UlkeTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<UlkeTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<UlkeTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new UlkeTablosuModel
					{
						Ulke = SModel.Reader.GetString(0),
						GrupNo = SModel.Reader.IsDBNull(1) ? null : new int?(SModel.Reader.GetInt32(1)),
						EklenmeTarihi = SModel.Reader.IsDBNull(2) ? null : new DateTime?(SModel.Reader.GetDateTime(2)),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<UlkeTablosuModel>{
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
				SDataModel = new SurecVeriModel<UlkeTablosuModel>{
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

		public virtual SurecVeriModel<UlkeTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<UlkeTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new UlkeTablosuModel{
						Ulke = Reader.GetString(Baslangic + 0),
						GrupNo = Reader.IsDBNull(Baslangic + 1) ? null : new int?(Reader.GetInt32(Baslangic + 1)),
						EklenmeTarihi = Reader.IsDBNull(Baslangic + 2) ? null : new DateTime?(Reader.GetDateTime(Baslangic + 2)),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<UlkeTablosuModel>{
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
				SDataModel = new SurecVeriModel<UlkeTablosuModel>{
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

	}
}