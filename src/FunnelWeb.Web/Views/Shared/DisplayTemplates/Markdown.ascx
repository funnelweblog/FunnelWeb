<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<String>" %>
<%= Html.Markdown(Model, ViewData.ContainsKey("Sanitize") && (bool)ViewData["Sanitize"]) %>