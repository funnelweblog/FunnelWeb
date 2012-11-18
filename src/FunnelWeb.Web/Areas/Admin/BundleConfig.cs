using System.Web.Mvc;
using System.Web.Optimization;
using FunnelWeb.Web.Application.Mvc;

namespace FunnelWeb.Web.Areas.Admin
{
    internal static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        internal static void RegisterBundles(BundleCollection bundles, AreaRegistrationContext context)
        {
            ViewBundleRegistrar.RegisterViewBundlesForArea(bundles, context);
        }
    }
}