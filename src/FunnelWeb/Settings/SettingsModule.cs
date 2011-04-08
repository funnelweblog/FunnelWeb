using Autofac;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Model.Repositories;

namespace FunnelWeb.Settings
{
    public class SettingsModule : Module
    {
        private readonly string bootstrapSettingsFilePath;

        public SettingsModule(string bootstrapSettingsFilePath)
        {
            this.bootstrapSettingsFilePath = bootstrapSettingsFilePath;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ConnectionStringProvider>()
                .As<IConnectionStringProvider>()
                .SingleInstance();

            builder.Register(c => new XmlBootstrapSettings(bootstrapSettingsFilePath))
                .As<IBootstrapSettings>()
                .SingleInstance();

            builder.Register(c => new SettingsProvider(c.Resolve<IAdminRepository>()))
                .As<ISettingsProvider>()
                .InstancePerLifetimeScope();
        }
    }
}
