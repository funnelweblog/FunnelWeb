using System;
using System.Web.Routing;
using Autofac;
using FunnelWeb.Web.Application.Mvc;

namespace FunnelWeb.Web
{
    public static class RouteExtensions
    {
    }

    public class RoutesModule : Module
    {
        private readonly RouteCollection _routes;

        public RoutesModule(RouteCollection routes)
        {
            _routes = routes;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var routeId = 0;
            var R = new Func<string>(() => (routeId++).ToString());

            var routes = _routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaxd}", new { allaxd = @".*\.axd(/.*)?" });
            routes.IgnoreRoute("pingback");

            // Administration
            routes.MapHyphenatedRoute("admin/{action}", new { controller = "Admin", action = "Index" });

            // Installation
            routes.MapLowerCaseRoute(R(), "install/{action}", new { controller = "Install", action = "Index" });

            // Feeds
            routes.MapLowerCaseRoute(R(), "feed", new { controller = "Feed", action = "Feed", feedName = (string)null });
            routes.MapLowerCaseRoute(R(), "feeds/{*feedName}", new { controller = "Feed", action = "Feed" });
            routes.MapLowerCaseRoute(R(), "commentfeed", new { controller = "Feed", action = "CommentFeed" });

            // Login
            routes.MapLowerCaseRoute(R(), "login/{action}", new { controller = "Login", action = "Index", databaseIssue = UrlParameter.Optional, ReturnUrl = UrlParameter.Optional });

            // Upload
            routes.MapLowerCaseRoute(R(), "get/{*path}", new { controller = "Upload", action = "Render" });
            routes.MapHyphenatedRoute("upload/{action}/{*path}", new { controller = "Upload", action = "Index", path = "/" });
            
            // Resources
            routes.MapLowerCaseRoute(R(), "robots", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapLowerCaseRoute(R(), "robots.txt", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapLowerCaseRoute(R(), "favicon", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapLowerCaseRoute(R(), "favicon.ico", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapLowerCaseRoute(R(), "favicon.png", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.png", contentType = "image/png" });
            routes.MapLowerCaseRoute(R(), "status", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Status.html", contentType = "text/html" });

            // Site Map
            routes.MapLowerCaseRoute(R(), "sitemap", new { controller = "Wiki", action = "SiteMap" });
            routes.MapLowerCaseRoute(R(), "sitemap.xml", new { controller = "Wiki", action = "SiteMap" });

            // Wiki
            routes.MapLowerCaseRoute(R(), "", new { controller = "Wiki", action = "Recent", pageNumber = "0" });
            routes.MapLowerCaseRoute(R(), "unpublished", new { controller = "Wiki", action = "Unpublished" });
            routes.MapLowerCaseRoute(R(), "search", new { controller = "Wiki", action = "Search" });
            routes.MapLowerCaseRoute(R(), "new", new { controller = "Wiki", action = "New" });
            routes.MapLowerCaseRoute(R(), "save", new { controller = "Wiki", action = "Save" });
            routes.MapLowerCaseRoute(R(), "{pageNumber}", new { controller = "Wiki", action = "Recent" }, new { pageNumber = "\\d+" });
            routes.MapLowerCaseRoute(R(), "{page}/via-feed", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Images/Transparent.gif", contentType = "image/gif" });
            routes.MapLowerCaseRoute(R(), "{page}/revisions", new { controller = "Wiki", action = "Revisions" });
            routes.MapLowerCaseRoute(R(), "{page}/edit", new { controller = "Wiki", action = "Edit" });
            routes.MapLowerCaseRoute(R(), "{page}/comment", new { controller = "Wiki", action = "Comment" });
            routes.MapLowerCaseRoute(R(), "{page}/{revision}", new { controller = "Wiki", action = "Page", revision = "0" }, new { revision = "\\d+" });
            routes.MapLowerCaseRoute(R(), "{page}/{*xyz}", new { controller = "Wiki", action = "NotFound" });
        }
    }
}