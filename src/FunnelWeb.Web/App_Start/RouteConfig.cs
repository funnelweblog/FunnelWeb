using System.Web.Mvc;
using System.Web.Routing;
using FunnelWeb.Web.Application.MetaWeblog;
using FunnelWeb.Web.Application.Mvc;

namespace FunnelWeb.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaxd}", new { allaxd = @".*\.axd(/.*)?" });
            routes.IgnoreRoute("pingback");

            // Feeds
            routes.MapLowerCaseRoute("feed", new { controller = "Feed", action = "Feed", feedName = (string)null });
            routes.MapLowerCaseRoute("feeds/{*feedName}", new { controller = "Feed", action = "Feed" });
            routes.MapLowerCaseRoute("commentfeed", new { controller = "Feed", action = "CommentFeed" });

            // Upload
            routes.MapLowerCaseRoute("get/{*path}", new { controller = "Upload", action = "Render", area = "Admin" });

            // Resources
            routes.MapLowerCaseRoute("content/theme.css", new { controller = "Resource", action = "RenderThemedFileIfExists", fileToRender = "{Theme}/Content/Styles/Theme.css", contentType = "text/css" });
            routes.MapLowerCaseRoute("robots", new { controller = "Resource", action = "Render", fileName = "Content/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapLowerCaseRoute("robots.txt", new { controller = "Resource", action = "Render", fileName = "Content/Resources/Robots.txt", contentType = "text/plain" });
            routes.MapLowerCaseRoute("humans.txt", new { controller = "Resource", action = "Render", fileName = "Content/Resources/Humans.txt", contentType = "text/plain" });
            routes.MapLowerCaseRoute("humans", new { controller = "Resource", action = "Render", fileName = "Content/Resources/Humans.txt", contentType = "text/plain" });
            routes.MapLowerCaseRoute("favicon", new { controller = "Resource", action = "Render", fileName = "{Theme}/Content/Images/favicon.ico", fileName2 = "Content/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapLowerCaseRoute("favicon.ico", new { controller = "Resource", action = "Render", fileName = "{Theme}/Content/Images/favicon.ico", fileName2 = "Content/Resources/favicon.ico", contentType = "image/vnd.microsoft.icon" });
            routes.MapLowerCaseRoute("favicon.png", new { controller = "Resource", action = "Render", fileName = "{Theme}/Content/Images/favicon.png", fileName2 = "Content/Resources/favicon.png", contentType = "image/png" });
            routes.MapLowerCaseRoute("status", new { controller = "Resource", action = "Render", fileName = "Content/Resources/Status.html", contentType = "text/html" });

            // Site Map
            routes.MapLowerCaseRoute("sitemap", new { controller = "Wiki", action = "SiteMap" });
            routes.MapLowerCaseRoute("sitemap.xml", new { controller = "Wiki", action = "SiteMap" });

            // Tags
            routes.MapLowerCaseRoute("tag/{*tagName}", new { controller = "Tag", action = "Index" });

            // Tagged Pages
            routes.MapLowerCaseRoute("tagged/{*tag}", new { controller = "Tagged", action = "Index" });

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

            routes.MapLowerCaseRoute("via-feed/{*page}", new { controller = "Resource", action = "Render", fileName = "Content/Images/transparent.gif", contentType = "image/gif" });
            routes.MapLowerCaseRoute("history-of/{*page}", new { controller = "Wiki", action = "Revisions" });
            routes.MapLowerCaseRoute("trackbacks-for/{*page}", new { controller = "Wiki", action = "Pingbacks" });

            // Remove .aspx
            routes.Add(new RedirectRoute("(?<page>[a-zA-Z0-9/\\-\\._\\+ ]+)\\.aspx", new MvcRouteHandler()) { ReplacePattern = "/$1" });
            routes.Add(new RedirectRoute("(?<page>rss)$", new MvcRouteHandler()) { ReplacePattern = "feed", ResponseCode = 302 });

            // http://www.cookcomputing.com/blog/archives/xml-rpc-and-asp-net-mvc
            routes.MapLowerCaseRoute("wlwmanifest.xml", new { controller = "MetaWeblog", action = "WlwManifest" });
            routes.MapLowerCaseRoute("rsd.xml", new { controller = "MetaWeblog", action = "Rsd" });
            routes.Add(new Route("{weblog}", null, new RouteValueDictionary(new { weblog = "blogapi" }), new MetaWeblogRouteHandler()));

            routes.MapLowerCaseRoute("{*page}", new { controller = "Wiki", action = "Page" });
        }
    }
}