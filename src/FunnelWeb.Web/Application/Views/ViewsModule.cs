using System;
using System.Web;
using System.Web.Mvc;
using Autofac;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application.Mvc;

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
            builder.Register(c => new SettingsProvider(c.Resolve<IAdminRepository>(), () => HttpContext.Current.Server.MapPath("~/Content/Styles/Themes")))
                .As<ISettingsProvider>()
                .InstancePerLifetimeScope();

            builder.Register(c => new MarkdownProvider(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority))).As<IMarkdownProvider>().InstancePerLifetimeScope();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.AddGenericMobile<RazorViewEngine>();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}
