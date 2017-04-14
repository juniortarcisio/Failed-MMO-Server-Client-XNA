<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/MmoMaster.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Screenshots
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Screenshots</h2>
    
    <img src="<%= Url.Content("~/Content/g1.jpg") %>"
</asp:Content>
