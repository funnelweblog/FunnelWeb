using System;
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

        public FunnelWebViewEngine(Func<ISettingsProvider> settings, IDatabaseUpgradeDetector databaseUpgradeDetector)
        {
            if (databaseUpgradeDetector.UpdateNeeded())
                return;

            _settings = settings();
            _partialViewLocationFormats = PartialViewLocationFormats;
            _viewLocationFormats = ViewLocationFormats;
            _masterLocationFormats = MasterLocationFormats;

            UpdateThemePath(_settings.GetSettings<FunnelWebSettings>());
        }

        protected internal void UpdateThemePath(FunnelWebSettings settings)
        {
            var locationFormats = new List<string>
        	                      	{
        	                      		"~/Themes/" + settings.Theme + "/Views/{1}/{0}.cshtml",
        	                      		"~/Themes/" + settings.Theme + "/Views/Shared/{0}.cshtml",
        	                      		"~/Themes/" + settings.Theme + "/Views/Shared/{1}/{0}.cshtml",
        	                      		"~/Themes/" + settings.Theme + "/Views/Extensions/{1}/{0}.cshtml",
        	                      		"~/Views/Extensions/{1}/{0}.cshtml",
        	                      	};
            locationFormats.AddRange(_partialViewLocationFormats);
            PartialViewLocationFormats = locationFormats.ToArray();

            locationFormats = new List<string>
                                  {
                                      "~/Themes/" + settings.Theme + "/Views/{1}/{0}.cshtml",
                                      "~/Themes/" + settings.Theme + "/Views/Extensions/{1}/{0}.cshtml",
                                      "~/Views/Extensions/{1}/{0}.cshtml",
                                  };
            locationFormats.AddRange(_viewLocationFormats);
            ViewLocationFormats = locationFormats.ToArray();

            locationFormats = new List<string>
                                  {
                                      "~/Themes/" + settings.Theme + "/Views/{1}/{0}.cshtml",
                                      "~/Themes/" + settings.Theme + "/Views/Extensions/{1}/{0}.cshtml",
                                      "~/Themes/" + settings.Theme + "/Views/Shared/{1}/{0}.cshtml",
                                      "~/Themes/" + settings.Theme + "/Views/Shared/{0}.cshtml",
                                      "~/Views/Extensions/{1}/{0}.cshtml",
                                  };
            locationFormats.AddRange(_masterLocationFormats);
            MasterLocationFormats = locationFormats.ToArray();
        }
    }
}