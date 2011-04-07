using System;
using Autofac;

namespace FunnelWeb.DatabaseDeployer
{
    public class DatabaseModule : Module
    {
        private readonly Func<string> connectionStringCallback;

        public DatabaseModule(Func<string> connectionStringCallback)
        {
            this.connectionStringCallback = connectionStringCallback;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterType<DatabaseUpgradeDetector>()
                .As<IDatabaseUpgradeDetector>()
                .WithParameter("connectionStringCallback", connectionStringCallback)
                .SingleInstance();
        }
    }
}
