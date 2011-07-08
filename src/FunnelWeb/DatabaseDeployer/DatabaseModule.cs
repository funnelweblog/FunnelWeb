using System;
using Autofac;
using FunnelWeb.DatabaseDeployer.DbProviders;

namespace FunnelWeb.DatabaseDeployer
{
    public class DatabaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ApplicationDatabase>()
                .As<IApplicationDatabase>()
                .SingleInstance();

            builder
                .RegisterType<DatabaseUpgradeDetector>()
                .As<IDatabaseUpgradeDetector>()
                .SingleInstance();

            builder
                .RegisterType<SqlDatabaseProvider>()
                .As<IDatabaseProvider>()
                .Named<IDatabaseProvider>("sql")
                .WithMetadata<IDatabaseProviderMetadata>(c => c.For(m => m.Name, "Sql"));

            builder
                .RegisterType<SqlCeDatabaseProvider>()
                .As<IDatabaseProvider>()
                .Named<IDatabaseProvider>("sqlce")
                .WithMetadata<IDatabaseProviderMetadata>(c => c.For(m => m.Name, "SqlCe"));
        }
    }
}
