using System.Web.Mvc;
using System.Web.Optimization;

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
            RegisterRoutes(context);
            RegisterBundles(context);
        }

        private void RegisterRoutes(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(context);
        }

        private void RegisterBundles(AreaRegistrationContext context)
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles , context);            
        }       
    }
}
