<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Safe.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Features.Admin.Views.FeedsModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	FunnelWeb Administration - Feeds
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Feeds</h1>

    <!-- Feeds -->
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
            <%= Html.Label("Name")%>
            <%= Html.TextBox("name")%>
        </p>
        <p>
            <%= Html.Label("Title")%>
            <%= Html.TextBox("title")%>
        </p>
        
        <p>
            <input type="submit" id="submit1" class="submit" value="Create this Feed" />
        </p>
    </div>
    <% } %>

<% Html.RequiresJs("/Views/Admin/Scripts/Admin.js", 2); %>

</asp:Content>
