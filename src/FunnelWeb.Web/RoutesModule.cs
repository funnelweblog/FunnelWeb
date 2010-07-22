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
            var routes = _routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("pingback");

            // Administration
            routes.MapRoute("70", "admin", new { controller = "Admin", action = "Index" });
            routes.MapRoute("71", "admin/delete-comment", new { controller = "Admin", action = "DeleteComment" });
            routes.MapRoute("72", "admin/update-settings", new { controller = "Admin", action = "UpdateSettings" });
            routes.MapRoute("73", "admin/add-redirect", new { controller = "Admin", action = "CreateRedirect" });
            routes.MapRoute("74", "admin/delete-redirect", new { controller = "Admin", action = "DeleteRedirect" });
            routes.MapRoute("76", "admin/delete-all-spam", new { controller = "Admin", action = "DeleteAllSpam" });
            routes.MapRoute("75", "admin/toggle-spam", new { controller = "Admin", action = "ToggleSpam" });
            routes.MapRoute("78", "admin/delete-pingback", new { controller = "Admin", action = "DeletePingback" });
            routes.MapRoute("79", "admin/toggle-pingback-spam", new { controller = "Admin", action = "TogglePingbackSpam" });
            
            // Feeds
            routes.MapRoute("60", "feeds/{feedName}", new { controller = "Feed", action = "Feed", feedName = "recent" });
            routes.MapRoute("61", "commentfeed", new { controller = "Feed", action = "CommentFeed" });

            // Login
            routes.MapRoute("90", "login", new { controller = "Login", action = "Index" });
            routes.MapRoute("91", "login/login", new { controller = "Login", action = "Login" });
            routes.MapRoute("92", "login/logout", new { controller = "Login", action = "Logout" });

            // Upload
            routes.MapRoute("141", "upload/create-directory", new { controller = "Upload", action = "CreateDirectory" });
            routes.MapRoute("142", "upload/upload", new { controller = "Upload", action = "Upload" });
            routes.MapRoute("143", "upload/move", new { controller = "Upload", action = "Move" });
            routes.MapRoute("144", "upload/delete", new { controller = "Upload", action = "Delete" });
            routes.MapRoute("145", "upload/{*path}", new { controller = "Upload", action = "Index", path = "/" });
            routes.MapRoute("146", "get/{*path}", new { controller = "Upload", action = "Render" });
            
            // Resources
            routes.MapRoute("100", "robots", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapRoute("101", "robots.txt", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapRoute("102", "favicon", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapRoute("103", "favicon.ico", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapRoute("104", "favicon.png", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/favicon.png", contentType = "image/png" });
            routes.MapRoute("105", "status", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Resources/Status.html", contentType = "text/html" });
            
            // Site Map
            routes.MapRoute("160", "sitemap", new {controller = "Wiki", action = "SiteMap"});
            routes.MapRoute("161", "sitemap.xml", new {controller = "Wiki", action = "SiteMap"});

            // Wiki
            routes.MapRoute("10", "", new { controller = "Wiki", action = "Recent", pageNumber = "0" });
            routes.MapRoute("11", "search", new { controller = "Wiki", action = "Search" });
            routes.MapRoute("12", "{pageNumber}", new { controller = "Wiki", action = "Recent" }, new { pageNumber = "\\d+" });
            routes.MapRoute("17", "{page}/via-feed", new { controller = "Resource", action = "Render", fileName = "/Views/Shared/Images/Transparent.gif", contentType = "image/gif" });
            routes.MapRoute("15", "{page}/revisions", new { controller = "Wiki", action = "Revisions" });
            routes.MapRoute("25", "{page}/edit", new { controller = "Wiki", action = "Edit" });
            routes.MapRoute("30", "{page}/save", new { controller = "Wiki", action = "Save" });
            routes.MapRoute("35", "{page}/comment", new { controller = "Wiki", action = "Comment" });
            routes.MapRoute("40", "{page}/{revision}", new { controller = "Wiki", action = "Page", revision = "0" }, new { revision = "\\d+" });
            routes.MapRoute("41", "{page}/{*xyz}", new { controller = "Wiki", action = "NotFound" });
        }
    }
}