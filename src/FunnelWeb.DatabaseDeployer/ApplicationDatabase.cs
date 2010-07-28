using System;
using System.Data.SqlClient;
using System.Reflection;
using FunnelWeb.DatabaseDeployer.Infrastructure;
using FunnelWeb.DatabaseDeployer.Infrastructure.Execution;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;
using FunnelWeb.DatabaseDeployer.Infrastructure.VersionTrackers;

namespace FunnelWeb.DatabaseDeployer
{
    /// <summary>
    /// An implementation of the <see cref="IApplicationDatabase"/>.
    /// </summary>
    public class ApplicationDatabase : IApplicationDatabase
    {
        public const string DefaultConnectionString = "Server=(local)\\SQLEXPRESS;Database=FunnelWeb;Trusted_connection=true";
        private readonly IScriptExecutor _scriptExecutor;
        private readonly IVersionTracker _versionTracker;
        private readonly IScriptProvider _scriptProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDatabase"/> class.
        /// </summary>
        public ApplicationDatabase()
        {
            _scriptExecutor = new SqlScriptExecutor();
            _versionTracker = new SchemaVersionsTableSqlVersionTracker();
            _scriptProvider = new EmbeddedSqlScriptProvider(
                Assembly.GetExecutingAssembly(),
                versionNumber => string.Format(
                                     "FunnelWeb.DatabaseDeployer.Scripts.Script{0}.sql",
                                     versionNumber.ToString().PadLeft(4, '0')));
        }

        /// <summary>
        /// Gets the current schema version number of the database.
        /// </summary>
        /// <returns>The current version number.</returns>
        public int GetCurrentVersion(string connectionString)
        {
            return _versionTracker.RecallVersionNumber(connectionString, new Log());
        }

        /// <summary>
        /// Gets the current schema version number that the application requires.
        /// </summary>
        /// <returns>The application version number.</returns>
        public int GetApplicationVersion(string connectionString)
        {
            return _scriptProvider.GetHighestScriptVersion();
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
                var csb = new SqlConnectionStringBuilder(connectionString);
                csb.Pooling = false;
                csb.ConnectTimeout = 5;

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

        /// <summary>
        /// Performs the upgrade.
        /// </summary>
        /// <returns>
        /// A container of information about the results of the database upgrade.
        /// </returns>
        public DatabaseUpgradeResult PerformUpgrade(string connectionString, ILog log)
        {
            var result = new DatabaseUpgrader(connectionString, _scriptProvider, _versionTracker, _scriptExecutor)
                .PerformUpgrade(log);
            return result;
        }
    }
}