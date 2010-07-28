using System;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace FunnelWeb.Web.Application.Routes
{
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
            routes.IgnoreRoute("pingback");

            // Administration
            routes.MapRoute(R(), "admin", new { controller = "Admin", action = "Index" });
            routes.MapRoute(R(), "admin/delete-comment", new { controller = "Admin", action = "DeleteComment" });
            routes.MapRoute(R(), "admin/update-settings", new { controller = "Admin", action = "UpdateSettings" });
            routes.MapRoute(R(), "admin/add-redirect", new { controller = "Admin", action = "CreateRedirect" });
            routes.MapRoute(R(), "admin/delete-redirect", new { controller = "Admin", action = "DeleteRedirect" });
            routes.MapRoute(R(), "admin/delete-all-spam", new { controller = "Admin", action = "DeleteAllSpam" });
            routes.MapRoute(R(), "admin/toggle-spam", new { controller = "Admin", action = "ToggleSpam" });
            routes.MapRoute(R(), "admin/delete-pingback", new { controller = "Admin", action = "DeletePingback" });
            routes.MapRoute(R(), "admin/toggle-pingback-spam", new { controller = "Admin", action = "TogglePingbackSpam" });
            routes.MapRoute(R(), "admin/add-feed", new { controller = "Admin", action = "CreateFeed" });
            routes.MapRoute(R(), "admin/delete-feed", new { controller = "Admin", action = "DeleteFeed" });
            
            // Installation
            routes.MapRoute(R(), "install", new { controller = "Install", action = "Index" });
            routes.MapRoute(R(), "install/test", new { controller = "Install", action = "Test" });
            routes.MapRoute(R(), "install/upgrade", new { controller = "Install", action = "Upgrade" });
            
            // Feeds
            routes.MapRoute(R(), "feeds/{feedName}", new { controller = "Feed", action = "Feed", feedName = "recent" });
            routes.MapRoute(R(), "commentfeed", new { controller = "Feed", action = "CommentFeed" });

            // Login
            routes.MapRoute(R(), "login", new { controller = "Login", action = "Index", databaseIssue = UrlParameter.Optional, ReturnUrl = UrlParameter.Optional });
            routes.MapRoute(R(), "login/login", new { controller = "Login", action = "Login" });
            routes.MapRoute(R(), "login/logout", new { controller = "Login", action = "Logout" });

            // Upload
            routes.MapRoute(R(), "upload/create-directory", new { controller = "Upload", action = "CreateDirectory" });
            routes.MapRoute(R(), "upload/upload", new { controller = "Upload", action = "Upload" });
            routes.MapRoute(R(), "upload/move", new { controller = "Upload", action = "Move" });
            routes.MapRoute(R(), "upload/delete", new { controller = "Upload", action = "Delete" });
            routes.MapRoute(R(), "upload/{*path}", new { controller = "Upload", action = "Index", path = "/" });
            routes.MapRoute(R(), "get/{*path}", new { controller = "Upload", action = "Render" });
            
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