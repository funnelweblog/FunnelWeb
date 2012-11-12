using System;
using System.IO;
using System.Web.Mvc;
using System.Web.WebPages;
using FunnelWeb.Web.Application.Mvc;

namespace FunnelWeb.Web.Application.Extensions
{
    public static class WebViewPageExtensions
    {
        /// <summary>
        /// Gets the bundle path for the specific view. Is used for rendering view specific resources like js or css.
        /// </summary>
        /// <param name="view">The view for which the bundle path is requested</param>
        /// <returns>The bundle path identifier</returns>
        public static string GetViewBundlePath(this WebViewPage view)
        {
            var areaName = view.ViewContext.RouteData.DataTokens["area"] as string;
            
            return string.IsNullOrEmpty(areaName) ? view.GetBundlePathForNormalView() : view.GetBundlePathForAreaView(areaName);
        }

        private static string GetBundlePathForAreaView(this WebPageExecutingBase view, string areaName)
        {
            var viewName = Path.GetFileNameWithoutExtension(view.VirtualPath);

            var bundleName = GetBundleNameForAreaView(view.VirtualPath, viewName, areaName);

            return String.Format("{0}{1}{2}", ViewBundleRegistrar.BUNDLE_NAME_AREA_VIEW_PREFIX, areaName.ToLowerInvariant(), bundleName);
        }

        private static string GetBundlePathForNormalView(this WebViewPage view)
        {
            var controllerName = view.ViewContext.RouteData.Values["controller"] as string;
            var viewName = Path.GetFileNameWithoutExtension(view.VirtualPath);

            var bundleName = GetBundleNameForNormalView(view.VirtualPath, viewName, controllerName);

            return String.Format("{0}{1}", ViewBundleRegistrar.BUNDLE_NAME_VIEW_PREFIX, bundleName);
        }

        /// <summary>
        /// Gets the bundle name for the view
        /// </summary>
        /// <param name="virtualPath">The virtual path of the view</param>
        /// <param name="viewName">The name of the physical view</param>
        /// <param name="areaName">The name of the area where the view is part of</param>
        /// <returns></returns>
        private static string GetBundleNameForAreaView(string virtualPath, string viewName, string areaName)
        {
            var lastIndexAfterView = virtualPath.LastIndexOf(viewName, StringComparison.InvariantCultureIgnoreCase) + viewName.Length;
            var firstIndexAfterArea = virtualPath.IndexOf(areaName, StringComparison.InvariantCultureIgnoreCase) + areaName.Length;

            var bundleName =virtualPath.Substring(firstIndexAfterArea, lastIndexAfterView - firstIndexAfterArea).ToLowerInvariant();

            return bundleName;
        }

        /// <summary>
        /// Gets the bundle name for the view
        /// </summary>
        /// <param name="virtualPath">The virtual path of the view</param>
        /// <param name="viewName">The name of the physical view</param>
        /// <param name="controllerName">This is the controller name of the view</param>
        /// <returns></returns>
        private static string GetBundleNameForNormalView(string virtualPath, string viewName, string controllerName)
        {
            var lastIndexAfterView = virtualPath.LastIndexOf(viewName, StringComparison.InvariantCultureIgnoreCase) + viewName.Length;
            var firstIndexOfController = virtualPath.IndexOf(String.Format("/{0}/", controllerName), StringComparison.InvariantCultureIgnoreCase);

            var bundleName = virtualPath.Substring(firstIndexOfController, lastIndexAfterView - firstIndexOfController).ToLowerInvariant();

            return bundleName;
        }
    }
}