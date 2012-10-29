using System;
using System.Data;
using System.Data.SqlClient;
using DbUp;
using DbUp.Builder;
using FluentNHibernate.Cfg.Db;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Providers.Database.Sql;

namespace FunnelWeb.Providers.Database
{
    public class SqlDatabaseProvider : IDatabaseProvider
    {
        public string DefaultConnectionString
        {
            get { return @"database=FunnelWeb;server=.\SQLEXPRESS;trusted_connection=true;"; }
        }

        public bool SupportSchema
        {
            get { return true; }
        }

        public bool SupportFuture
        {
            get { return true; }
        }

        public bool SupportsFullText
        {
            get { return true; }
        }

        /// <summary>
        /// Tries to connect to the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="errorMessage">Any error message encountered.</param>
        /// <returns></returns>
        public bool TryConnect(string connectionString, out string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    errorMessage = "No connection string specified";
                    return false;
                }

                var csb = new SqlConnectionStringBuilder(connectionString)
                {
                    Pooling = false,
                    ConnectTimeout = 5
                };

                errorMessage = "";
                using (var connection = new SqlConnection(csb.ConnectionString))
                {
                    connection.Open();

                    new SqlCommand("select 1", connection).ExecuteScalar();
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public IPersistenceConfigurer GetDatabaseConfiguration(IConnectionStringSettings connectionStringSettings)
        {
            return MsSqlConfiguration.MsSql2008.ConnectionString(connectionStringSettings.ConnectionString)
                .Driver<ProfiledSqlClientDriver>()
                .ShowSql()
                .DefaultSchema(connectionStringSettings.Schema);
        }

        public Func<IDbConnection> GetConnectionFactory(string connectionString)
        {
            return () => new SqlConnection(connectionString);
        }

        public UpgradeEngineBuilder GetUpgradeEngineBuilder(string connectionString, string schema)
        {
            return DeployChanges.To
                .SqlDatabase(GetConnectionFactory(connectionString), schema);
        }
    }
}