using System.Collections.Generic;
using System.Linq;
using Autofac;
using FluentNHibernate.Cfg;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Model.Mappings;
using FunnelWeb.Model.Repositories.Internal;
using FunnelWeb.Providers.Database;
using FunnelWeb.Providers.File;
using FunnelWeb.Repositories;
using NHibernate;
using NHibernate.Bytecode;

namespace FunnelWeb.Model.Repositories
{
    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdminRepository>().As<IAdminRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TaskStateRepository>().As<ITaskStateRepository>().InstancePerLifetimeScope();

            builder.RegisterType<NHibernateRepository>().As<IRepository>().InstancePerLifetimeScope();

            builder.Register(ConfigureSessionFactory).As<ISessionFactory>().SingleInstance();
            builder.Register(c => c.Resolve<ISessionFactory>().OpenSession()).As<ISession>().InstancePerLifetimeScope();
        }

        private static ISessionFactory ConfigureSessionFactory(IComponentContext context)
        {
            var connectionStringProvider = context.Resolve<IConnectionStringSettings>();
            EntryMapping.CurrentSchema = connectionStringProvider.Schema;
            var databaseProvider = context.ResolveNamed<IDatabaseProvider>(connectionStringProvider.DatabaseProvider.ToLower());

            var databaseConfiguration = databaseProvider.GetDatabaseConfiguration(connectionStringProvider);
            var configuration =
                Fluently
                    .Configure()
                    .Database(databaseConfiguration)
                    .Mappings(m =>
                                  {
                                      m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

                                      //Scan extensions for nHibernate mappings 
                                      var extension = context.Resolve<IEnumerable<ScriptedExtension>>();
                                      foreach (var assembly in extension.Select(provider => provider.SourceAssembly))
                                      {
                                          m.FluentMappings.AddFromAssembly(assembly);
                                      }
                                  })
                    .ProxyFactoryFactory(typeof (DefaultProxyFactoryFactory));

            return configuration.BuildSessionFactory();
        }
    }
}
