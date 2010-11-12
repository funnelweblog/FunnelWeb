<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Safe.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.LoginController.IndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <% if (Model.DatabaseIssue) { %>
    <h1>Database Issue</h1>
    <p class='bad'>
      The database used by your FunnelWeb installation is either offline, out of date or has not been 
      configured correctly. To resolve this issue, you will need to log in with the username and password 
      from your web.config file.
    </p>
    <%} else { %>
    <h1>Login</h1>
    <p class='good'>
        To administer this site, please log in using the form below. The username and password are generally
        stored in your web.config file.
    </p>
    <%} %>
    
    <% if (Model.PreviousLoginFailed) { %>
    <p class="message">
    The username or password could not be authenticated. Please try again.
    </p>
    <% } %>
    
    <% using (Html.BeginForm("Login", "Login")) { %>
        
        <div class="form-body">
            <p>
                <%= Html.Label("Name", "name") %>
                <%= Html.InputTextBox("name").Default("").Medium().IsRequired()%>
            </p>
            <p>
                <%= Html.Label("Password", "password") %>
                <%= Html.Password("password", string.Empty, new {@class="required"})%>
            </p>
            <%= Html.Hidden("databaseIssue", Model.DatabaseIssue) %>
            <p>
                <input type="submit" id="submit" class="submit" value="Submit" />
            </p>
        </div>
        
    <% } %>
</asp:Content>

