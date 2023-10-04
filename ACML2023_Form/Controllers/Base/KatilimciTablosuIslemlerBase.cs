using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class KatilimciTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<KatilimciTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<KatilimciTablosuModel> SDataModel;
		public SurecVeriModel<IList<KatilimciTablosuModel>> SDataListModel;

		public KatilimciTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public KatilimciTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(KatilimciTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO KatilimciTablosu (KatilimciTipiID, Ad, Soyad, ePosta, Telefon, Kurum, Ulke, BildiriNo, GuncellenmeTarihi, EklenmeTarihi) VALUES (@KatilimciTipiID, @Ad, @Soyad, @ePosta, @Telefon, @Kurum, @Ulke, @BildiriNo, @GuncellenmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("KatilimciTipiID", YeniKayit.KatilimciTipiID);
			VTIslem.AddWithValue("Ad", YeniKayit.Ad);
			VTIslem.AddWithValue("Soyad", YeniKayit.Soyad);
			VTIslem.AddWithValue("ePosta", YeniKayit.ePosta);
			VTIslem.AddWithValue("Telefon", YeniKayit.Telefon);
			VTIslem.AddWithValue("Kurum", YeniKayit.Kurum);
			VTIslem.AddWithValue("Ulke", YeniKayit.Ulke);
			VTIslem.AddWithValue("BildiriNo", YeniKayit.BildiriNo);
			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(KatilimciTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE KatilimciTablosu SET KatilimciTipiID=@KatilimciTipiID, Ad=@Ad, Soyad=@Soyad, ePosta=@ePosta, Telefon=@Telefon, Kurum=@Kurum, Ulke=@Ulke, BildiriNo=@BildiriNo, GuncellenmeTarihi=@GuncellenmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE KatilimciID=@KatilimciID");
			VTIslem.AddWithValue("KatilimciTipiID", GuncelKayit.KatilimciTipiID);
			VTIslem.AddWithValue("Ad", GuncelKayit.Ad);
			VTIslem.AddWithValue("Soyad", GuncelKayit.Soyad);
			VTIslem.AddWithValue("ePosta", GuncelKayit.ePosta);
			VTIslem.AddWithValue("Telefon", GuncelKayit.Telefon);
			VTIslem.AddWithValue("Kurum", GuncelKayit.Kurum);
			VTIslem.AddWithValue("Ulke", GuncelKayit.Ulke);
			VTIslem.AddWithValue("BildiriNo", GuncelKayit.BildiriNo);
			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("KatilimciID", GuncelKayit.KatilimciID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(int KatilimciID)
		{
			VTIslem.SetCommandText("DELETE FROM KatilimciTablosu WHERE KatilimciID=@KatilimciID");
			VTIslem.AddWithValue("KatilimciID", KatilimciID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<KatilimciTablosuModel> KayitBilgisi(int KatilimciID)
		{
			VTIslem.SetCommandText($"SELECT {KatilimciTablosuModel.SQLSutunSorgusu} FROM KatilimciTablosu WHERE KatilimciID = @KatilimciID");
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
					SDataModel = new SurecVeriModel<KatilimciTablosuModel>
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
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<KatilimciTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {KatilimciTablosuModel.SQLSutunSorgusu} FROM KatilimciTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KatilimciTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<KatilimciTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KatilimciTablosuModel
					{
						KatilimciID = SModel.Reader.GetInt32(0),
						KatilimciTipiID = SModel.Reader.GetInt32(1),
						Ad = SModel.Reader.GetString(2),
						Soyad = SModel.Reader.GetString(3),
						ePosta = SModel.Reader.GetString(4),
						Telefon = SModel.Reader.GetString(5),
						Kurum = SModel.Reader.GetString(6),
						Ulke = SModel.Reader.GetString(7),
						BildiriNo = SModel.Reader.GetString(8),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(9),
						EklenmeTarihi = SModel.Reader.GetDateTime(10),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
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
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
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

		public virtual SurecVeriModel<KatilimciTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KatilimciTablosuModel{
						KatilimciID = Reader.GetInt32(Baslangic + 0),
						KatilimciTipiID = Reader.GetInt32(Baslangic + 1),
						Ad = Reader.GetString(Baslangic + 2),
						Soyad = Reader.GetString(Baslangic + 3),
						ePosta = Reader.GetString(Baslangic + 4),
						Telefon = Reader.GetString(Baslangic + 5),
						Kurum = Reader.GetString(Baslangic + 6),
						Ulke = Reader.GetString(Baslangic + 7),
						BildiriNo = Reader.GetString(Baslangic + 8),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 9),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 10),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
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
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
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

		public virtual SurecVeriModel<IList<KatilimciTablosuModel>> KatilimciTipiBilgileri(int KatilimciTipiID)
		{
			VTIslem.SetCommandText($"SELECT {KatilimciTablosuModel.SQLSutunSorgusu} FROM KatilimciTablosu WHERE KatilimciTipiID=@KatilimciTipiID");
			VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KatilimciTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		public virtual SurecVeriModel<IList<KatilimciTablosuModel>> UlkeBilgileri(string Ulke)
		{
			VTIslem.SetCommandText($"SELECT {KatilimciTablosuModel.SQLSutunSorgusu} FROM KatilimciTablosu WHERE Ulke=@Ulke");
			VTIslem.AddWithValue("Ulke", Ulke);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KatilimciTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

	}
}