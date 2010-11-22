using System;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using FunnelWeb.Web.Application.Mvc;

namespace FunnelWeb.Web
{
    public class RoutesModule : Module
    {
        private readonly RouteCollection routes;

        public RoutesModule(RouteCollection routes)
        {
            this.routes = routes;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var routeId = 0;
            var r = new Func<string>(() => (routeId++).ToString());

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaxd}", new { allaxd = @".*\.axd(/.*)?" });
            routes.IgnoreRoute("pingback");

            // Administration
            routes.MapHyphenatedRoute("admin/{action}", new { controller = "Admin", action = "Index" });

            // Installation
            routes.MapLowerCaseRoute(r(), "install/{action}", new { controller = "Install", action = "Index" });

            // Feeds
            routes.MapLowerCaseRoute(r(), "feed", new { controller = "Feed", action = "Feed", feedName = (string)null });
            routes.MapLowerCaseRoute(r(), "feeds/{*feedName}", new { controller = "Feed", action = "Feed" });
            routes.MapLowerCaseRoute(r(), "commentfeed", new { controller = "Feed", action = "CommentFeed" });

            // Login
            routes.MapLowerCaseRoute(r(), "login/{action}", new { controller = "Login", action = "Login", databaseIssue = UrlParameter.Optional, ReturnUrl = UrlParameter.Optional });

            // Upload
            routes.MapLowerCaseRoute(r(), "get/{*path}", new { controller = "Upload", action = "Render" });
            routes.MapHyphenatedRoute("upload/{action}/{*path}", new { controller = "Upload", action = "Index", path = "/" });
            
            // Resources
            routes.MapLowerCaseRoute(r(), "robots", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapLowerCaseRoute(r(), "robots.txt", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapLowerCaseRoute(r(), "favicon", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapLowerCaseRoute(r(), "favicon.ico", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapLowerCaseRoute(r(), "favicon.png", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.png", contentType = "image/png" });
            routes.MapLowerCaseRoute(r(), "status", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Status.html", contentType = "text/html" });

            // Site Map
            routes.MapLowerCaseRoute(r(), "sitemap", new { controller = "Wiki", action = "SiteMap" });
            routes.MapLowerCaseRoute(r(), "sitemap.xml", new { controller = "Wiki", action = "SiteMap" });

            // Wiki
            routes.MapLowerCaseRoute(r(), "", new { controller = "Wiki", action = "Recent", pageNumber = "0" });
            routes.MapLowerCaseRoute(r(), "unpublished", new { controller = "Wiki", action = "Unpublished" });
            routes.MapLowerCaseRoute(r(), "search", new { controller = "Wiki", action = "Search" });
            routes.MapLowerCaseRoute(r(), "new", new { controller = "Wiki", action = "New" });
            routes.MapLowerCaseRoute(r(), "edit/{page}", new { controller = "Wiki", action = "Edit", page = UrlParameter.Optional });
            routes.MapLowerCaseRoute(r(), "revert/{page}/{revision}", new { controller = "Wiki", action = "Revert" }, new { revision = "\\d+" });
            routes.MapLowerCaseRoute(r(), "{pageNumber}", new { controller = "Wiki", action = "Recent" }, new { pageNumber = "\\d+" });
            routes.MapLowerCaseRoute(r(), "{page}/via-feed", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Images/Transparent.gif", contentType = "image/gif" });
            routes.MapLowerCaseRoute(r(), "{page}/revisions", new { controller = "Wiki", action = "Revisions" });
            routes.MapLowerCaseRoute(r(), "{page}/{revision}", new { controller = "Wiki", action = "Page", revision = "0" }, new { revision = "\\d+" });
            routes.MapLowerCaseRoute(r(), "{page}/{*xyz}", new { controller = "Wiki", action = "NotFound" });
        }
    }
}