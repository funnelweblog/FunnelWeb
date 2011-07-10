using System;
using System.Data;
using System.Data.SqlClient;
using DbUp;
using DbUp.Builder;
using FluentNHibernate.Cfg.Db;

namespace FunnelWeb.DatabaseDeployer.DbProviders
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

        public IPersistenceConfigurer GetDatabaseConfiguration(IConnectionStringProvider connectionStringProvider)
        {
            return MsSqlConfiguration.MsSql2008.ConnectionString(connectionStringProvider.ConnectionString)
                .ShowSql()
                .DefaultSchema(connectionStringProvider.Schema);
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