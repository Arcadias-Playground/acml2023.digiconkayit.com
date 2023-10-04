using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using VeritabaniIslemMerkezi;
using Microsoft.AspNet.FriendlyUrls;
using System.IO;

namespace ACML2023_Form
{
    public partial class RegistationSuccessful : Page
    {
        IList<string> segments;

        SurecBilgiModel SModel;
        SurecVeriModel<OdemeTablosuModel> SDataModel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                segments = Request.GetFriendlyUrlSegments();

                if (segments.Count.Equals(1))
                {
                    SDataModel = new OdemeTablosuIslemler().KayitBilgisi(segments.First());

                    if (SDataModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        switch (SDataModel.Veriler.OdemeTipiID)
                        {
                            // Banka Havalesi
                            case 1:
                                if (!SDataModel.Veriler.Durum && SDataModel.Veriler.OdemeTarihi is null)
                                {
                                    SDataModel.Veriler.Durum = true;
                                    SDataModel.Veriler.OdemeTarihi = new BilgiKontrolMerkezi().Simdi();
                                    SDataModel.Veriler.OdemeParametreleri = "Bank Transfer";

                                    SModel = new OdemeTablosuIslemler().OdemeDurumGuncelle(SDataModel.Veriler);

                                    File.WriteAllText(Server.MapPath($"~/Dosyalar/PaymentLog/OK/{SDataModel.Veriler.OdemeID}_{DateTime.Now:yyyy.MM.dd HH.mm.ss}.log"), SDataModel.Veriler.OdemeParametreleri);

                                    lblAdSoyad.Text = $"{SDataModel.Veriler.KatilimciBilgisi.Ad} {SDataModel.Veriler.KatilimciBilgisi.Soyad}";
                                    lblOdemeID.Text = SDataModel.Veriler.OdemeID;
                                    PnlBankaHavalesi.Visible = true;

                                    // Mail Gönderim
                                    new MailGonderimIslemler().KayitBilgilendirme(SDataModel.Veriler);
                                }
                                else
                                {
                                    Response.Redirect("~/");
                                }
                                break;

                            // Kredi Kartı
                            case 2:
                                if (!(Request.Form["Hash"] is null) && !string.IsNullOrEmpty(Request.Form["Hash"]) && SDataModel.Veriler.SecureHash.Equals(Request.Form["Hash"]))
                                {
                                    SDataModel.Veriler.Durum = true;
                                    SDataModel.Veriler.OdemeTarihi = new BilgiKontrolMerkezi().Simdi();

                                    List<KeyValuePair<string, string>> ParamList = new List<KeyValuePair<string, string>>();
                                    foreach (string Key in Request.Form.Keys)
                                        ParamList.Add(new KeyValuePair<string, string>(Key, Request.Form[Key]));

                                    SDataModel.Veriler.OdemeParametreleri = string.Join(" //// ", ParamList.Select(x => $"{x.Key} : {x.Value}"));

                                    SModel = new OdemeTablosuIslemler().OdemeDurumGuncelle(SDataModel.Veriler);

                                    File.WriteAllText(Server.MapPath($"~/Dosyalar/PaymentLog/OK/{SDataModel.Veriler.OdemeID}_{DateTime.Now:yyyy.MM.dd HH.mm.ss}.log"), string.Join("\r\n", ParamList.Select(x => $"{x.Key} : {x.Value}")));

                                    lblAdSoyad.Text = $"{SDataModel.Veriler.KatilimciBilgisi.Ad} {SDataModel.Veriler.KatilimciBilgisi.Soyad}";
                                    lblOdemeID.Text = SDataModel.Veriler.OdemeID;

                                    new MailGonderimIslemler().KayitBilgilendirme(SDataModel.Veriler);
                                }
                                else
                                {
                                    Response.Redirect("~/");
                                }
                                break;

                            default:
                                Response.Redirect("~/");
                                break;
                        }
                    }
                    else
                    {
                        Response.Redirect("~/");
                    }
                }
                else
                {
                    Response.Redirect("~/");
                }
            }
        }
    }
}