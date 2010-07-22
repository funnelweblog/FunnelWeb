using System.Web.Configuration;
using Autofac;
using Autofac.Integration.Web;
using Bindable.Core;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Web.Model.Repositories.Internal;

namespace FunnelWeb.Web.Model.Repositories
{
    public class RepositoriesModule : Module
    {
        private readonly bool _automaticallyCreateDatabase;
        private readonly string _connectionString;

        public RepositoriesModule(bool automaticallyCreateDatabase, string connectionString)
        {
            _automaticallyCreateDatabase = automaticallyCreateDatabase;
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_automaticallyCreateDatabase)
            {
                var databaseManager = new ApplicationDatabase(_connectionString);
                databaseManager.CreateDatabase();
                var upgradeResult = databaseManager.PerformUpgrade();
                if (upgradeResult.Successful)
                {
                    TraceSources.Current.TraceInformation("Database version is {0}", upgradeResult.UpgradedVersion);
                }
                else
                {
                    TraceSources.Current.TraceInformation("Database could not be upgraded: {0}", upgradeResult.Error.Message);
                    throw upgradeResult.Error;
                }
            }

            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(_connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()))
                .BuildSessionFactory();

            builder.RegisterType<FileRepository>().As<IFileRepository>().HttpRequestScoped().WithParameter(new NamedParameter("root", WebConfigurationManager.AppSettings["FunnelWeb.configuration.uploadpath"]));
            builder.RegisterType<FeedRepository>().As<IFeedRepository>().HttpRequestScoped();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>();
            builder.RegisterType<EntryRepository>().As<IEntryRepository>().HttpRequestScoped();
            builder.RegisterInstance(sessionFactory).As<ISessionFactory>();
            builder.Register(x =>
            {
                var session = sessionFactory.OpenSession();
                return session;
            }).As<ISession>().InstancePerLifetimeScope();
        }
    }
}
