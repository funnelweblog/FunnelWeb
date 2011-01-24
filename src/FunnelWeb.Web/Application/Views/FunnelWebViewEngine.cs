using System.Collections.Generic;
using System.Web.Mvc;
using FunnelWeb.Settings;

namespace FunnelWeb.Web.Application.Views
{
    /// <summary>
    /// 
    /// </summary>
    public class FunnelWebViewEngine : RazorViewEngine
    {
        private readonly ISettingsProvider _settings;

        public FunnelWebViewEngine(ISettingsProvider settings)
        {
            _settings = settings;

            UpdateThemePath();
        }

        protected internal void UpdateThemePath()
        {
            var locationFormats = PartialViewLocationFormats;
            PartialViewLocationFormats = new List<string>(locationFormats)
                                                  {
                                                      "~/Views/Themes/" + _settings.GetSettings().Theme + "/{1}/{0}.cshtml", 
                                                      "~/Views/Themes/" + _settings.GetSettings().Theme + "/Extensions/{1}/{0}.cshtml", 
                                                      "~/Views/Extensions/{0}.cshtml",
                                                  }.ToArray();

            locationFormats = ViewLocationFormats;
            ViewLocationFormats = new List<string>(locationFormats)
                                      {
                                          "~/Views/Themes/" + _settings.GetSettings().Theme + "/{1}/{0}.cshtml",
                                          "~/Views/Themes/" + _settings.GetSettings().Theme +
                                          "/Extensions/{1}/{0}.cshtml",
                                          "~/Views/Extensions/{0}.cshtml",
                                      }.ToArray();

            locationFormats = MasterLocationFormats;
            MasterLocationFormats = new List<string>(locationFormats)
                                      {
                                          "~/Views/Themes/" + _settings.GetSettings().Theme + "/{1}/{0}.cshtml",
                                          "~/Views/Themes/" + _settings.GetSettings().Theme +
                                          "/Extensions/{1}/{0}.cshtml",
                                          "~/Views/Extensions/{0}.cshtml",
                                      }.ToArray();
        }
    }
}