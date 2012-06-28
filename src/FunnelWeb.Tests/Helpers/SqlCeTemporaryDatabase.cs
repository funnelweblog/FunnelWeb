using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using Autofac;
using DbUp.Engine.Output;
using DbUp.Helpers;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.DatabaseDeployer.DbProviders;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Repositories;
using NHibernate;

namespace FunnelWeb.Tests.Helpers
{
    public class SqlCeTemporaryDatabase : ITemporaryDatabase
    {
        private readonly string connectionString;
        private readonly AdHocSqlRunner database;
        private readonly IContainer container;
        private readonly string databaseProviderName;
        private readonly SqlCeDatabaseProvider databaseProvider;
        private readonly string databaseFile;

        public SqlCeTemporaryDatabase()
        {
            databaseProviderName = "sqlce";
            databaseProvider = new SqlCeDatabaseProvider();
            databaseFile = Path.Combine(Path.GetTempPath(), "FunnelWeb.sdf");
            connectionString = string.Format("Data Source={0}; Persist Security Info=False", databaseFile);

            database = new AdHocSqlRunner(() => new SqlCeConnection(connectionString), null);

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(this).As<IConnectionStringProvider>();
            containerBuilder.RegisterModule(new RepositoriesModule());
            containerBuilder.RegisterModule(new DatabaseModule());
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

        public void WithSession(Action<ISession> callback)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                var session = scope.Resolve<ISession>();

                var txn = session.BeginTransaction();

                callback(session);

                session.Flush();

                txn.Commit();
            }
        }

        public string DatabaseProvider
        {
            get { return databaseProviderName; }
            set { }
        }

        public string ConnectionString
        {
            get { return connectionString; }
            set { }
        }

        public string Schema
        {
            get { return null; }
            set { }
        }

        public string ReadOnlyReason { get; private set; }

        public AdHocSqlRunner AdHoc
        {
            get { return database; }
        }

        public void CreateAndDeploy()
        {
            try
            {
                string errorMessage;
                if (File.Exists(databaseFile))
                    File.Delete(databaseFile);
                databaseProvider.TryConnect(connectionString, out errorMessage);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Could not drop integration test database: {0}", ex));
            }

            var app = container.Resolve<IApplicationDatabase>();
            app.PerformUpgrade(new List<ScriptedExtension>(), new TraceLog());
        }

        public ScriptedExtension ScriptProviderFor<T>(T extensionWithScripts) where T : IRequireDatabaseScripts
        {
            var scriptProvider = new FunnelWebScriptProvider(
                typeof(T).Assembly,
                x => x.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase),
                databaseProviderName);

            return new ScriptedExtension(extensionWithScripts.SourceIdentifier, typeof(T).Assembly, scriptProvider);
        }

        public void Dispose()
        {
            File.Delete(databaseFile);
        }

        public class TraceLog : IUpgradeLog
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