using System;
using System.Data;
using DbUp.Builder;
using FluentNHibernate.Cfg.Db;

namespace FunnelWeb.DatabaseDeployer.DbProviders
{
    public interface IDatabaseProvider
    {
        string DefaultConnectionString { get; }
        bool SupportSchema { get; }
        bool SupportFuture { get; }

        /// <summary>
        /// Tries to connect to the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="errorMessage">Any error message encountered.</param>
        /// <returns></returns>
        bool TryConnect(string connectionString, out string errorMessage);

        IPersistenceConfigurer GetDatabaseConfiguration(IConnectionStringProvider connectionStringProvider);
        Func<IDbConnection> GetConnectionFactory(string connectionString);
        UpgradeEngineBuilder GetUpgradeEngineBuilder(string connectionString, string schema);
    }
}