<%@ Page Language="C#" ContentType="text/xml" Inherits="System.Web.Mvc.ViewPage<FunnelWeb.Web.Features.Wikis.Views.SiteMapModel>" %><?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
   <url>
      <loc><%= Html.Qualify("/") %></loc>
      <lastmod><%= DateTime.Now.ToString("yyyy-MM-dd") %></lastmod>
      <changefreq>daily</changefreq>
      <priority>1.0</priority>
   </url>
<% foreach (var item in Model.Entries) { %>   <url>
      <loc><%= Html.Qualify(Html.ActionUrl("Page", "Wiki", new { page = item.Name})) %></loc>
      <lastmod><%= item.LatestRevision.Revised.ToString("yyyy-MM-dd") %></lastmod>
      <changefreq><%= Model.GetChangeFrequency(item) %></changefreq>
      <priority><%= Model.Prioritize(item).ToString("n3")%></priority>
   </url>
<% } %></urlset>
