using Autofac;
using Autofac.Features.Indexed;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Providers.Database;
using FunnelWeb.Providers.File;
using FunnelWeb.Settings;

namespace FunnelWeb.Providers
{
    public class InternalProviderRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterFileRepositoryProviders(builder);
            RegisterDatabaseProviders(builder);
        }

        private static void RegisterDatabaseProviders(ContainerBuilder builder)
        {
            builder
                .RegisterType<SqlDatabaseProvider>()
                .Named<IDatabaseProvider>("sql")
                .WithMetadata<IProviderMetadata>(c => c.For(m => m.Name, "Sql"));

            builder
                .RegisterType<SqlCeDatabaseProvider>()
                .Named<IDatabaseProvider>("sqlce")
                .WithMetadata<IProviderMetadata>(c => c.For(m => m.Name, "SqlCe"));

            builder
                .Register(ProviderInfo.For<IDatabaseProvider>)
                .As<IProviderInfo<IDatabaseProvider>>()
                .SingleInstance();

            builder.Register(
                c =>
                {
                    var providerLookup = c.Resolve<IIndex<string, IDatabaseProvider>>();
                    var databaseProvider = c.Resolve<IConnectionStringSettings>().DatabaseProvider.ToLower();
                    return providerLookup[databaseProvider];
                })
                .As<IDatabaseProvider>()
                .InstancePerLifetimeScope();
        }

        private static void RegisterFileRepositoryProviders(ContainerBuilder builder)
        {
            builder
                .RegisterType<AzureBlobFileRepository>()
                .Named<IFileRepository>(AzureBlobFileRepository.ProviderName)
                .WithMetadata<IProviderMetadata>(c => c.For(m => m.Name, AzureBlobFileRepository.ProviderName))
                .InstancePerLifetimeScope();

            builder
                .RegisterType<FileRepository>()
                .Named<IFileRepository>(FileRepository.ProviderName)
                .WithMetadata<IProviderMetadata>(c => c.For(m => m.Name, FileRepository.ProviderName))
                .InstancePerLifetimeScope();

            builder
                .Register(ProviderInfo.For<IFileRepository>)
                .As<IProviderInfo<IFileRepository>>()
                .SingleInstance();

            builder.Register(
                c =>
                {
                    var providerLookup = c.Resolve<IIndex<string, IFileRepository>>();
                    var funnelWebSettings = c.Resolve<ISettingsProvider>().GetSettings<FunnelWebSettings>();
                    var databaseProvider = funnelWebSettings.StorageProvider;
                    return providerLookup[databaseProvider];
                })
                .As<IFileRepository>()
                .InstancePerLifetimeScope();
        }
    }
}