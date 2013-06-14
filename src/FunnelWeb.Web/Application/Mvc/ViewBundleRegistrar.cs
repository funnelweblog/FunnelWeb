using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace FunnelWeb.Web.Application.Mvc
{
    public static class ViewBundleRegistrar
    {
        public const string BUNDLE_PATH_START = "~/bundles/";
        public const string BUNDLE_NAME_AREA_VIEW_PREFIX = BUNDLE_PATH_START + "area/";
        public const string BUNDLE_NAME_VIEW_PREFIX = BUNDLE_PATH_START + "views";

        /// <summary>
        /// Registers all the area's views specific bundles. The registrations are based on the Scripts/View folder inside the area.
        /// </summary>
        /// <param name="bundles">The collection to add the bundles to</param>
        /// <param name="context">The context of the area registration</param>
        /// <param name="httpContext"></param>
        public static void RegisterViewBundlesForArea(BundleCollection bundles, AreaRegistrationContext context, HttpContextBase httpContext)
        {
            string path = httpContext.Server.MapPath("Areas/" + context.AreaName + "/Scripts/Views");

            if (Directory.Exists(path))
            {
                foreach (string jsFile in Directory.EnumerateFiles(path, "*.js", SearchOption.AllDirectories))
                {
                    bundles.Add(CreateScriptBundle(context.AreaName, jsFile, path));
                }
            }
        }
        
        private static Bundle CreateScriptBundle(string areaName, string jsFile, string path)
        {
            var endOfPath = jsFile.Substring(path.Length, jsFile.Length - path.Length).Replace(@"\", "/");
            var bundleName = endOfPath.Replace(Path.GetFileName(jsFile), Path.GetFileNameWithoutExtension(endOfPath));

            var bundlePath = BUNDLE_NAME_AREA_VIEW_PREFIX + areaName + "/views" + bundleName;
            var jsResource = "~/Areas/" + areaName + "/Scripts/Views" + endOfPath;

            return new ScriptBundle(bundlePath.ToLowerInvariant()).Include(jsResource.ToLowerInvariant());
        }

        /// <summary>
        /// Registers all the views specific bundles. The registrations are based on the ~/Scripts/View folder.
        /// </summary>
        /// <param name="bundles">The collection to add the bundles to</param>
        public static void RegisterViewBundles(BundleCollection bundles)
        {
            string path = HttpContext.Current.Server.MapPath("Scripts/Views");

            if ( Directory.Exists( path ) ) {
              foreach ( string jsFile in Directory.EnumerateFiles( path, "*.js", SearchOption.AllDirectories ) ) {
                bundles.Add( CreateScriptBundle( jsFile, path ) );
              }
            }
        }

        private static Bundle CreateScriptBundle(string jsFile, string path)
        {
            var endOfPath = jsFile.Substring(path.Length, jsFile.Length - path.Length).Replace(@"\", "/");

            var fileName = Path.GetFileName(jsFile);

            var bundleName = endOfPath.Replace(fileName, Path.GetFileNameWithoutExtension(endOfPath));

            return new ScriptBundle(BUNDLE_NAME_VIEW_PREFIX + bundleName.ToLowerInvariant()).Include("~/Scripts/Views" + endOfPath);
        }
    }
}