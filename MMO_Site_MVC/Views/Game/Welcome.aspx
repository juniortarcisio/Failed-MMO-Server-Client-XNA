<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/MmoMaster.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	MMO - Welcome
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Welcome</h2>
    <h4>Welcome to the best MMORPG ever</h4>
    <h5><span class="firstLetter">T</span>his page is a prototype of the page to the best MMORPG made ever. This MMORPG is still under development, we still dont know how long it'll take,
    but we advice you to have patience male, because it will really worth the price. </h5>

    <img src="<%= Url.Content("~/Content/g1.jpg") %>" />
</asp:Content>
