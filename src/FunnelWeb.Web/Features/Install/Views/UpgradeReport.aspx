<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Safe.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Features.Install.Views.UpgradeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FunnelWeb Installation Report
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
      FunnelWeb Upgrade Report
    </h1>
    
    <% if (Model.Result.Successful) { %>
      <p class='good'>The upgrade to version <%= Model.Result.UpgradedVersion %> completed successfully. Feel free to review the logs below before continuing.</p>
    <%} else {%>
      <p class='bad'>The upgrade failed. Please review the logs below to resolve the issue.</p>
    <%} %>

    <p class='important'>
      <%= Html.ActionLink("Try again", "Index") %>
    </p>

    <h2>Log</h2>
    <ol>
      <%= Model.Log.ToString() %>
    </ol>

    <% if (!Model.Result.Successful) { %>
    <h2>Exception Details</h2>
    <pre><%= Model.Result.Error == null ? "" : Model.Result.Error.ToString()%></pre>
    <% } %>
</asp:Content>
