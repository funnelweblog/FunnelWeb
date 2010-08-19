<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="FunnelWeb.Web.Application.Views.ApplicationView<FunnelWeb.Web.Controllers.WikiController.PageModel>" %>

<asp:content contentplaceholderid="TitleContent" runat="server"><%= Model.Entry.MetaTitle %></asp:content>
<asp:content contentplaceholderid="MetaContent" runat="server">
  <meta name="description" content="<%= Model.Entry.MetaDescription %>" />
  <meta name="keywords" content="<%= Model.Entry.MetaKeywords %>" /><% if (Model.IsPriorVersion) { %><%--We ask Mr. Google very nicely to please not index this page.--%>
  <meta name="robots" content="noindex, nofollow" /><% } %>
  <link rel="canonical" href="<%= Html.Qualify(Html.ActionUrl("Page", new { page = Model.Entry.Name })) %>" />
  <link rel="pingback" href="<%= Html.Qualify("/pingback") %>" />
</asp:content>
<asp:content contentplaceholderid="SummaryContent" runat="server"><%= Html.DisplayFor(model => model.Entry.Summary) %></asp:content>
<asp:content contentplaceholderid="MainContent" runat="server">      
      <h1><%= Html.ActionLink(Model.Entry.Title, "Page", new { page = Model.Entry.Name })%></h1>
      <div class="entry-date">
        <span class='month'><%= Model.Entry.FeedDate.ToString("MMM d") %></span>
        <span class='year'><%= Model.Entry.FeedDate.ToString("yyyy")%></span>
      </div>
      <div class="entry-container">
        <% if (Model.IsPriorVersion) { %>
        <div class='entry-tools'>
          <div>
            <span>You are looking at revision <%= Model.Entry.LatestRevision.RevisionNumber %> of this page, which may be out of date. <%= Html.ActionLink("View the latest version.", "Page", new { page = Model.Entry.Name })%></span>
          </div>
        </div>
        <% } %>
        <div class='entry'>
        <%= Html.DisplayFor(model => model.Entry.LatestRevision.Body) %>
        </div>  
        <div class='entry-tools'>
          <span>Last revised: <a href="<%= Html.ActionUrl("Page", new { page = Model.Page, revision = Model.Entry.LatestRevision.RevisionNumber }) %>"><%= Html.Date(Model.Entry.LatestRevision.Revised) %></a></span>
          <span><%= Html.ActionLink("History", "Revisions", new { page = Model.Page })%></span>
          <% if (ViewData.IsLoggedIn()) { %>
          <span><%= Html.ActionLink("Edit", "Edit", new { page = Model.Page })%></span>
          <% } %>
          <% if (Model.IsPriorVersion) { %>
          <div>
            <span>You are looking at revision <%= Model.Entry.LatestRevision.RevisionNumber %> of this page, which may be out of date. <%= Html.ActionLink("View the latest version.", "Page", new { page = Model.Entry.Name })%></span>
          </div>
          <% } %>
        </div>
        <% if (Model.Entry.Pingbacks.Count > 0) { %>
        <div class="trackbacks">
          <h2>Trackbacks</h2>
          <ul><% foreach (var pingback in Model.Entry.Pingbacks.Where(x => !x.IsSpam)) { %>
            <li><%= Html.Encode(pingback.TargetTitle) %> | <a href="<%= Html.AttributeEncode(pingback.TargetUri) %>"><%= Html.Encode(pingback.TargetUri) %></a></li>
            <% } %>
          </ul>
        </div>
        <% } %>
      </div>
      <div class="clear"></div>
      <% if (Model.Entry.IsDiscussionEnabled) { %>
      <div class="comments">
        <div class="comments-in">
        <a name="comments" />
          <h2>Discussion</h2><% foreach (var comment in Model.Entry.Comments.Where(x => !x.IsSpam)) { %>
          <div class="comment">
            <div class="comment-author">
              <img class="gravatar" src="<%= Html.Gravatar(comment.AuthorEmail) %>" alt="<%= Html.Encode(comment.AuthorName) %>" />
              <br />
              <%= Html.UrlLink(comment.AuthorUrl, comment.AuthorName) %>
            </div>
            <div class="comment-body"> 
              <div class="comment-date"><%= Html.Date(comment.Posted) %></div>
              <%= Html.DisplayFor(_ => comment.Body, new { Sanitize = true }) %>
            </div>
            <div class="clear">
            </div>
          </div>
          <% } %>
        </div>
      </div>
      <h2>Your Comments</h2>
      <div class='entry-comment'>
      <% using (var form = Html.BeginForm("Comment", "Wiki", new { page = Model.Page }, FormMethod.Post, new { @class = "promptBeforeUnload" })) { %>
        <div class="form-body">
          <p>
            <%= Html.Label("Name", "name") %>
            <%= Html.InputTextBox("name").Default("").Medium().IsRequired()%>
          </p>
          <p>
            <%= Html.LabelOptional("URL", "url") %>
            <%= Html.InputTextBox("url").Default("http://").Large()%>
          </p>
          <p>
            <%= Html.LabelOptional("E-mail", "email") %>
            <%= Html.InputTextBox("email").Default("").Medium()%>
            <span class="hint">Used for your <a href="http://en.gravatar.com/">gravatar</a>. Will not be public.</span>
          </p>
          <p>
            <%= Html.Label("Comment", "wmd-input") %>
          </p>
            <%= Html.InputTextEditor("comments").IsRequired()%>
          <p>
            <span class="hint">Posting code? Indent it by four spaces to make it look nice. Learn more about <a href="http://daringfireball.net/projects/markdown/syntax">Markdown</a>.</span>
          </p>
          <p>
            <input type="submit" id="submit" class="submit" value="Submit" />
          </p>
          <p>
            <span class="notification-wait" id="submitnotification"></span>
          </p>
        </div>
        <% } %>
      </div>
      <h3>Preview</h3>
      <div id="wmd-preview" class="wmd-panel"></div>
      <% } %>
</asp:content>
