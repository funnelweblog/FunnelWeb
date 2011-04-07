using Autofac;
using FunnelWeb.Model.Repositories;

namespace FunnelWeb.Settings
{
    public class SettingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder.Register(c => new SettingsProvider(c.Resolve<IAdminRepository>()))
                .As<ISettingsProvider>()
                .InstancePerLifetimeScope();
        }
    }
}
