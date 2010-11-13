<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Site.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Features.Wiki.Views.EditModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server"><%: Html.Settings().SiteTitle %> - Edit - <%: Model.Title %></asp:Content>
<asp:Content ContentPlaceHolderID="MetaContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="SummaryContent" runat="server">

    <% if (Model.IsNew) { %>     
    <p>The page you navigated to has not been created yet. Use the editor below to create the first version of this page.</p>
    <% } else { %>
    <p>Use the editor below to update this page. Alternatively, you can <%= Html.ActionLink("go back to the previous page", "Page", new{page = Model.Page}) %>.</p>
    <% } %>

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <% if (Model.IsNew) { %>     
    <h1>Create: <%: Model.Title%></h1>
    <% } %>
    <% else { %>     
    <h1>Edit: <%: Model.Title%></h1>
    <% } %>
    
    <%: Html.ValidationSummary("Edit unsuccessful. Please correct the errors below.") %>

    <% using (Html.BeginForm("Edit", "Wiki", FormMethod.Post)) { %>
    <div class="form-body">
      <div class="editor-label">
        <%: Html.LabelFor(m => m.Page)%>
      </div>
      <div class="editor-field">
        <%: Html.TextBoxFor(m => m.Page, Html.AttributesFor(m => m.Page))%>
        <span><%= (Model.Page ?? string.Empty).ToString().Length == 0 ? string.Empty : Html.ActionLink(Model.Page, "Page", new{page = Model.Page}).ToString() %></span>
        <%: Html.ValidationMessageFor(m => m.Page)%>
        <%: Html.HintFor(m => m.Page)%>
      </div>

      <div class="editor-label">
        <%: Html.LabelFor(m => m.Title)%>
      </div>
      <div class="editor-field">
        <%: Html.TextBoxFor(m => m.Title, Html.AttributesFor(m => m.Title))%>
        <%: Html.ValidationMessageFor(m => m.Title)%>
        <%: Html.HintFor(m => m.Title)%>
      </div>

      <div class="editor-label">
        <%: Html.LabelFor(m => m.MetaTitle)%>
      </div>
      <div class="editor-field">
        <%: Html.TextBoxFor(m => m.MetaTitle, Html.AttributesFor(m => m.MetaTitle))%>
        <%: Html.ValidationMessageFor(m => m.MetaTitle)%>
        <%: Html.HintFor(m => m.MetaTitle)%>
      </div>
      
      <div class="editor-label">
        <%: Html.LabelFor(m => m.PublishDate)%>
      </div>
      <div class="editor-field">
        <%: Html.TextBoxFor(m => m.PublishDate, Html.AttributesFor(m => m.PublishDate))%>
        <%: Html.ValidationMessageFor(m => m.PublishDate)%>
        <%: Html.HintFor(m => m.PublishDate)%>
      </div>
      
      <div class="editor-label">
        <%: Html.LabelFor(m => m.MetaDescription)%>
      </div>
      <div class="editor-field">
        <%: Html.TextAreaFor(m => m.MetaDescription, Html.AttributesFor(m => m.MetaDescription))%>
        <%: Html.ValidationMessageFor(m => m.MetaDescription)%>
        <%: Html.HintFor(m => m.MetaDescription)%>
      </div>

      <div class="editor-label">
        <%: Html.LabelFor(m => m.Sidebar)%>
      </div>
      <div class="editor-field">
        <%: Html.TextAreaFor(m => m.Sidebar, Html.AttributesFor(m => m.Sidebar))%>
        <%: Html.ValidationMessageFor(m => m.Sidebar)%>
        <%: Html.HintFor(m => m.Sidebar)%>
      </div>
      
      <div class="editor-label">
        <%: Html.LabelFor(m => m.Keywords)%>
      </div>
      <div class="editor-field">
        <%: Html.TextBoxFor(m => m.Keywords, Html.AttributesFor(m => m.Keywords))%>
        <%: Html.ValidationMessageFor(m => m.Keywords)%>
        <%: Html.HintFor(m => m.Keywords)%>
      </div>
      
      <div class="editor-label">
        <%: Html.Label("Upload")%>
      </div>
      <div class="editor-field">
        <span><%=Html.ActionLink("Click here to upload files...", "Index", "Upload", null, new{target = "_blank"}) %></span>
      </div>

      <div class="editor-label">
        <%: Html.LabelFor(m => m.Content)%>
      </div>
      <div class="editor-field">
        <%: Html.EditorFor(m => m.Content, Html.AttributesFor(m => m.Content))%>
        <%: Html.ValidationMessageFor(m => m.Content)%>
        <%: Html.HintFor(m => m.Content)%>
      </div>
    
      <div class="editor-label">
        <%: Html.Label("Feeds") %>        
      </div>
      <div class="editor-field">
          <% foreach (var feed in Model.Feeds) { %>
          <input type="checkbox" name="FeedIds" id="feedIds" value="<%: feed.Id %>" <%= Model.FeedIds != null && Model.FeedIds.Contains(feed.Id) ? "checked='checked'" : "" %> /> <%= feed.Title %>
          <% } %>
          <span class="hint">When you save this entry, it will appear in the feeds above.</span>
      </div>
        
      <div class="editor-label">
        <%: Html.LabelFor(m => m.ChangeSummary)%>
      </div>
      <div class="editor-field">
        <%: Html.TextAreaFor(m => m.ChangeSummary, Html.AttributesFor(m => m.ChangeSummary))%>
        <%: Html.ValidationMessageFor(m => m.ChangeSummary)%>
        <%: Html.HintFor(m => m.ChangeSummary)%>
      </div>

      <div class="editor-label">
        <%: Html.LabelFor(m => m.AllowComments)%>
      </div>
      <div class="editor-field">
        <%: Html.CheckBoxFor(m => m.AllowComments, Html.AttributesFor(m => m.AllowComments))%>
        <%: Html.ValidationMessageFor(m => m.AllowComments)%>
        <%: Html.HintFor(m => m.AllowComments)%>
      </div>

      <div class="editor-label">
      </div>
      <div class="editor-field">
        <input type="submit" id="submit" class="submit" value="Save" />
      </div>
    </div>
    
    <% } %>

    <h3>Preview</h3>
    <div id="wmd-preview" class="wmd-panel"></div>

</asp:Content>

