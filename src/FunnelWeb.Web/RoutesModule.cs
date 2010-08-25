using System;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace FunnelWeb.Web.Application.Routes
{
    public static class RouteExtensions
    {
        public static Route MapHyphenatedRoute(this RouteCollection routes, string url, object defaults)
        {
            Route route = new Route(
                                url,
                                new RouteValueDictionary(defaults),
                                new HyphenatedRouteHandler()
                                );
            routes.Add(route);

            return route;
        }
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
            routes.MapRoute(R(), "install/{action}", new { controller = "Install", action = "Index" });

            // Feeds
            routes.MapRoute(R(), "feed", new { controller = "Feed", action = "Feed", feedName = (string)null });
            routes.MapRoute(R(), "feeds/{*feedName}", new { controller = "Feed", action = "Feed" });
            routes.MapRoute(R(), "commentfeed", new { controller = "Feed", action = "CommentFeed" });

            // Login
            routes.MapRoute(R(), "login/{action}", new { controller = "Login", action = "Index", databaseIssue = UrlParameter.Optional, ReturnUrl = UrlParameter.Optional });

            // Upload
            routes.MapRoute(R(), "get/{*path}", new { controller = "Upload", action = "Render" });
            routes.MapHyphenatedRoute("upload/{action}/{*path}", new { controller = "Upload", action = "Index", path = "/" });
            //routes.MapRoute(R(), "upload/create-directory", new { controller = "Upload", action = "CreateDirectory" });
            //routes.MapRoute(R(), "upload/upload", new { controller = "Upload", action = "Upload" });
            //routes.MapRoute(R(), "upload/move", new { controller = "Upload", action = "Move" });
            //routes.MapRoute(R(), "upload/delete", new { controller = "Upload", action = "Delete" });
            //routes.MapRoute(R(), "upload/{*path}", new { controller = "Upload", action = "Index", path = "/" });
            
            // Resources
            routes.MapRoute(R(), "robots", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapRoute(R(), "robots.txt", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapRoute(R(), "favicon", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapRoute(R(), "favicon.ico", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapRoute(R(), "favicon.png", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.png", contentType = "image/png" });
            routes.MapRoute(R(), "status", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Status.html", contentType = "text/html" });

            // Site Map
            routes.MapRoute(R(), "sitemap", new { controller = "Wiki", action = "SiteMap" });
            routes.MapRoute(R(), "sitemap.xml", new { controller = "Wiki", action = "SiteMap" });

            // Wiki
            routes.MapRoute(R(), "", new { controller = "Wiki", action = "Recent", pageNumber = "0" });
            routes.MapRoute(R(), "unpublished", new { controller = "Wiki", action = "Unpublished" });
            routes.MapRoute(R(), "search", new { controller = "Wiki", action = "Search" });
            routes.MapRoute(R(), "new", new { controller = "Wiki", action = "New" });
            routes.MapRoute(R(), "save", new { controller = "Wiki", action = "Save" });
            routes.MapRoute(R(), "{pageNumber}", new { controller = "Wiki", action = "Recent" }, new { pageNumber = "\\d+" });
            routes.MapRoute(R(), "{page}/via-feed", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Images/Transparent.gif", contentType = "image/gif" });
            routes.MapRoute(R(), "{page}/revisions", new { controller = "Wiki", action = "Revisions" });
            routes.MapRoute(R(), "{page}/edit", new { controller = "Wiki", action = "Edit" });
            routes.MapRoute(R(), "{page}/comment", new { controller = "Wiki", action = "Comment" });
            routes.MapRoute(R(), "{page}/{revision}", new { controller = "Wiki", action = "Page", revision = "0" }, new { revision = "\\d+" });
            routes.MapRoute(R(), "{page}/{*xyz}", new { controller = "Wiki", action = "NotFound" });
        }
    }
}