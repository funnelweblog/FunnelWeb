using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Model.Repositories.Internal;
using FunnelWeb.Repositories;
using FunnelWeb.Settings;
using NHibernate;

namespace FunnelWeb.Model.Repositories
{
    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<IFileRepository>(x => new FileRepository(x.Resolve<ISettingsProvider>(), x.Resolve<HttpServerUtilityBase>())).InstancePerLifetimeScope();
            builder.RegisterType<TagRepository>().As<ITagRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FeedRepository>().As<IFeedRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TaskStateRepository>().As<ITaskStateRepository>().InstancePerLifetimeScope();

            builder.RegisterType<NHibernateRepository>().As<IRepository>().InstancePerLifetimeScope();

            builder.Register(ConfigureSessionFactory).As<ISessionFactory>().SingleInstance();
            builder.Register(c => c.Resolve<ISessionFactory>().OpenSession()).As<ISession>().InstancePerLifetimeScope();
        }

        private static ISessionFactory ConfigureSessionFactory(IComponentContext context)
        {
            var configuration =
                Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(context.Resolve<IConnectionStringProvider>().ConnectionString).ShowSql())
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

                    //Scan extensions for nHibernate mappings 
                    var extension = context.Resolve<IEnumerable<ScriptedExtension>>();
                    foreach (var assembly in extension.Select(provider => provider.SourceAssembly))
                    {
                        m.FluentMappings.AddFromAssembly(assembly);
                    }
                });

            return configuration.BuildSessionFactory();
        }
    }
}
