<%@ Page Title="" Language="C#" MasterPageFile="~/Content/Site.Master" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Features.Wikis.Views.EditModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server"><%= Settings.SiteTitle %> - Edit - <%=Model.Entry.Title%></asp:Content>
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
    <h1>Create: <%=Model.Entry.Title%></h1>
    <% } %>
    <% else { %>     
    <h1>Edit: <%=Model.Entry.Title%></h1>
    <% } %>
    
    <% using (Html.BeginForm("Save", "Wiki", FormMethod.Post, new { @class = "promptBeforeUnload" })) { %>
    
    <div class="form-body">
        <p>
            <%= Html.Label("Name", "page")%>
            <%= Html.InputTextBox("page").Default(Model.Page).Large().Max(50).IsRequired()%>
            <span><%= (Model.Page ?? string.Empty).ToString().Length == 0 ? string.Empty : Html.ActionLink(Model.Page, "Page", new{page = Model.Page}).ToString() %></span>
            <span class="hint">This will appear in the URL to the page.</span>
        </p>
        
        <p>
            <%= Html.Label("Title", "title") %>
            <%= Html.InputTextBox("title").Default(Model.Entry.Title).Large().Max(200).IsRequired()%>
            <span class="hint">This appears at the top of this page and on the home page.</span>
        </p>
        
        <p>
            <%= Html.Label("Meta Title", "metaTitle") %>
            <%= Html.InputTextBox("metaTitle").Default(Model.Entry.MetaTitle).Large().Max(65).IsRequired()%>
            <span class="hint">This appears at the top of the browser and is used by search engines.</span>
        </p>
        
        <p>
            <%= Html.Label("Publish", "published") %>
            <%= Html.InputDatePicker("published").Default(Model.Entry.Published).IsRequired()%>
            <span class="hint">This page will not be published until after the date above.</span>
        </p>
        
        <p>
            <%= Html.Label("Brief", "metaDescription") %>
            <%= Html.InputTextBox("metaDescription").Large().Max(150).Default(Model.Entry.MetaDescription).IsRequired()%>
            <span class="hint">A short description that will appear in the &lt;meta&gt; tags of the page.</span>
        </p>
        
        <p>
            <%= Html.Label("Sidebar", "summary") %>
            <%= Html.InputTextArea("summary").Default(Model.Entry.Summary)%>
            <span class="hint">This will appear at the right of the page. Use it to provide a quick description of the page to users.</span>
        </p>
        
        <p>
            <%= Html.Label("Keywords", "metaKeywords") %>
            <%= Html.InputTextBox("metaKeywords").Max(100).Large().Default(Model.Entry.MetaKeywords).IsRequired()%>
            <span class="hint">Comma-seperated keywords that will appear in the &lt;meta&gt; tags of the page.</span>
        </p>
        
        <p>
            <%= Html.Label("Upload", "upload") %>
            <span><%=Html.ActionLink("Click here to upload files...", "Index", "Upload", null, new{target = "_blank"}) %></span>
        </p>
        
        <p>
            <%= Html.Label("Content", "wmd-input") %>
            <%= Html.InputTextEditor("body").Default(ViewData.Eval("Entry.LatestRevision.Body")).IsRequired()%>
        </p>
        
        <p>
            <%= Html.Label("Feeds", "") %>
            <% foreach (var feed in Model.Feeds) { %>
            <%= Html.InputCheckBox("feeds", feed.Id) %> <%= feed.Title %>
            <% } %>
            <span class="hint">When you save this entry, it will appear in the feeds above.</span>
        </p>
        
        <p>
            <%= Html.Label("Comments", "comment") %>
            <%= Html.InputTextArea("comment").Default(Model.IsNew ? "Initial creation." : "").IsRequired() %>
            <span class="hint">A breif overview of what was changed and why. This will appear on the page history.</span>
        </p>
        
        <p>
            <%= Html.Label("Comments", "enableDiscussion")%>
            <%= Html.CheckBox("enableDiscussion", Model.Entry.IsDiscussionEnabled) %> Allowed
            <span class="hint">If checked, allows users to post comments on this page.</span>
        </p>
        
        <p>
            <input type="submit" id="submit" class="submit" value="Submit" />
        </p>
        <p>
            <span class="notification-wait" id="submitnotification"></span>
        </p>
    </div>
    
    <% } %>

    <h3>Preview</h3>
    <div id="wmd-preview" class="wmd-panel"></div>

</asp:Content>

