<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Site.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Features.Wiki.Views.NotFoundModel>" %>

<script runat="server">
  protected override void OnLoad(EventArgs e)
  {
      base.OnLoad(e);
      if (Model.Is404)
      {
          Response.StatusCode = 404;
      }
  }
</script>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">Search: <%= Html.Encode(Model.SearchText) %></asp:Content>
<asp:content contentplaceholderid="SummaryContent" runat="server">
      <p>
        A team of ninjas have scoured the land, looking for the results to your query. 
      </p>
</asp:content>
      
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
      <h1>Search Results: <%= Html.Encode(Model.SearchText) %></h1>
      
      <% if (Model.Is404) { %>
      <p>
        The page you requested does not exist. Below are some pages that may be related. If you still can't find 
        what you are looking for, you can <%= Html.ActionLink("contact me", "Page", "Wiki", new {page = "contact"}, null) %>.
      </p>
      <% } %>
      
      <div class="search-input">
        <% using (Html.BeginForm("Search", "Wiki", FormMethod.Get)) { %>
        <label for="q">Search again:</label> <%= Html.InputTextBox("q").Default(Model.SearchText).Large() %>
        <input type="submit" id="research" class="submit" value="Search" />
        <% } %>
      </div>
      
      <% if (Model.Results.Count() == 0) { %>
        <p>
          Sorry, there were no results for your query.
        </p>
      <% } else { %>
      
      <div class="search-list"><% foreach (var entry in Model.Results) { %>
        <div class='search-line'>
          <h2><%= Html.Qualify(Html.ActionLink(entry.Title, "Page", new { page = entry.Name }))%></h2><% if (!string.IsNullOrEmpty(entry.MetaDescription)) { %>
          <p><%= Html.Encode(entry.MetaDescription) %></p><% } %>
          <div class='revised'><span class='month'><%= entry.FeedDate.ToString("MMM d")%></span> <span class='year'><%= entry.FeedDate.ToString("yyyy")%></span></div>
        </div><% } %>
      </div>
      <% } %>
      
</asp:Content>
