<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Safe.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Controllers.InstallController.IndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	FunnelWeb Installation
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>
      FunnelWeb <%= Model.IsInstall ? "Installer" : "Upgrader" %>
    </h1>
    
    <p class='important'>
      Welcome to your FunnelWeb installation! 
    </p>
    <p>
      This page allows you to edit the SQL Server connection string, and install or upgrade the database. You will see this page any time you upgrade FunnelWeb.
    </p>

        
    <h2>Change Connection String</h2>
    <% if (Model.CanConnect) { %>
    <p class='good'>The connection string below appears to work. However, you can change it if you wish to 
    use an alternative database.</p>
    <%} else {%>
    <p class='bad'>The connection string below is invalid. You will need to change it before proceeding with this <%= Model.IsInstall ? "install" : "upgrade" %>. The error was:</p>
    <pre><%= Model.ConnectionError %></pre>
    <%} %>
    
    <% using (Html.BeginForm("Test", "Install", FormMethod.Post)) { %>
    <div class="form-body">
        <p>
          <%= Html.Label("Connection", "connectionString")%>
          <%= Html.InputTextArea("connectionString").Default(Model.ConnectionString).IsRequired()%>
          <span class="hint">Enter the connection string to the Microsoft SQL Server database given to you by your web host.</span>
        </p>
        <p>
            <input type="submit" id="submit" class="submit" value="Save and Test" />
        </p>
    </div>
    <%} %>

    <% if (Model.CanConnect) { %>    
    
      <h2><%= Model.IsInstall ? "Proceed with Installation" : "Proceed with Upgrade" %></h2>
      <p>
        The FunnelWeb <%= Model.IsInstall ? "installer" : "upgrader" %> is now ready to <%= Model.IsInstall ? "install" : "upgrade" %> your database.
      </p>

      <table>
        <thead>
          <tr>
            <td colspan='2'><%= Model.IsInstall ? "Install" : "Upgrade" %> Information</td>
          </tr>
        </thead>
        <tr>
          <td>Current FunnelWeb database version: </td>
          <td><%= Model.CanConnect ? Model.CurrentVersion.ToString() : "N/A" %></td>
        </tr>
        <tr>
          <td>Can upgrade to: </td>
          <td><%= Model.CanConnect ? Model.NewVersion.ToString() : "N/A"%></td>
        </tr>
      </table>

      <% if (Model.CurrentVersion == Model.NewVersion) {%>
        <p class='good'>Your database is up to date. No <%= Model.IsInstall ? "install" : "upgrade" %> is necessary.</p>
        <p class='important'><a href="~/" runat="server">Sweet!</a></p>
      <%} else {%>
      <p class='warning'>
        Reminder: make sure to perform a backup of your database before proceeding with this <%= Model.IsInstall ? "installation" : "upgrade" %>.
      </p>
      <% using (Html.BeginForm("Upgrade", "Install", FormMethod.Post)) { %>
      <div class="form-body">
          <p>
              <input type="submit" id="submit1" class="submit" value="Upgrade to version <%= Model.NewVersion %>" />
          </p>
      </div>
      <%} %>
      
      <%} %>
      
    <%} %>

</asp:Content>
