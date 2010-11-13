<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Safe.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Features.Admin.Views.FeedsModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	FunnelWeb Administration - Feeds
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Feeds</h1>

    <!-- Feeds -->
    <% using (Html.BeginForm("Feeds", "Admin", FormMethod.Post, new { @class = "promptBeforeUnload" })) { %>    
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
        <div class="editor-label">
          <%: Html.LabelFor(m => m.FeedName)%>
        </div>
        <div class="editor-field">
          <%: Html.TextBoxFor(m => m.FeedName, Html.AttributesFor(m => m.FeedName))%>
          <%: Html.ValidationMessageFor(m => m.FeedName)%>
          <%: Html.HintFor(m => m.FeedName)%>
        </div>
        
        <div class="editor-label">
          <%: Html.LabelFor(m => m.FeedTitle)%>
        </div>
        <div class="editor-field">
          <%: Html.TextBoxFor(m => m.FeedTitle, Html.AttributesFor(m => m.FeedTitle))%>
          <%: Html.ValidationMessageFor(m => m.FeedTitle)%>
          <%: Html.HintFor(m => m.FeedTitle)%>
        </div>
        
        <div class="editor-label">
        </div>
        <div class="editor-field">
            <input type="submit" id="submit1" class="submit" value="Create this Feed" />
        </div>
    </div>
    <% } %>

<% Html.RequiresJs("/Views/Admin/Scripts/Admin.js", 2); %>

</asp:Content>
