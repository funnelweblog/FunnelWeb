using System.Collections.Generic;
using System.Web.Mvc;
using FunnelWeb.Settings;

namespace FunnelWeb.Web.Application.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public class FunnelWebViewEngine : RazorViewEngine
    {
        private readonly ISettingsProvider _settings;

        private readonly string[] _partialViewLocationFormats;
        private readonly string[] _viewLocationFormats;
        private readonly string[] _masterLocationFormats;

        public FunnelWebViewEngine(ISettingsProvider settings)
        {
            _settings = settings;
            _partialViewLocationFormats = PartialViewLocationFormats;
            _viewLocationFormats = ViewLocationFormats;
            _masterLocationFormats = MasterLocationFormats;

            if(RequireUpdatedDatabaseHttpModule.DatabaseRequiresUpgrade())
                return;

            UpdateThemePath(_settings.GetSettings());
        }

        protected internal void UpdateThemePath(Settings.Settings settings)
        {
            var locationFormats = new List<string>()
                                                  {
                                                      "~/Themes/" + settings.Theme + "/Views/{1}/{0}.cshtml", 
                                                      "~/Themes/" + settings.Theme + "/Views/Extensions/{1}/{0}.cshtml", 
                                                      "~/Views/Extensions/{1}/{0}.cshtml",
                                                  };
            locationFormats.AddRange(_partialViewLocationFormats);
            PartialViewLocationFormats = locationFormats.ToArray();

            locationFormats = new List<string>()
                                      {
                                          "~/Themes/" + settings.Theme + "/Views/{1}/{0}.cshtml",
                                          "~/Themes/" + settings.Theme + "/Views/Extensions/{1}/{0}.cshtml",
                                          "~/Views/Extensions/{1}/{0}.cshtml",
                                      };
            locationFormats.AddRange(_viewLocationFormats);
            ViewLocationFormats = locationFormats.ToArray();

            locationFormats = new List<string>()
                                      {
                                          "~/Themes/" + settings.Theme + "/Views/{1}/{0}.cshtml",
                                          "~/Themes/" + settings.Theme + "/Views/Extensions/{1}/{0}.cshtml",
                                          "~/Views/Extensions/{1}/{0}.cshtml",
                                      };
            locationFormats.AddRange(_masterLocationFormats);
            MasterLocationFormats = locationFormats.ToArray();
        }
    }
}