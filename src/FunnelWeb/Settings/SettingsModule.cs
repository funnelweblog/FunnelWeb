using Autofac;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Model.Repositories;

namespace FunnelWeb.Settings
{
    public class SettingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ConnectionStringProvider>().As<IConnectionStringProvider>();

            builder.RegisterType<XmlBootstrapSettings>().As<IBootstrapSettings>().SingleInstance();

            builder.Register(c => new SettingsProvider(c.Resolve<IAdminRepository>()))
                .As<ISettingsProvider>()
                .InstancePerLifetimeScope();
        }
    }
}
