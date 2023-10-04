<%@ Page Title="" Language="C#" MasterPageFile="~/EN.Master" AutoEventWireup="true" CodeBehind="PaymentFail.aspx.cs" Inherits="ACML2023_Form.PaymentFail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
    <div class="col-md-12">
        <fieldset>
            <legend>Registration Completed</legend>
            <p class="text-center">
                <asp:Image ID="ImgFail" runat="server" CssClass="InfoLogo" ImageUrl="~/Gorseller/Failed.png" style="max-width:250px;"/>
            </p>
            <div class="text-center">
                Dear
                <asp:Label ID="lblAdSoyad" runat="server" Text=""></asp:Label>,
            </div>
            <div class="text-center">
                There is an error occured while payment process
            </div>
            <div class="text-center">
                Order No: <b><asp:Label ID="lblOdemeID" runat="server" Text=""></asp:Label></b>
            </div>
            <p class="text-center">
                <asp:HyperLink ID="hyplnkReturn" runat="server" CssClass="btn btn-info" NavigateUrl="~/">
                    <i class="fa fa-arrow-left"></i>&nbsp;Return to Registration Page
                </asp:HyperLink>
            </p>
        </fieldset>
    </div>
</div>
</asp:Content>
