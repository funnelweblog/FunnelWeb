using System.Web.Mvc;
using FunnelWeb.Web.Application.Mvc;

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

            //context.MapRoute(
            //    "Admin_Settings",
            //    "admin/settings",
            //    new { controller = "Admin", action = "Settings" }
            //    );

            //context.MapRoute(
            //    "Admin_comments",
            //    "admin/comments",
            //    new { controller = "Admin", action = "Comments" }
            //    );

            //context.MapRoute(
            //    "Admin_Install",
            //    "admin/install/{action}",
            //    new { controller = "Install", action = "Index" }
            //    );

            context.MapRoute(
                "Admin_Upload",
                "upload/{action}/{*path}",
                new {controller = "Upload", action = "Index", path = "/"}
                );

            context.MapRoute(
                "Admin_Wiki_Edit",
                "admin/{controller}/edit/{*page}",
                new { controller = "WikiAdmin", action = "Edit", page = UrlParameter.Optional }
                );

            //context.MapRoute(
            //    "Admin_Wiki_Page",
            //    "admin/{controller}/page/{*page}",
            //    new { controller = "WikiAdmin", action = "Edit", page = UrlParameter.Optional }
            //    );

            context.MapRoute(
                "Admin_default",
                "admin/{action}/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}
