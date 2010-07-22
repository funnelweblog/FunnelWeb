<%@ Page Language="C#" ContentType="application/rss+xml" Inherits="FunnelWeb.Web.Application.Views.ApplicationView<FunnelWeb.Web.Controllers.FeedController.FeedModel>" 
%><?xml version="1.0" encoding="utf-8"?>
<feed xmlns="http://www.w3.org/2005/Atom" xmlns:creativeCommons="http://backend.userland.com/creativeCommonsRssModule" xmlns:thr="http://purl.org/syndication/thread/1.0">
    <title type="text"><%= Settings.SiteTitle %></title>
    <link rel="self" href="<%= Html.Qualify(Html.ActionUrl("Feed", "Feed", new {feedName = Model.FeedName })) %>" type="application/atom+xml" />
    <link rel="alternate" href="<%= Html.Qualify(Html.ActionUrl("Recent", "Wiki", null)) %>" type="text/html" />
    <subtitle><%= Settings.SearchDescription %></subtitle>
    <updated><%= Html.DateRssFormat(DateTime.UtcNow) %></updated>
    <id><%= Html.Qualify(Html.ActionUrl("Feed", "Feed", new {feedName = Model.FeedName })) %></id>
    <creativeCommons:license>http://www.creativecommons.org/licenses/by-nc/2.5/rdf</creativeCommons:license>
    
<% foreach (var item in Model.Items) { %>
    <entry>
        <id><%= Html.Qualify(Html.ActionUrl("Page", "Wiki", new {page = item.Name })) %></id>
        <title type="text"><%= item.Title %></title>
        <author><name><%= Settings.Author %></name></author>
        <link rel="alternate" href="<%= Html.Qualify(Html.ActionUrl("Page", "Wiki", new {page = item.Name })) %>" />
        <published><%= Html.DateRssFormat(item.FeedDate)%></published>
        <updated><%= Html.DateRssFormat(item.FeedDate)%></updated>
        <summary type="html">
        <![CDATA[
        <%= Html.Markdown(item.LatestRevision.Body, false) %>
        <img src="<%= Html.Qualify(item.Name + "/via-feed") %>" />
        ]]>
        </summary>
    </entry>
<% } %>
</feed>