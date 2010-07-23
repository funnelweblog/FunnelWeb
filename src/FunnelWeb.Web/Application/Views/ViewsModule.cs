using System.Web.Mvc;
using FunnelWeb.Web.Application.Settings;
using Autofac;
using System.Web;
using System;

namespace FunnelWeb.Web.Application.Views
{
    public class ViewsModule : Module
    {
        private readonly ViewEngineCollection _engines;

        public ViewsModule(ViewEngineCollection engines)
        {
            _engines = engines;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SettingsProvider>().As<ISettingsProvider>().InstancePerLifetimeScope();
            builder.Register(c => new MarkdownProvider(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority))).As<IMarkdownProvider>().InstancePerLifetimeScope();

            _engines.Clear();
            _engines.Add(new AutofacAwareViewEngine());
        }
    }
}
