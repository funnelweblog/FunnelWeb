using System.Web;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.Web.Mvc;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Model.Repositories.Internal;
using FunnelWeb.Settings;
using NHibernate;

namespace FunnelWeb.Model.Repositories
{
    public class RepositoriesModule : Module
    {
        private static ISessionFactory sessionFactory;
        private static readonly object Lock = new object();
        
        public RepositoriesModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDatabase>().As<IApplicationDatabase>();
            builder.RegisterType<ConnectionStringProvider>().As<IConnectionStringProvider>();

            builder.Register<IFileRepository>(x => new FileRepository(x.Resolve<ISettingsProvider>(), x.Resolve<HttpServerUtilityBase>())).InstancePerHttpRequest();
            builder.RegisterType<FeedRepository>().As<IFeedRepository>().InstancePerHttpRequest();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>();
            builder.RegisterType<EntryRepository>().As<IEntryRepository>().InstancePerHttpRequest();
            builder.Register(x =>
            {
                if (sessionFactory == null)
                {
                    lock (Lock)
                    {
                        if (sessionFactory == null)
                        {
                            sessionFactory = Fluently.Configure()
                                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(new ConnectionStringProvider().ConnectionString))
                                .Mappings(m => m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()))
                                .BuildSessionFactory();
                        }
                    }
                }

                var session = sessionFactory.OpenSession();
                return session;
            }).As<ISession>().InstancePerLifetimeScope();
        }
    }
}
