<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Dialog.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Features.Uploads.Views.IndexModel>" %>

<asp:Content ContentPlaceHolderID="TitleContentPlaceHolder" runat="server">Uploads</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2>Directory Listing: <% foreach (var part in ViewData.Model.Path) {%>/<%= Html.ActionLink(part.Name, "Index", new { path = part.Path })%><% } %></h2>
    
    <table>
        <thead>
            <tr>
                <td></td>
                <td style="min-width: 300px">Name</td>
                <td>Extension</td>
                <td>Size</td>
                <td>Modified</td>
                <td>Actions</td>
            </tr>
        </thead>
        
        <% foreach (var item in ViewData.Model.Items) { %>
            <tr>
                <td><img width="16" height="16" src="/Views/Shared/Images/FileTypes/<%= item.Image%>" alt="<%= item.Extension %>" /></td>
                <td class="firstcolumn">
                    <% if (item.IsDirectory) { %>
                        <%= Html.ActionLink(item.Name, "Index", new { path = item.Path })%>
                    <% } else { %>
                        <%= Html.ActionLink(item.Name, "Render", new { path = item.Path })%>
                    <% } %>
                </td>
                <td><%= item.Extension %></td>
                <td class="numeric"><%= item.FileSize %></td>
                <td class="date"><%= item.Modified %></td>
                <td>
                    <%= Html.ActionLink("Delete", "Delete", new { path = Model.PathString, filePath = item.Path, })%>
                </td>
            </tr>
        <% } %>    
    </table>
    
    <h2>Upload</h2>
    
    <% using (Html.BeginForm("Upload", "Upload", FormMethod.Post, new { enctype = "multipart/form-data" })) {%>
        <input type="hidden" name="path" value="<%= Model.PathString %>" />
        <input type="file" name="upload" />
        <input type="submit" value="Submit" />
    <%} %>
    
    <h2>Create Directory</h2>
    
    <% using (Html.BeginForm("CreateDirectory", "Upload", FormMethod.Post, new { enctype = "multipart/form-data" })) {%>
        <input type="hidden" name="path" value="<%= Model.PathString %>" />
        <input type="text" name="name" />
        <input type="submit" value="Submit" />
    <%} %>
    
</asp:Content>
