using System.Web.Mvc;
using FunnelWeb.Web.Application.Settings;
using Autofac;
using System.Web;
using System;

namespace FunnelWeb.Web.Application.Views
{
    public class ViewsModule : Module
    {
        private readonly ViewEngineCollection engines;

        public ViewsModule(ViewEngineCollection engines)
        {
            this.engines = engines;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SettingsProvider>().As<ISettingsProvider>().InstancePerLifetimeScope();
            builder.Register(c => new MarkdownProvider(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority))).As<IMarkdownProvider>().InstancePerLifetimeScope();

            var engine = new WebFormViewEngine();
            engine.ViewLocationFormats = new[]
                {
                    "~/Features/{1}/Views/{0}.aspx",
                    "~/Features/{1}/Views/{0}.ascx",
                };
            engine.PartialViewLocationFormats = new[]
                {
                    "~/Content/Shared/{0}.ascx",
                    "~/Features/{1}/Views/{0}.ascx",
                };
            engines.Clear();
            engines.Add(engine);
        }
    }
}
