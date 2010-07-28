<%@ Page Language="C#" ContentType="application/rss+xml" Inherits="FunnelWeb.Web.Application.Views.ApplicationView<FunnelWeb.Web.Controllers.FeedController+CommentFeedModel>" 
%><?xml version="1.0" encoding="utf-8"?>
<feed xmlns="http://www.w3.org/2005/Atom" xmlns:creativeCommons="http://backend.userland.com/creativeCommonsRssModule" xmlns:thr="http://purl.org/syndication/thread/1.0">
    <title type="text"><%= Settings.SiteTitle %> Comments</title>
    <link rel="self" href="<%= Html.Qualify(Html.ActionUrl("CommentFeed", "Feed", null)) %>" type="application/atom+xml" />
    <link rel="alternate" href="<%= Html.Qualify(Html.ActionUrl("Recent", "Wiki", null)) %>" type="text/html" />
    <subtitle><%= Settings.SearchDescription %></subtitle>
    <updated><%= Html.DateRssFormat(DateTime.UtcNow) %></updated>
    <id><%= Html.Qualify(Html.ActionUrl("CommentFeed", "Feed")) %></id>
    <creativeCommons:license>http://www.creativecommons.org/licenses/by-nc/2.5/rdf</creativeCommons:license>
    
<% foreach (var item in Model.Comments) { %>
    <entry>
        <id><%= Html.Qualify(Html.ActionUrl("Page", "Wiki", new {page = item.Entry.Name, comment = item.Id })) %></id>
        <title type="text"><%= item.AuthorName %> on <%= item.Entry.Title %></title>
        <author><name><%= item.AuthorName %></name></author>
        <link rel="alternate" href="<%= Html.Qualify(Html.ActionUrl("Page", "Wiki", new {page = item.Entry.Name })) %>" />
        <published><%= Html.DateRssFormat(item.Posted) %></published>
        <updated><%= Html.DateRssFormat(item.Posted) %></updated>
        <summary type="html">
        <![CDATA[
        <%= Html.Markdown(item.Body, false) %>
        ]]>
        </summary>
    </entry>
<% } %>
</feed>