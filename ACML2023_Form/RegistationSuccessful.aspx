<%@ Page Title="" Language="C#" MasterPageFile="~/EN.Master" AutoEventWireup="true" CodeBehind="RegistationSuccessful.aspx.cs" Inherits="ACML2023_Form.RegistationSuccessful" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <fieldset>
                <legend>Registration Completed</legend>
                <p class="text-center">
                    <asp:Image ID="ImgOk" runat="server" CssClass="InfoLogo" ImageUrl="~/Gorseller/tick.png" style="max-width:250px;"/>
                </p>
                <div class="text-center">
                    Dear
                    <asp:Label ID="lblAdSoyad" runat="server" Text=""></asp:Label>,
                </div>
                <div class="text-center">
                    Your registration is completed.
                </div>
                <div class="text-center">
                    Order No: <b><asp:Label ID="lblOdemeID" runat="server" Text=""></asp:Label></b>
                </div>
                <asp:Panel ID="PnlBankaHavalesi" runat="server" Visible="false" CssClass="mt-2">
                    <fieldset>
                        <legend>Bank Information</legend>
                        <table class="AlseinTable">
                            <tbody>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>Account Name</td>
                                    <td>Suer Seyahat Acentasi Turizm Ltd. Sti.</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>Bank Name</td>
                                    <td>Yapi Kredi Bank</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>Euro IBAN</td>
                                    <td>TR81 0006 7010 0000 0085 2760 93</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>Swift Code</td>
                                    <td>YAPITRIS072</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>Adres</td>
                                    <td>Fisekhane Cad. Turkcu Sokak Kayali Apt. A-Blok No:6/1 D:7 Bakirkoy | istanbul&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                    </fieldset>
                </asp:Panel>
            </fieldset>
        </div>
    </div>
</asp:Content>
