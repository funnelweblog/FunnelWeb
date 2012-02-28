using System;
using System.Data;
using System.Data.SqlClient;
using NHibernate.AdoNet;
using NHibernate.Driver;
using StackExchange.Profiling;

namespace FunnelWeb.DatabaseDeployer.DbProviders
{
    public class ProfiledSqlClientDriver : Sql2008ClientDriver, IEmbeddedBatcherFactoryProvider
    {
        public override IDbConnection CreateConnection()
        {
            return new ProfiledSqlDbConnection(
                new SqlConnection(),
                MiniProfiler.Current);
        }

        public override IDbCommand CreateCommand()
        {
            return new ProfiledSqlDbCommand(
                new SqlCommand(),
                null,
                MiniProfiler.Current);
        }

        Type IEmbeddedBatcherFactoryProvider.BatcherFactoryClass
        {
            get { return typeof(ProfiledClientBatchingBatcherFactory); }
        }
    }
}