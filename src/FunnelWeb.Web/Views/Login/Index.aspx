<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="FunnelWeb.Web.Application.Views.ApplicationView<FunnelWeb.Web.Controllers.LoginController.IndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	<%= Settings.SiteTitle %> - Login
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h1>Login</h1>
    <p>
        To administer this site, please log in using the form below.
    </p>
    
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
            <p>
                <input type="submit" id="submit" class="submit" value="Submit" />
            </p>
        </div>
        
    <% } %>
</asp:Content>

