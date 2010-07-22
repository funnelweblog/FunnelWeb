using System.Web.Mvc;
using FunnelWeb.Web.Application.Settings;
using Autofac;

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
            _engines.Clear();
            _engines.Add(new AutofacAwareViewEngine());
        }
    }
}
