<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Safe.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Controllers.LoginController.IndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <% if (Model.DatabaseIssue) { %>
    <h1>Database Issue</h1>
    <p>
      The database used by your FunnelWeb installation is either offline, out of date or has not been 
      configured correctly. To resolve this issue, you will need to log in.
    </p>
    <%} else { %>
    <h1>Login</h1>
    <p>
        To administer this site, please log in using the form below.
    </p>
    <%} %>
    
    <%= Html.Flashes() %>
    
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

