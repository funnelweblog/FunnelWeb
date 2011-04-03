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
                new {controller = "Login", action = "Login"}
                );

            context.MapRoute(
                "Admin_Install",
                "admin/install/{action}",
                new {controller = "Install", action = "Index"}
                );

            context.MapRoute("Admin_Get", "get/{*path}", new { controller = "Upload", action = "Render" });

            //need to do this route explicitly otherwise we can't handle nice URLs when directory browsing
            context.MapRoute(
                "Admin_Upload",
                "upload/{action}/{*path}",
                new {controller = "Upload", action = "Index", path = "/"}
                );

            //an explicit route to the edit page so that it can be done well
            context.MapRoute(
                "Admin_Wiki_Edit",
                "admin/{controller}/edit/{*page}",
                new { controller = "WikiAdmin", action = "Edit", page = UrlParameter.Optional }
                );

            //anything else we expect is on the admin controller, so just route to it
            context.MapRoute(
                "Admin_default",
                "admin/{action}/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}
