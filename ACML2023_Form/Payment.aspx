<%@ Page Title="" Language="C#" MasterPageFile="~/EN.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="ACML2023_Form.Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%= ResolveClientUrl("~/js/jquery.inputmask.min.js") %>"></script>
    <script>
        $(() => {
            $('#<%= txtKrediKartNo.ClientID%>').inputmask('9999 9999 9999 9999', { onincomplete: () => { $('#<%= txtKrediKartNo.ClientID%>').val(''); } });
            $('#<%= txtAy.ClientID%>').inputmask('99', { onincomplete: () => { $('#<%= txtAy.ClientID%>').val(''); } });
            $('#<%= txtYil.ClientID%>').inputmask('99', { onincomplete: () => { $('#<%= txtYil.ClientID%>').val(''); } });
            $('#<%= txtCvv2.ClientID%>').inputmask('999', { onincomplete: () => { $('#<%= txtCvv2.ClientID%>').val(''); } });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <fieldset>
                <legend>Credit Card & Payment Information</legend>
                <table class="AlseinTable">
                    <tr>
                        <td>&nbsp;</td>
                        <td>Participant</td>
                        <td>
                            <asp:Label ID="lblAdSoyad" runat="server" Text="Label" CssClass="form-control"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>Registration Type</td>
                        <td>
                            <asp:Label ID="lblKatilimciTipi" runat="server" Text="" CssClass="form-control"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>Total Grand</td>
                        <td>
                            <asp:Label ID="lblKatilimciTipiUcret" runat="server" Text="" CssClass="form-control"></asp:Label>
                            <asp:HiddenField ID="hfKatilimciTipiUcret" runat="server" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>Order No</td>
                        <td>
                            <asp:Label ID="lblOdemeID" runat="server" Text="" CssClass="form-control"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>*</td>
                        <td>Credit Card No</td>
                        <td>
                            <asp:TextBox ID="txtKrediKartNo" runat="server" CssClass="form-control" placeholder="xxxx xxxx xxxx xxxx"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="row">
                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-12">
                        <table class="AlseinTable">
                            <tr>
                                <td>*</td>
                                <td>Exp. Month</td>
                                <td>
                                    <asp:TextBox ID="txtAy" runat="server" CssClass="form-control"  placeholder="xx"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-12">
                        <table class="AlseinTable">

                            <tr>
                                <td>*</td>
                                <td>Exp. Year</td>
                                <td>
                                    <asp:TextBox ID="txtYil" runat="server" CssClass="form-control" placeholder="xx"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-12">
                        <table class="AlseinTable">
                            <tr>
                                <td>*</td>
                                <td>CVV2</td>
                                <td>
                                    <asp:TextBox ID="txtCvv2" runat="server" CssClass="form-control"  placeholder="xxx"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <p class="mt-3" align="center">
                    <asp:LinkButton ID="lnkbtnOdemeYap" runat="server" CssClass="btn btn-success" OnClick="lnkbtnOdemeYap_Click" OnClientClick="$(this).css('display', 'none'); $('.LoadingIcon').css('display', 'inline-block');">
                        <i class="fa fa-check"></i>&nbsp;Complete Payment
                    </asp:LinkButton>
                    <asp:Image ID="ImgLoding" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" CssClass="LoadingIcon" />
                </p>
            </fieldset>
        </div>
    </div>
</asp:Content>
