using System.Web.Optimization;
using FunnelWeb.Web.Application.Mvc;

namespace FunnelWeb.Web.App_Start
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/disqus-count").Include("~/Scripts/disqus-count.js"));
            bundles.Add(new ScriptBundle("~/bundles/jsdate").Include("~/Scripts/jsdate.js"));
            
            // Get the latest version from here: https://code.google.com/p/pagedown/source/browse/
            bundles.Add(new ScriptBundle("~/bundles/pagedown").Include(
                        "~/Scripts/Markdown.Converter.js",
                        "~/Scripts/Markdown.Editor.js",
                        "~/Scripts/Markdown.Sanitizer.js"));

            bundles.Add(new ScriptBundle("~/bundles/taggy").Include("~/Scripts/taggy.js"));
            bundles.Add(new       Bundle("~/bundles/prettify").Include("~/Scripts/Prettify/prettify.js", "~/Scripts/Prettify/lang-*"));
            bundles.Add(new ScriptBundle("~/bundles/site").Include("~/Scripts/site.js"));
            
            bundles.Add(new StyleBundle("~/Content/themes/base/baseCss").Include("~/Content/themes/base/Base.css"));
            bundles.Add(new StyleBundle("~/Content/themes/base/adminCss").Include("~/Content/themes/base/Base.css", "~/Content/themes/base/Admin.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            ViewBundleRegistrar.RegisterViewBundles(bundles);
        }

    }
}