using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using VeritabaniIslemMerkezi;
using VeritabaniIslemMerkezi.Access;

namespace ACML2023_Form
{
    public partial class Default : Page
    {
        StringBuilder Uyarilar = new StringBuilder();
        BilgiKontrolMerkezi Kontrol = new BilgiKontrolMerkezi();

        SurecBilgiModel SModel;

        KatilimciTablosuModel KModel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListItem li = new ListItem { Value = string.Empty, Text = "Select" };

                ddlUlke.DataBind();
                ddlUlke.Items.Insert(0, li);

                ddlKatilimciTipi.DataBind();
                ddlKatilimciTipi.Items.Insert(0, li);

                ddlOdemeTipi.DataBind();
                ddlOdemeTipi.Items.Insert(0, li);
            }
        }

        protected void ddlBildiriDurum_SelectedIndexChanged(object sender, EventArgs e)
        {
            tr_BildiriNo.Visible = string.IsNullOrEmpty(ddlBildiriDurum.SelectedValue) ? false : Convert.ToBoolean(ddlBildiriDurum.SelectedValue);
            Kontrol.Temizle(txtBildiriNo);
        }

        protected void ddlOdemeTipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            PnlBankaHavalesi.Visible = ddlOdemeTipi.SelectedValue.Equals("1");


            switch (ddlOdemeTipi.SelectedValue)
            {
                default:
                case "1":
                    lnkbtnKayitOl.Text = "<i class=\"fa fa-check\"></i>&nbsp;Complete Registration";
                    break;

                case "2":
                    lnkbtnKayitOl.Text = "<i class=\"fa fa-check\"></i>&nbsp;Process to Payment Information";
                    break;

            }
        }

        protected void lnkbtnKayitOl_Click(object sender, EventArgs e)
        {
            KModel = new KatilimciTablosuModel
            {
                KatilimciID = 0,
                Ad = Kontrol.KelimeKontrol(txtAd, "Name cannot be empty", ref Uyarilar),
                Soyad = Kontrol.KelimeKontrol(txtSoyad, "Surname cannot be empty", ref Uyarilar),
                ePosta = Kontrol.ePostaKontrol(txtePosta, "Email cannot be empty", "Invalid email is typed", ref Uyarilar),
                Telefon = Kontrol.KelimeKontrol(txtTelefon, "Telephone cannot be empty", ref Uyarilar),
                Kurum = Kontrol.KelimeKontrol(txtKurum, "Institution cannot be empty", ref Uyarilar),
                Ulke = Kontrol.KelimeKontrol(ddlUlke, "Please select your country", ref Uyarilar),
                BildiriNo = string.Empty,
                KatilimciTipiID = Kontrol.TamSayiyaKontrol(ddlKatilimciTipi, "Please select your registration type", "Insvalid registration type is selected", ref Uyarilar),

                GuncellenmeTarihi = Kontrol.Simdi(),
                EklenmeTarihi = Kontrol.Simdi(),
                OdemeBilgisi = new OdemeTablosuModel
                {
                    OdemeID = $"ACML{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(10, 99)}",
                    OdemeTipiID = Kontrol.TamSayiyaKontrol(ddlOdemeTipi, "Please select your payment type", "Invalid registration type is selected", ref Uyarilar),
                    KatilimciID = 0,
                    Durum = false,
                    OdemeTarihi = null,
                    OdemeParametreleri = "Waiting to payment result",
                    SecureHash = "Waiting to secure hash",
                    EklenmeTarihi = Kontrol.Simdi()
                }
            };


            if (string.IsNullOrEmpty(ddlBildiriDurum.SelectedValue))
                Uyarilar.Append("<p>Please select status of accepted paper</p>");
            else if (Convert.ToBoolean(ddlBildiriDurum.SelectedValue))
                KModel.BildiriNo = Kontrol.KelimeKontrol(txtBildiriNo, "Paper number cannot be empty", ref Uyarilar);


            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                using (OleDbConnection cnn = ConnectionBuilder.DefaultConnection())
                {
                    ConnectionBuilder.OpenConnection(cnn);
                    using (OleDbTransaction trn = cnn.BeginTransaction())
                    {
                        SModel = new KatilimciTablosuIslemler(trn).YeniKayitEkle(KModel);

                        if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                        {
                            KModel.KatilimciID = Convert.ToInt32(SModel.YeniKayitID);
                            KModel.OdemeBilgisi.KatilimciID = KModel.KatilimciID;

                            SModel = new OdemeTablosuIslemler(trn).YeniKayitEkle(KModel.OdemeBilgisi);

                            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                            {
                                trn.Commit();

                                if (KModel.OdemeBilgisi.OdemeTipiID.Equals(1))
                                    Response.Redirect($"~/RegistationSuccessful/{KModel.OdemeBilgisi.OdemeID}");
                                else
                                    Response.Redirect($"~/Payment/{KModel.OdemeBilgisi.OdemeID}");
                            }
                            else
                            {
                                trn.Rollback();
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>There is an error occured while saving payment information</p><p>Error message : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                            }
                        }
                        else
                        {
                            trn.Rollback();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>There is an error occured while saving personel information</p><p>Error message : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                        }
                    }
                    ConnectionBuilder.CloseConnection(cnn);
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '{Uyarilar}', false);", false);
            }
        }
    }
}