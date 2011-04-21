using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using Autofac;
using DbUp;
using DbUp.ScriptProviders;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Extensions.SqlAuthentication;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Repositories;
using NHibernate;

namespace FunnelWeb.Tests.Helpers
{
    public class TemporaryDatabase : IDisposable, IConnectionStringProvider
    {
        private readonly string connectionString;
        private readonly AdHocSqlRunner database;
        private readonly string databaseName;
        private readonly AdHocSqlRunner master;
        private readonly IContainer container;

        public TemporaryDatabase()
        {
            databaseName = "FunnelWebIntegrationTests";
            connectionString = string.Format("Server=(local)\\SQLEXPRESS;Database={0};Trusted_connection=true;Pooling=false", databaseName);
            database = new AdHocSqlRunner(connectionString);

            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = "master";

            master = new AdHocSqlRunner(builder.ToString());

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(this).As<IConnectionStringProvider>();
            containerBuilder.RegisterModule(new RepositoriesModule());
            container = containerBuilder.Build();
        }

        public void WithRepository(Action<IRepository> callback)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                var session = scope.Resolve<ISession>();
                var repo = scope.Resolve<IRepository>();

                var txn = session.BeginTransaction();

                callback(repo);

                session.Flush();

                txn.Commit();
            }
        }

        public string ConnectionString
        {
            get { return connectionString; }
            set { }
        }

        public AdHocSqlRunner AdHoc
        {
            get { return database; }
        }

        public void CreateAndDeploy()
        {
            try
            {
                master.ExecuteNonQuery("alter database [" + databaseName + "] set single_user with rollback immediate");
                master.ExecuteNonQuery("drop database [" + databaseName + "]");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Could not drop integration test database: {0}", ex));
            }
            master.ExecuteNonQuery("create database [" + databaseName + "]");

            var app = new ApplicationDatabase();
            app.PerformUpgrade(connectionString, new List<ScriptedExtension>
                                                     {
                                                         ScriptProviderFor(new SqlAuthenticationExtension())
                                                     }, new TraceLog());
        }

        public ScriptedExtension ScriptProviderFor<T>(T extensionWithScripts) where T : IRequireDatabaseScripts
        {
            var provider = new EmbeddedScriptProvider(
                typeof(T).Assembly,
                x => x.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase));

            return new ScriptedExtension(extensionWithScripts.SourceIdentifier, typeof(T).Assembly, provider);
        }

        public void Dispose()
        {
            master.ExecuteNonQuery("drop database [" + databaseName + "]");
        }

        public class TraceLog : ILog
        {
            public void WriteInformation(string format, params object[] args)
            {
                Trace.TraceInformation(format, args);
            }

            public void WriteError(string format, params object[] args)
            {
                Trace.TraceError(format, args);
            }

            public void WriteWarning(string format, params object[] args)
            {
                Trace.TraceWarning(format, args);
            }

            public IDisposable Indent()
            {
                return new FooDisposable();
            }

            public class FooDisposable : IDisposable
            {
                public void Dispose()
                {
                }
            }
        }
    }
}