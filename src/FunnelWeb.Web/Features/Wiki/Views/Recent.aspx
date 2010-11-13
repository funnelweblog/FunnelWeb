<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Site.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Features.Wikis.Views.RecentModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server"><%= Settings.SiteTitle %></asp:Content>
<asp:Content ContentPlaceHolderID="MetaContent" runat="server">
  <meta name="description" content="<%= Settings.SearchDescription %>" />
  <meta name="keywords" content="<%= Settings.SearchKeywords %>" />
</asp:Content>

<asp:Content ContentPlaceHolderID="SummaryContent" runat="server">
<%= Html.DisplayFor(_ => Settings.Introduction)%>
</asp:Content>
 
<asp:Content ContentPlaceHolderID="MainContent" runat="server"><h1>Recent Posts</h1>
      <div class="post-list"><% foreach (var entry in Model.Entries) { %>
        <div class='post-line'>
          <div class='revised'><span class='month'><%= entry.FeedDate.ToString("MMM d")%></span> <span class='year'><%= entry.FeedDate.ToString("yyyy")%></span></div>
          <div class='comments'>
          <a href='<%= Url.Action("Page", new { page = entry.Name }) + "#comments" %>'>
            <span class='comment-count'><%= entry.CommentCount %></span>
            <span>comments</span> 
          </a>
          </div>
          <div class='summary'>
            <h2><%= Html.Qualify(Html.ActionLink(entry.Title, "Page", new { page = entry.Name }))%></h2><% if (!string.IsNullOrEmpty(entry.MetaDescription)) { %>
            <p><%= Html.Encode(entry.MetaDescription) %></p><% } %>
          </div>
        </div><% } %>
      </div>
      <% if (Model.TotalPages > 1) { %>
      <div class="paginator">
        <span class="title">Page: </span><% for (var i = 0; i < Model.TotalPages; i++) { %>
        <span class="page"><% if (i == Model.PageNumber) { %><%= (i + 1).ToString()%><% } else if (i == 0) { %><%= Html.ActionLink((i + 1).ToString(), "Recent") %> <% } else { %> <%= Html.ActionLink((i + 1).ToString(), "Recent", new { pageNumber = i }) %> <% } %></span><% } %>
      </div>
      <% } %>

</asp:Content>

