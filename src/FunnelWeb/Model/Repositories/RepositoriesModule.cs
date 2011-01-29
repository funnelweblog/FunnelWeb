using System;
using System.Collections.Generic;
using System.Web;
using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;
using FunnelWeb.Model.Repositories.Internal;
using FunnelWeb.Settings;
using NHibernate;

namespace FunnelWeb.Model.Repositories
{
    public class RepositoriesModule : Module
    {
        private readonly Func<IEnumerable<IScriptProvider>> _extensionsWithScripts;
        private static ISessionFactory sessionFactory;
        private static readonly object Lock = new object();

        public RepositoriesModule(Func<IEnumerable<IScriptProvider>> extensionsWithScripts)
        {
            _extensionsWithScripts = extensionsWithScripts;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDatabase>().As<IApplicationDatabase>();
            builder.RegisterType<ConnectionStringProvider>().As<IConnectionStringProvider>();

            builder.Register<IFileRepository>(x => new FileRepository(x.Resolve<ISettingsProvider>(), x.Resolve<HttpServerUtilityBase>())).InstancePerLifetimeScope();
            builder.RegisterType<TagRepository>().As<ITagRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EntryRepository>().As<IEntryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FeedRepository>().As<IFeedRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TaskStateRepository>().As<ITaskStateRepository>().InstancePerLifetimeScope();

            builder.Register(x =>
            {
                if (sessionFactory == null)
                {
                    lock (Lock)
                    {
                        if (sessionFactory == null)
                        {
                            var fluentConfiguration = Fluently.Configure()
                                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(new ConnectionStringProvider().ConnectionString).ShowSql())
                                .Mappings(m =>
                                              {
                                                  m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
                                                  //Scan extensions for nHibernate mappings 
                                                  foreach (var extensionScriptProvider in _extensionsWithScripts())
                                                  {
                                                      var providerAssembly = extensionScriptProvider.SourceAssembly;
                                                      m.FluentMappings.AddFromAssembly(providerAssembly);
                                                  }
                                              });

                            

                            sessionFactory = fluentConfiguration.BuildSessionFactory();
                        }
                    }
                }

                var session = sessionFactory.OpenSession();
                return session;
            }).As<ISession>().InstancePerLifetimeScope();
        }
    }
}
