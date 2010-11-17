<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Views.Wiki.RevisionsModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server"><%= Html.Settings().SiteTitle %> - <%= Model.Entry.Title %> - History</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MetaContent" runat="server">
  <meta name="description" content="Revision History of <%= Model.Entry.Title %>" />
  <meta name="robots" content="noindex, nofollow" />
  <link rel="canonical" href="<%= Html.Qualify(Html.ActionUrl("Page", new { page = Model.Entry.Name })) %>" />
</asp:Content>

<asp:Content ContentPlaceHolderID="SummaryContent" runat="server">
    Each page on this site has a history, which contains a record of all changes made to the page content.
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h1><%= Model.Entry.Title %> - History</h1>
    <p>
        Below is a list of all changes made to the <%= Model.Entry.Name %> page. You can click on an individual item to 
        see the page as it was.
    </p>
    
    <table>
        <thead>
            <tr>
                <td></td>
                <td><abbr title="When the change was made">Revised</abbr></td>
                <td><abbr title="Reasons explaining why the page was changed">Comments</abbr></td>
            </tr>
        </thead>
        <tbody>
    <% foreach (var revision in Model.Entry.Revisions) { %>
            <tr>
                <td valign="top"><%: revision.RevisionNumber %></td>
                <td valign="top" align="right"><a href="<%: Html.ActionUrl("Page", new { page = Model.Page, revision = revision.RevisionNumber })%>"><%=Html.Date(revision.Revised)%></a></td>
                <td valign="top"><%: Html.TextilizeList(revision.Reason)%></td>
            </tr>
    <% } %>
        </tbody>
    </table>

</asp:Content>

