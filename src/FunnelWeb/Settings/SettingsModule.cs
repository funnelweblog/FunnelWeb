using System;
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

            builder.RegisterType<ConnectionStringSettings>()
                .As<IConnectionStringSettings>()
                .SingleInstance();

            // This is for Azure connection string support, needs more thought on how to adapt/detect Azure
            //builder.Register(c => new ConfigSettingsAdapter(new XmlConfigSettings(bootstrapSettingsFilePath)))
            builder.Register(c => new XmlConfigSettings(bootstrapSettingsFilePath))
                .As<IConfigSettings>()
                .SingleInstance();

            builder.RegisterType<AppHarborSettings>()
                .As<IAppHarborSettings>()
                .SingleInstance();

            builder.Register(c => new SettingsProvider(c.Resolve<Lazy<IAdminRepository>>()))
                .As<ISettingsProvider>()
                .InstancePerLifetimeScope();
        }
    }
}
