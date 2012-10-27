using System;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Settings;

namespace FunnelWeb.Web.Application.Mvc
{
    /// <summary>
    /// A view engine that is aware of FunnelWeb's Theme override capabilities.
    /// </summary>
    public class FunnelWebViewEngine : IViewEngine
    {
        private readonly RazorViewEngine fallbackViewEngine = new RazorViewEngine();
        private string lastTheme;
        private RazorViewEngine lastEngine;
        private readonly object @lock = new object();

        private RazorViewEngine CreateRealViewEngine()
        {
            var database = DependencyResolver.Current.GetService<IDatabaseUpgradeDetector>();
            if (database.UpdateNeeded())
            {
                // If the database is offline, we don't have settings support (since they are stored in the database)
                // so we'll use the out-of-the-box razor engine
                return fallbackViewEngine;
            }

            lock (@lock)
            {
                FunnelWebSettings settings;
                try
                {
                    settings = DependencyResolver.Current.GetService<ISettingsProvider>().GetSettings<FunnelWebSettings>();
                    if (settings.Theme == lastTheme)
                    {
                        return lastEngine ?? fallbackViewEngine;
                    }
                }
                catch (Exception)
                {
                    return fallbackViewEngine;
                }

                // Create a new razor view engine using the theme name when prioritizing names for resolving views
                lastEngine = new RazorViewEngine();
                
                lastEngine.PartialViewLocationFormats =
                    new[]
                    {
                        "~/Themes/" + settings.Theme + "/Views/{1}/{0}.cshtml",
                        "~/Themes/" + settings.Theme + "/Views/Shared/{0}.cshtml",
                        "~/Themes/" + settings.Theme + "/Views/Shared/{1}/{0}.cshtml",
                        "~/Themes/" + settings.Theme + "/Views/Extensions/{1}/{0}.cshtml",
                        "~/Views/Extensions/{1}/{0}.cshtml",
                    }.Union(lastEngine.PartialViewLocationFormats).ToArray();

                lastEngine.ViewLocationFormats =
                    new[]
                    {
                        "~/Themes/" + settings.Theme + "/Views/{1}/{0}.cshtml",
                        "~/Themes/" + settings.Theme + "/Views/Extensions/{1}/{0}.cshtml",
                        "~/Views/Extensions/{1}/{0}.cshtml",
                    }.Union(lastEngine.ViewLocationFormats).ToArray();

                lastEngine.MasterLocationFormats =
                    new[]
                    {
                        "~/Themes/" + settings.Theme + "/Views/{1}/{0}.cshtml",
                        "~/Themes/" + settings.Theme + "/Views/Extensions/{1}/{0}.cshtml",
                        "~/Themes/" + settings.Theme + "/Views/Shared/{1}/{0}.cshtml",
                        "~/Themes/" + settings.Theme + "/Views/Shared/{0}.cshtml",
                        "~/Views/Extensions/{1}/{0}.cshtml",
                    }.Union(lastEngine.MasterLocationFormats).ToArray();

                lastTheme = settings.Theme;

                return lastEngine;
            }
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return CreateRealViewEngine().FindPartialView(controllerContext, partialViewName, useCache);
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return CreateRealViewEngine().FindView(controllerContext, viewName, masterName, useCache);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            CreateRealViewEngine().ReleaseView(controllerContext, view);
        }
    }
}