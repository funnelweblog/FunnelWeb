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
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaxd}", new { allaxd = @".*\.axd(/.*)?" });
            routes.IgnoreRoute("pingback");

            // Administration
            //routes.MapHyphenatedRoute("admin/{action}", new { controller = "Admin", action = "Index" });

            // Installation
            //routes.MapLowerCaseRoute("install/{action}", new { controller = "Install", action = "Index" });

            // Feeds
            routes.MapLowerCaseRoute("feed", new { controller = "Feed", action = "Feed", feedName = (string)null });
            routes.MapLowerCaseRoute("feeds/{*feedName}", new { controller = "Feed", action = "Feed" });
            routes.MapLowerCaseRoute("commentfeed", new { controller = "Feed", action = "CommentFeed" });

            // Login - does not work with Areas (not sure why)
            //routes.MapLowerCaseRoute("login/{action}", new { Area = "Admin", controller = "Login", action = "Login", databaseIssue = UrlParameter.Optional, ReturnUrl = UrlParameter.Optional });

            // Upload
            routes.MapLowerCaseRoute("get/{*path}", new { controller = "Upload", action = "Render" });
            //routes.MapHyphenatedRoute("upload/{action}/{*path}", new { controller = "Upload", action = "Index", path = "/" });

            // Resources
            routes.MapLowerCaseRoute("robots", new { controller = "Resource", action = "Render", fileName = "/Content/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapLowerCaseRoute("robots.txt", new { controller = "Resource", action = "Render", fileName = "Content/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapLowerCaseRoute("favicon", new { controller = "Resource", action = "Render", fileName = "/Content/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapLowerCaseRoute("favicon.ico", new { controller = "Resource", action = "Render", fileName = "/Content/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapLowerCaseRoute("favicon.png", new { controller = "Resource", action = "Render", fileName = "/Content/Resources/favicon.png", contentType = "image/png" });
            routes.MapLowerCaseRoute("status", new { controller = "Resource", action = "Render", fileName = "/Content/Resources/Status.html", contentType = "text/html" });

            // Site Map
            routes.MapLowerCaseRoute("sitemap", new { controller = "Wiki", action = "SiteMap" });
            routes.MapLowerCaseRoute("sitemap.xml", new { controller = "Wiki", action = "SiteMap" });

            // Tags
            routes.MapLowerCaseRoute("tag/{*tagName}", new { controller = "Tag", action = "Index" });

            // Wiki
            routes.MapLowerCaseRoute("blog", new { controller = "Wiki", action = "Recent", pageNumber = "0" });
            routes.MapLowerCaseRoute("blog/{pageNumber}", new { controller = "Wiki", action = "Recent" }, new { pageNumber = "\\d+" });
            routes.MapLowerCaseRoute("", new { controller = "Wiki", action = "Home", pageNumber = "0" });
            routes.MapLowerCaseRoute("{pageNumber}", new { controller = "Wiki", action = "Home" }, new { pageNumber = "\\d+" });
            routes.MapLowerCaseRoute("search", new { controller = "Wiki", action = "Search" });

            routes.MapLowerCaseRoute("unpublished", new { controller = "WikiAdmin", Area = "Admin", action = "Unpublished" });
            routes.MapLowerCaseRoute("admin/new", new { controller = "WikiAdmin", Area = "Admin", action = "Edit", page = "" });
            routes.MapLowerCaseRoute("edit/{*page}", new { controller = "WikiAdmin", Area = "Admin", action = "Edit", page = UrlParameter.Optional });
            routes.MapLowerCaseRoute("revert/{*page}", new { controller = "WikiAdmin", Area = "Admin", action = "Revert" });

            routes.MapLowerCaseRoute("via-feed/{*page}", new { controller = "Resource", action = "Render", fileName = "/Content/Images/Transparent.gif", contentType = "image/gif" });
            routes.MapLowerCaseRoute("history-of/{*page}", new { controller = "Wiki", action = "Revisions" });
            routes.MapLowerCaseRoute("{*page}", new { controller = "Wiki", action = "Page" });
        }
    }
}