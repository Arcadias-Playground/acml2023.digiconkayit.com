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
    public partial class PaymentFail : Page
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

                    if (SDataModel.Sonuc.Equals(Sonuclar.Basarili) && SDataModel.Veriler.OdemeTipiID.Equals(2))
                    {
                        if (!(Request.Form["Hash"] is null) && !string.IsNullOrEmpty(Request.Form["Hash"]) && SDataModel.Veriler.SecureHash.Equals(Request.Form["Hash"]))
                        {
                            SDataModel.Veriler.Durum = false;
                            SDataModel.Veriler.OdemeTarihi = new BilgiKontrolMerkezi().Simdi();

                            List<KeyValuePair<string, string>> ParamList = new List<KeyValuePair<string, string>>();
                            foreach (string Key in Request.Form.Keys)
                                ParamList.Add(new KeyValuePair<string, string>(Key, Request.Form[Key]));

                            SDataModel.Veriler.OdemeParametreleri = string.Join(" //// ", ParamList.Select(x => $"{x.Key} : {x.Value}"));

                            SModel = new OdemeTablosuIslemler().OdemeDurumGuncelle(SDataModel.Veriler);

                            File.WriteAllText(Server.MapPath($"~/Dosyalar/PaymentLog/Fail/{SDataModel.Veriler.OdemeID}_{DateTime.Now:yyyy.MM.dd HH.mm.ss}.log"), string.Join("\r\n", ParamList.Select(x => $"{x.Key} : {x.Value}")));

                            lblAdSoyad.Text = $"{SDataModel.Veriler.KatilimciBilgisi.Ad} {SDataModel.Veriler.KatilimciBilgisi.Soyad}";
                            lblOdemeID.Text = SDataModel.Veriler.OdemeID;
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