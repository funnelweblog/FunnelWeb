<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Safe.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Views.Admin.PingbacksModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	FunnelWeb Administration - Pingbacks
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Pingbacks</h1>
      
    <% foreach (var pingback in Model.Pingbacks) { %>
    <table width="90%" style="margin: 5px;background:<%= pingback.IsSpam ? "#fff0f0;" : "#f0f0f0;" %>">
    <tr>
        <td width="75%">
            <strong><%= Html.Encode(pingback.Entry.Title) %></strong><br />
            <em><%= Html.Encode(pingback.TargetTitle) %></em> | 
            <a href="<%= Html.Encode(pingback.TargetUri) %>"><%= Html.Encode(pingback.TargetUri) %></a>
        </td>
        <td width="25%">
            <%= Html.ActionLink("Delete", "DeletePingback", "Admin", new {pingback = pingback.Id}, null) %>
            <%= Html.ActionLink(pingback.IsSpam ? "Unspam" : "Spam", "TogglePingbackSpam", "Admin", new {pingback = pingback.Id}, null) %>
        </td>
    </tr>
    </table>
    <% } %>

<% Html.RequiresJs("/Views/Admin/Scripts/Admin.js", 2); %>

</asp:Content>
