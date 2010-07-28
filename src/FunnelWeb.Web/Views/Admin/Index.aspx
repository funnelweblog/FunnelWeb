<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Safe.Master" Inherits="FunnelWeb.Web.Application.Views.ApplicationView<FunnelWeb.Web.Controllers.AdminController.IndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	FunnelWeb Administration
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Administration</h1>

    <h2>General Settings</h2>

    <% using (Html.BeginForm("UpdateSettings", "Admin", FormMethod.Post, new { @class = "promptBeforeUnload" })) { %>
        <div class="form-body">
            
            <% foreach (var setting in Model.Settings) { %>
            <p>
                <%= Html.Label(setting.DisplayName, "settings_" + setting.Name)%>
                <%= Html.InputTextArea("settings_" + setting.Name).Default(setting.Value).IsRequired()%>
                <span class="hint"><%= setting.Description %></span>
            </p>
            <% } %>
            
            <p>
                <input type="submit" id="submit" class="submit" value="Save" />
            </p>
        </div>
    <% } %>
    
    <h2>Feeds</h2>
    
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
            <input type="submit" id="submit1" class="submit" value="Create" />
        </p>
    </div>
    <% } %>
    
    <h2>Comments</h2>
    
    <p>
      <%= Html.ActionLink("Delete all Spam", "DeleteAllSpam", "Admin", null, null) %>
    </p>
    
    <% foreach (var comment in Model.Comments) { %>
    <table width="90%" style="margin: 5px;background:<%= comment.IsSpam ? "#fff0f0;" : "#f0f0f0;" %>">
    <tr>
        <td colspan="2" width="90%">
            <strong><%= Html.Encode(comment.Entry.Title) %></strong><br />
            <%= Markdown.Render(comment.Body, true)%>
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
    
    <h2>Pingbacks</h2>
    
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
    
    <h2>Redirects</h2>
    
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
            <input type="submit" id="submit2" class="submit" value="Create" />
        </p>
    </div>
    <% } %>

</asp:Content>
