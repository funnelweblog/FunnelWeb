using System.Web.Mvc;

namespace FunnelWeb.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_Logout",
                "admin/logout",
                new { controller = "Login", action = "Logout" }
                );

            context.MapRoute(
                "Admin_Login",
                "admin/login/{action}",
                new { controller = "Login", action = "Login" }
                );

            context.MapRoute(
                "Admin_Settings",
                "admin/settings/{action}",
                new { controller = "Settings", action = "Index" }
                );

            context.MapRoute(
                "Admin_comments",
                "admin/comments/{action}",
                new { controller = "Comments", action = "Index" }
                );

            context.MapRoute(
                "Admin_Install",
                "admin/install/{action}",
                new {controller = "Install", action = "Index"}
                );

            context.MapRoute(
                "Admin_Wiki",
                "admin/{controller}/{action}/{*page}",
                new { controller = "WikiAdmin", action = "Edit", page = UrlParameter.Optional }
                );

            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}
