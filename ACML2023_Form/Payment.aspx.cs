using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Model;
using VeritabaniIslemMerkezi;
using System.Security.Cryptography;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;

namespace ACML2023_Form
{
    public partial class Payment : Page
    {
        IList<string> segments;

        StringBuilder Uyarilar = new StringBuilder();
        BilgiKontrolMerkezi Kontrol = new BilgiKontrolMerkezi();

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

                    if (SDataModel.Sonuc.Equals(Sonuclar.Basarili) && SDataModel.Veriler.OdemeTarihi is null)
                    {
                        lblAdSoyad.Text = $"{SDataModel.Veriler.KatilimciBilgisi.Ad} {SDataModel.Veriler.KatilimciBilgisi.Soyad}";
                        lblKatilimciTipi.Text = SDataModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KatilimciTipi;
                        lblKatilimciTipiUcret.Text = $"{SDataModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.FormUcret} €";
                        hfKatilimciTipiUcret.Value = SDataModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.FormUcret.ToString("0.00");
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
        }

        protected void lnkbtnOdemeYap_Click(object sender, EventArgs e)
        {
            string
                    KrediKartNo = Kontrol.KelimeKontrol(txtKrediKartNo, "Credit card number cannot be empty", ref Uyarilar),
                    Ay = Kontrol.KelimeKontrol(txtAy, "Exp. month cannot be empty", ref Uyarilar),
                    Yil = Kontrol.KelimeKontrol(txtYil, "Exp. year cannot be empty", ref Uyarilar),
                    CVV2 = Kontrol.KelimeKontrol(txtCvv2, "CVV2 cannot be empty", ref Uyarilar);


            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                List<KeyValuePair<string, string>> postParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("MbrId", "5"),
                     new KeyValuePair<string, string>("MerchantID", "088900000015289"),
                     new KeyValuePair<string, string>("UserCode", "suerseyahatapi"),
                     new KeyValuePair<string, string>("UserPass", "VEXA9"),
                     new KeyValuePair<string, string>("SecureType", "3DPay"),
                     new KeyValuePair<string, string>("TxnType", "Auth"),
                     new KeyValuePair<string, string>("InstallmentCount", "0"),
                     //new KeyValuePair<string, string>("Currency", "978"),
                     new KeyValuePair<string, string>("Currency", "949"),
                     new KeyValuePair<string, string>("OkUrl", $"{Request.Url.Scheme}://{Request.Url.Authority}/{Request.ApplicationPath}/RegistationSuccessful/{lblOdemeID.Text}"),
                     new KeyValuePair<string, string>("FailUrl", $"{Request.Url.Scheme}://{Request.Url.Authority}/{Request.ApplicationPath}/PaymentFail/{lblOdemeID.Text}"),
                     new KeyValuePair<string, string>("OrderId", lblOdemeID.Text),
                     //new KeyValuePair<string, string>("PurchAmount", $"{Convert.ToDecimal(hfKatilimciTipiUcret.Value):0}"),
                     new KeyValuePair<string, string>("PurchAmount", $"1"),
                     new KeyValuePair<string, string>("Lang", "EN"),
                     new KeyValuePair<string, string>("Rnd", DateTime.Now.Ticks.ToString()),

                     new KeyValuePair<string, string>("Pan", KrediKartNo.Replace(" ", string.Empty)),
                     new KeyValuePair<string, string>("Expiry", $"{Ay}{Yil}"),
                     new KeyValuePair<string, string>("Cvv2", CVV2)
                };

                using (SHA1 sha = new SHA1CryptoServiceProvider())
                {
                    postParams.Add(new KeyValuePair<string, string>("Hash", Convert.ToBase64String(sha.ComputeHash(Encoding.ASCII.GetBytes(
                        postParams.First(x => x.Key.Equals("MbrId")).Value +
                        postParams.First(x => x.Key.Equals("OrderId")).Value +
                        postParams.First(x => x.Key.Equals("PurchAmount")).Value +
                        postParams.First(x => x.Key.Equals("OkUrl")).Value +
                        postParams.First(x => x.Key.Equals("FailUrl")).Value +
                        postParams.First(x => x.Key.Equals("TxnType")).Value +
                        postParams.First(x => x.Key.Equals("InstallmentCount")).Value +
                        postParams.First(x => x.Key.Equals("Rnd")).Value +
                        "71647984")))));
                }

                SModel = new OdemeTablosuIslemler().SecureHashGuncelle(lblOdemeID.Text, postParams.First(x => x.Key.Equals("Hash")).Value);

                File.WriteAllText(Server.MapPath($"~/Dosyalar/PaymentLog/Prepare/{lblOdemeID.Text}_{DateTime.Now:yyyy.MM.dd HH.mm.ss}.log"), string.Join("\r\n", postParams.Where(x=> !x.Key.Equals("Cvv2")).Select(x => $"{x.Key} : {x.Value}")));

                using (HttpClient hc = new HttpClient())
                {
                    using (HttpResponseMessage response = hc.PostAsync("https://vpos.qnbfinansbank.com/Gateway/Default.aspx", new FormUrlEncodedContent(postParams)).GetAwaiter().GetResult())
                    {
                        string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        Response.Write(responseContent);
                    }
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '{Uyarilar}', false);", true);
            }
        }
    }
}