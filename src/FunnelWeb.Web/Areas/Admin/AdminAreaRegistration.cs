using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace FunnelWeb.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        private readonly Lazy<HttpContextBase> httpContextWrapper;

        public AdminAreaRegistration()
        {
            httpContextWrapper = new Lazy<HttpContextBase>(() => new HttpContextWrapper(HttpContext.Current));
        }

        public AdminAreaRegistration(Lazy<HttpContextBase> httpContextWrapper)
        {
            this.httpContextWrapper = httpContextWrapper;
        }

        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RegisterRoutes(context);
            RegisterBundles(context);
        }

        private void RegisterRoutes(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(context);
        }

        private void RegisterBundles(AreaRegistrationContext context)
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles, context, httpContextWrapper.Value);
        }
    }
}
