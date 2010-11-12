<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Safe.Master" Inherits="FunnelWeb.Web.Application.Views.ApplicationView<FunnelWeb.Web.Features.Admin.AdminController.IndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	FunnelWeb Administration
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Administration</h1>

    <div class="ui-widget"> 
			<div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;"> 
				<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span> 
				<strong>Remember:</strong> you'll need to click 'save' on each tab; we're not smart enough to do it for you yet.</p> 
			</div>
		</div> 
    <br />

    <div id="tabs">
	    <ul>
		    <li><a href="#tabs-1">General Settings</a></li>
		    <li><a href="#tabs-2">Database</a></li>
		    <li><a href="#tabs-3">Comments</a></li>
		    <li><a href="#tabs-4">Pingbacks</a></li>
	      <li><a href="#tabs-5">Feeds</a></li>
	      <li><a href="#tabs-6">Redirects</a></li>
		  </ul>

      <!-- General -->
	    <div id="tabs-1">
        <% using (Html.BeginForm("UpdateSettings", "Admin", FormMethod.Post, new { @class = "promptBeforeUnload" })) { %>
        <div class="form-body">
            <h3>Site Information</h3>
            <p>
                <%= Html.Label("Title", "settings_ui-title")%>
                <%= Html.InputTextBox("settings_ui-title").Default(Settings.SiteTitle).Large().IsRequired()%>
                <span class="hint">The title shown at the top in the browser</span>
            </p>
            <p>
                <%= Html.Label("Introduction", "settings_ui-introduction")%>
                <%= Html.InputTextArea("settings_ui-introduction").Default(Settings.Introduction).Medium().IsRequired()%>
                <span class="hint">The welcome text that is shown on the home page. You can use markdown.</span>
            </p>

            <h3>Appearance</h3>
            <p>
                <%= Html.Label("Header Links", "settings_ui-links")%>
                <%= Html.InputTextArea("settings_ui-links").Default(Settings.MainLinks).Medium()%>
                <span class="hint">A list of links shown at the top of each page. Use HTML for this.</span>
            </p>
            <p>
                <%= Html.Label("Footer", "settings_ui-footer")%>
                <%= Html.InputTextArea("settings_ui-footer").Default(Settings.Footer).Medium()%>
                <span class="hint">This will appear at the bottom of the page - use it to add copyright information, links to any web hosts, people or technologies that helped you to build the site, and so on.</span>
            </p>

            <p>
                <%= Html.Label("Theme", "settings_ui-theme") %>
                <%= Html.DropDownList("settings_ui-theme", Model.Themes.Select(x =>
                    new SelectListItem { Text = x, Selected = Settings.Theme == x })
                                    )%>
                <span class="hint">The theme which will be used for this website</span>
            </p>

            <h3>Metadata</h3>
            <p>
                <%= Html.Label("Author", "settings_search-author")%>
                <%= Html.InputTextBox("settings_search-author").Default(Settings.Author).Medium().IsRequired()%>
                <span class="hint">Your name. Rendered as a meta tag.</span>
            </p>
            <p>
                <%= Html.Label("Keywords", "settings_search-keywords")%>
                <%= Html.InputTextBox("settings_search-keywords").Default(Settings.SearchKeywords).Large().IsRequired()%>
                <span class="hint">Keywords shown to search engines (comma-separated text).</span>
            </p>
            <p>
                <%= Html.Label("Description", "settings_search-description")%>
                <%= Html.InputTextArea("settings_search-description").Default(Settings.SearchDescription).Medium()%>
                <span class="hint">The description shown to search engines in the meta description tag.</span>
            </p>

            <h3>Spam</h3>
            <p>
                <span class="hint">TODO: Move Akismet settings here (and take them out of web.config).</span>              
            </p>
            <p>
                <%= Html.Label("Spam Blacklist", "settings_spam-blacklist")%>
                <%= Html.InputTextArea("settings_spam-blacklist").Default(Settings.SpamWords).Medium()%>
                <span class="hint">Comments with these words (case-insensitive) will automatically be marked as spam, in addition to Akismet.</span>
            </p>
            
            <h3>Save</h3>
            <p>
                <input type="submit" id="submit" class="submit" value="Save changes to General Settings" />
            </p>
        </div>
        <% } %>

	    </div>
	    
      <!-- Database -->
      <div id="tabs-2">
        <p>
          The database connection settings can be managed through <%= Html.ActionLink("the installation tool", "Index", "Install") %>.
        </p>
	    </div>

      <!-- Comments -->
	    <div id="tabs-3">
		      <p>
            FunnelWeb comments are automatically sent to <strong><a href="http://akismet.com">Akismet</a></strong> for spam filtering.
            You can also add your own blacklist of naughty words considered as spam on the <strong>General Settings</strong> tab.
          </p>
          
          <p>
            <%= Html.ActionLink("Delete all Spam", "DeleteAllSpam", "Admin", null, null) %>
          </p>
    
          <% foreach (var comment in Model.Comments) { %>
          <table width="90%" style="margin: 5px;background:<%= comment.IsSpam ? "#fff0f0;" : "#f0f0f0;" %>">
          <tr>
              <td colspan="2" width="90%">
                  <strong><%= Html.Encode(comment.Entry.Title) %></strong><br />
                  <%= Html.DisplayFor(_ => comment.Body, new { Sanitize = true })%>
              </td>
          </tr>
          <tr>
              <td width="75%">
                  <%= string.Join("<br />", new[] { "<strong>" + Html.Encode(comment.AuthorName) + "</strong>", Html.Encode(comment.AuthorEmail), Html.Encode(comment.AuthorUrl) }.Where(x => !string.IsNullOrEmpty(x)).ToArray() ) %>
              </td>
              <td width="25%">
                  <%= Html.ActionLink("Delete", "DeleteComment", "Admin", new {comment = comment.Id}, null) %>
                  <%= Html.ActionLink(comment.IsSpam ? "Unspam" : "Spam", "ToggleSpam", "Admin", new {comment = comment.Id}, null) %>
              </td>
          </tr>
          </table>
          <% } %>
          
      </div>

      <!-- Pingbacks -->
      <div id="tabs-4">
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

      </div>

      <!-- Feeds -->
      <div id="tabs-5">
          <% using (Html.BeginForm("CreateFeed", "Admin", FormMethod.Post, new { @class = "promptBeforeUnload" })) { %>    
          <table>
              <thead>
              <tr>
                  <td>Name</td>
                  <td>Title</td>
                  <td>Entries</td>
                  <td>Actions</td>
              </tr>
              </thead>
              <% foreach (var item in Model.Feeds) { %>
              <tr>
                  <td><%= item.Name %></td>
                  <td><%= item.Title %></td>
                  <td><% = item.Items.Count %></td>
                  <td><%= Html.ActionLink("Delete", "DeleteFeed", new { feedId = item.Id }) %></td>
              </tr>
              <% } %>
          </table>
        
          <div class="form-body">
              <p>
                  <%= Html.Label("Name", "name")%>
                  <%= Html.InputTextBox("name").Default("").IsRequired()%>
              </p>
              <p>
                  <%= Html.Label("Title", "title")%>
                  <%= Html.InputTextBox("title").Default("").IsRequired()%>
              </p>
        
              <p>
                  <input type="submit" id="submit1" class="submit" value="Create this Feed" />
              </p>
          </div>
          <% } %>
      </div>

      <!-- Redirects -->
      <div id="tabs-6">
          <% using (Html.BeginForm("CreateRedirect", "Admin", FormMethod.Post, new { @class = "promptBeforeUnload" })) { %>
    
          <table>
              <thead>
              <tr>
                  <td>From</td>
                  <td>To</td>
                  <td>Actions</td>
              </tr>
              </thead>
              <% foreach (var item in Model.Redirects) { %>
              <tr>
                  <td><%= item.From %></td>
                  <td><%= item.To %></td>
                  <td><%= Html.ActionLink("Delete", "DeleteRedirect", new{redirectId = item.Id}) %></td>
              </tr>
              <% } %>
          </table>
        
          <div class="form-body">
              <p>
                  <%= Html.Label("From", "from")%>
                  <%= Html.InputTextBox("from").Default("").Large().IsRequired()%>
              </p>
              <p>
                  <%= Html.Label("To", "to")%>
                  <%= Html.InputTextBox("to").Default("").Large().IsRequired()%>
              </p>
        
              <p>
                  <input type="submit" id="submit2" class="submit" value="Create this Redirect" />
              </p>
          </div>
          <% } %>

      
      </div>
    </div>

<% Html.RequiresJs("/Views/Admin/Scripts/Admin.js", 2); %>

</asp:Content>
