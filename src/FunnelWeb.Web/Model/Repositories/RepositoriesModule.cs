using System.Web;
using System.Web.Configuration;
using Autofac;
using Autofac.Integration.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FunnelWeb.Web.Model.Repositories.Internal;
using NHibernate;

namespace FunnelWeb.Web.Model.Repositories
{
    public class RepositoriesModule : Module
    {
        private readonly string _connectionString;

        public RepositoriesModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(_connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()))
                .BuildSessionFactory();

            builder.Register<IFileRepository>(x => new FileRepository(WebConfigurationManager.AppSettings["FunnelWeb.configuration.uploadpath"], x.Resolve<HttpServerUtilityBase>()))
                .HttpRequestScoped();
            builder.RegisterType<FeedRepository>().As<IFeedRepository>()
                .HttpRequestScoped();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>();
            builder.RegisterType<EntryRepository>().As<IEntryRepository>()
                .HttpRequestScoped();
            builder.RegisterInstance(sessionFactory).As<ISessionFactory>();
            builder.Register(x =>
            {
                var session = sessionFactory.OpenSession();
                return session;
            }).As<ISession>().InstancePerLifetimeScope();
        }
    }
}
