<%@ Page Title="" Language="C#" MasterPageFile="~/EN.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ACML2023_Form.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UPnlGenel" runat="server" class="col-md-12">
        <ContentTemplate>
            <fieldset>
                <legend>Registration Form</legend>
                <table class="AlseinTable">
                    <tr>
                        <td>*</td>
                        <td>Name</td>
                        <td>
                            <asp:TextBox ID="txtAd" runat="server" CssClass="form-control" onchange="toUpper(this);" onkeyup="toUpper(this);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>Surname</td>
                        <td>
                            <asp:TextBox ID="txtSoyad" runat="server" CssClass="form-control" onchange="toUpper(this);" onkeyup="toUpper(this);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>Email</td>
                        <td>
                            <asp:TextBox ID="txtePosta" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>Telephone</td>
                        <td>
                            <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>Name of Institution</td>
                        <td>
                            <asp:TextBox ID="txtKurum" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>Country</td>
                        <td>
                            <asp:DropDownList ID="ddlUlke" runat="server" CssClass="form-control" DataSourceID="OleDbUlke" DataTextField="Ulke" DataValueField="Ulke"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>Do you have an accepted paper?</td>
                        <td>
                            <asp:DropDownList ID="ddlBildiriDurum" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlBildiriDurum_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="">Select</asp:ListItem>
                                <asp:ListItem Value="true">Yes</asp:ListItem>
                                <asp:ListItem Value="false">No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="tr_BildiriNo" runat="server" visible="false">
                        <td>*</td>
                        <td>Please write your paper number from CMT.</td>
                        <td>
                            <asp:TextBox ID="txtBildiriNo" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>Registration Type</td>
                        <td>
                            <asp:DropDownList ID="ddlKatilimciTipi" runat="server" CssClass="form-control" DataSourceID="OleDbKatilimciTipiListesi" DataTextField="KatilimciTipiWF" DataValueField="KatilimciTipiID"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>Payment Type</td>
                        <td>
                            <asp:DropDownList ID="ddlOdemeTipi" runat="server" CssClass="form-control" DataSourceID="OleDbOdemeTipiListesi" DataTextField="OdemeTipi" DataValueField="OdemeTipiID" OnSelectedIndexChanged="ddlOdemeTipi_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                    </tr>
                </table>

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
                                    <td>QNB Finansbank</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>Euro IBAN</td>
                                    <td>TR76 0011 1000 0000 0129 5308 06</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>Swift Code</td>
                                    <td>FNNBTRIS</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>Adres</td>
                                    <td>Cevizlik Mahallesi, Fahri Koruturk Caddesi Cemal is Hani, No:26/B, 34142 Bakirkoy/istanbul&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                    </fieldset>
                </asp:Panel>
                <p align="center">
                    <asp:LinkButton ID="lnkbtnKayitOl" runat="server" CssClass="btn btn-success" OnClick="lnkbtnKayitOl_Click" OnClientClick="$(this).css('display', 'none'); $('.LoadingIcon').css('display', 'inline-block');">
                        <i class="fa fa-check"></i>&nbsp;Complete Registration
                    </asp:LinkButton>
                    <asp:Image ID="ImgLoding" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" CssClass="LoadingIcon"/>
                </p>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:SqlDataSource runat="server" ID="OleDbUlke" ConnectionString='<%$ ConnectionStrings:OleDbConnectionString %>' ProviderName='<%$ ConnectionStrings:OleDbConnectionString.ProviderName %>' SelectCommand="SELECT * FROM [UlkeTablosu] ORDER BY [GrupNo], [Ulke]"></asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="OleDbKatilimciTipiListesi" ConnectionString='<%$ ConnectionStrings:OleDbConnectionString %>' ProviderName='<%$ ConnectionStrings:OleDbConnectionString.ProviderName %>' SelectCommand="SELECT [KatilimciTipiID], [KatilimciTipi] &' - '& IIF(NOW() < #10/19/2023 15:00:00#, [ErkenUcret], [NormalUcret]) & ' €' AS [KatilimciTipiWF] FROM [KatilimciTipiTablosu]"></asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="OleDbOdemeTipiListesi" ConnectionString='<%$ ConnectionStrings:OleDbConnectionString %>' ProviderName='<%$ ConnectionStrings:OleDbConnectionString.ProviderName %>' SelectCommand="SELECT [OdemeTipiID], [OdemeTipi] FROM [OdemeTipiTablosu]"></asp:SqlDataSource>

</asp:Content>
