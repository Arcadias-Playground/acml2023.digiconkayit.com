using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MimeKit;
using Model;
using VeritabaniIslemMerkezi;
using System.Text;
using MailKit;
using MailKit.Net.Smtp;

namespace VeritabaniIslemMerkezi
{
    public class MailGonderimIslemler
    {
        StringBuilder HtmlContent = new StringBuilder();

        public void KayitBilgilendirme(OdemeTablosuModel OModel)
        {
            using (MimeMessage mm = new MimeMessage())
            {
                mm.From.Add(new MailboxAddress("ACML 2023 Congress", "acml2023@digiconkayit.com"));
                mm.To.Add(new MailboxAddress($"{OModel.KatilimciBilgisi.Ad} {OModel.KatilimciBilgisi.Soyad}", OModel.KatilimciBilgisi.ePosta));
                mm.Bcc.Add(new MailboxAddress("Mehmet Gönen", "mehmetgonen@ku.edu.tr"));
                mm.Bcc.Add(new MailboxAddress("Cihan Çalışkan", "cihan@suerturizm.com"));

                mm.Subject = "ACML 2023 Registation Information";

                HtmlContent.Append($"<p>Dear {OModel.KatilimciBilgisi.Ad} {OModel.KatilimciBilgisi.Soyad},</p>");
                HtmlContent.Append($"<p>Your registration informations:</p>");
                HtmlContent.Append($"<table style='width:100%;'>");
                HtmlContent.Append($"<tr><td style='width:250px;'>E-Mail</td><td>{OModel.KatilimciBilgisi.ePosta}</td></tr>");
                HtmlContent.Append($"<tr><td>Telephone</td><td>{OModel.KatilimciBilgisi.Telefon}</td></tr>");
                HtmlContent.Append($"<tr><td>Institution</td><td>{OModel.KatilimciBilgisi.Kurum}</td></tr>");
                HtmlContent.Append($"<tr><td>Country</td><td>{OModel.KatilimciBilgisi.Ulke}</td></tr>");
                HtmlContent.Append($"<tr><td>Accepted Paper No</td><td>{OModel.KatilimciBilgisi.BildiriNo}</td></tr>");
                HtmlContent.Append($"<tr><td>Registration Type</td><td>{OModel.KatilimciBilgisi.KatilimciTipiBilgisi.KatilimciTipi}</td></tr>");
                HtmlContent.Append($"<tr><td>Payment Type</td><td>{OModel.OdemeTipiBilgisi.OdemeTipi}</td></tr>");
                HtmlContent.Append($"<tr><td>Payment Fee</td><td>{OModel.KatilimciBilgisi.KatilimciTipiBilgisi.FormUcret} €</td></tr>");
                HtmlContent.Append($"</table>");

                if (OModel.OdemeTipiID.Equals(1))
                {
                    HtmlContent.Append($"<p>&nbsp;</p><p style='text-align:center'>Bank Information</p>");
                    HtmlContent.Append($"<table style='width:100%;'>");
                    HtmlContent.Append($"<tr><td style='width:250px;'>Account Name</td><td>Suer Seyahat Acentasi Turizm Ltd. Sti.</td></tr>");
                    HtmlContent.Append($"<tr><td>Bank Name</td><td>Yapi Kredi Bank</td></tr>");
                    HtmlContent.Append($"<tr><td>Euro IBAN</td><td>TR81 0006 7010 0000 0085 2760 93</td></tr>");
                    HtmlContent.Append($"<tr><td>Swift Code</td><td>YAPITRIS072</td></tr>");
                    HtmlContent.Append($"<tr><td>Adres</td><td>Fisekhane Cad. Turkcu Sokak Kayali Apt. A-Blok No:6/1 D:7 Bakirkoy | istanbul&nbsp;</td></tr>");
                    HtmlContent.Append($"</table>");
                }

                mm.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = HtmlContent.ToString()
                };


                using (ProtocolLogger logger = new ProtocolLogger(HttpContext.Current.Server.MapPath($"~/Dosyalar/MailLog/{OModel.OdemeID}_{OModel.KatilimciBilgisi.ePosta}_{DateTime.Now:yyyy.MM.dd HH.mm.ss}.log")))
                {
                    using (SmtpClient client = new SmtpClient(logger))
                    {
                        try
                        {
                            client.Connect("mail.digiconkayit.com", 587, MailKit.Security.SecureSocketOptions.None);
                            client.Authenticate("acml2023@digiconkayit.com", "Wzyxe7A!");
                            client.Send(mm);

                        }
                        catch (Exception ex)
                        {
                            byte[] ExecptionBytes = Encoding.UTF8.GetBytes($"{ex.Message}\r\n");
                            logger.Stream.Write(ExecptionBytes, 0, ExecptionBytes.Length);
                        }
                        finally
                        {
                            client.Disconnect(true);
                        }
                    }
                }
            }
        }
    }
}