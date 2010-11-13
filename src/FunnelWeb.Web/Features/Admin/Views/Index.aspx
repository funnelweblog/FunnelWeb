<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Safe.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Features.Admin.Views.IndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	FunnelWeb Administration
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Administration</h1>

    TODO: list of admin links

<% Html.RequiresJs("/Views/Admin/Scripts/Admin.js", 2); %>

</asp:Content>
