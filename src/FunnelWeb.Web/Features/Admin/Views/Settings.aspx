<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Safe.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Application.Settings.Settings>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">FunnelWeb Administration - Settings</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Settings</h1>

    <% using (Html.BeginForm("Settings", "Admin", FormMethod.Post, new { @class = "promptBeforeUnload" })) { %>
    <div class="form-body">
            
        <h3>Site Information</h3>

        <div class="editor-label">
          <%: Html.LabelFor(m => m.SiteTitle)%>
        </div>
        <div class="editor-field">
          <%: Html.TextBoxFor(m => m.SiteTitle, new { @class = "large" })%>
          <%: Html.ValidationMessageFor(m => m.SiteTitle) %>
          <%: Html.HintFor(m => m.SiteTitle) %>
        </div>
        
        <div class="editor-label">
          <%: Html.LabelFor(m => m.Introduction)%>
        </div>
        <div class="editor-field">
          <%: Html.TextAreaFor(m => m.Introduction, new { @class = "large" })%>
          <%: Html.ValidationMessageFor(m => m.Introduction)%>
          <%: Html.HintFor(m => m.Introduction)%>
        </div>
        
        <h3>Appearance</h3>
        
        <div class="editor-label">
          <%: Html.LabelFor(m => m.MainLinks)%>
        </div>
        <div class="editor-field">
          <%: Html.TextAreaFor(m => m.MainLinks, new { @class = "large" })%>
          <%: Html.ValidationMessageFor(m => m.MainLinks)%>
          <%: Html.HintFor(m => m.MainLinks)%>
        </div>

        <div class="editor-label">
          <%: Html.LabelFor(m => m.Footer)%>
        </div>
        <div class="editor-field">
          <%: Html.TextAreaFor(m => m.Footer, new { @class = "large" })%>
          <%: Html.ValidationMessageFor(m => m.Footer)%>
          <%: Html.HintFor(m => m.Footer)%>
        </div>

        <div class="editor-label">
          <%: Html.LabelFor(m => m.Theme)%>
        </div>
        <div class="editor-field">
          <%= Html.DropDownList("Theme", Model.Themes.Select(x =>
                new SelectListItem { Text = x, Selected = Theme == x })
                )%>
          <%: Html.ValidationMessageFor(m => m.Theme)%>
          <%: Html.HintFor(m => m.Theme)%>
        </div>

        <h3>Metadata</h3>

        <div class="editor-label">
          <%: Html.LabelFor(m => m.Author)%>
        </div>
        <div class="editor-field">
          <%: Html.TextBoxFor(m => m.Author, new { @class = "large" })%>
          <%: Html.ValidationMessageFor(m => m.Author)%>
          <%: Html.HintFor(m => m.Author)%>
        </div>
        
        <div class="editor-label">
          <%: Html.LabelFor(m => m.SearchKeywords)%>
        </div>
        <div class="editor-field">
          <%: Html.TextBoxFor(m => m.SearchKeywords, new { @class = "large" })%>
          <%: Html.ValidationMessageFor(m => m.SearchKeywords)%>
          <%: Html.HintFor(m => m.SearchKeywords)%>
        </div>
        
        <div class="editor-label">
          <%: Html.LabelFor(m => m.SearchDescription)%>
        </div>
        <div class="editor-field">
          <%: Html.TextBoxFor(m => m.SearchDescription, new { @class = "large" })%>
          <%: Html.ValidationMessageFor(m => m.SearchDescription)%>
          <%: Html.HintFor(m => m.SearchDescription)%>
        </div>

        <h3>Spam</h3>
        <div class="editor-label">
          <%: Html.LabelFor(m => m.SpamWords)%>
        </div>
        <div class="editor-field">
          <%: Html.TextAreaFor(m => m.SpamWords, new { @class = "large" })%>
          <%: Html.ValidationMessageFor(m => m.SpamWords)%>
          <%: Html.HintFor(m => m.SpamWords)%>
        </div>

        <h3>Save</h3>
        <p>
            <input type="submit" id="submit" class="submit" value="Save" />
        </p>
    </div>
    <% } %>

<% Html.RequiresJs("/Views/Admin/Scripts/Admin.js", 2); %>

</asp:Content>
