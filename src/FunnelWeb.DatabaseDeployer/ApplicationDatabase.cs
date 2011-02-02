using System;
using System.Collections.Generic;
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
        private readonly IScriptExecutor scriptExecutor;
        private readonly IVersionTracker versionTracker;
        private readonly IScriptProvider scriptProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDatabase"/> class.
        /// </summary>
        public ApplicationDatabase()
        {
            scriptExecutor = new SqlScriptExecutor();
            versionTracker = new SchemaVersionsTableSqlVersionTracker();
            scriptProvider = new EmbeddedSqlScriptProvider(
                "FunnelWeb.DatabaseDeployer",
                Assembly.GetExecutingAssembly(),
                versionNumber => string.Format(
                                     "FunnelWeb.DatabaseDeployer.Scripts.Script{0}.sql",
                                     versionNumber.ToString().PadLeft(4, '0')));
        }

        public IScriptProvider ScriptProvider
        {
            get { return scriptProvider; }
        }

        /// <summary>
        /// Gets the current schema version number of the database.
        /// </summary>
        /// <returns>The current version number.</returns>
        public int GetApplicationCurrentVersion(string connectionString)
        {
            return versionTracker.RecallVersionNumber(connectionString, scriptProvider.SourceIdentifier, new Log());
        }

        /// <summary>
        /// Gets the current schema version number that the application requires.
        /// </summary>
        /// <returns>The application version number.</returns>
        public int GetApplicationVersion()
        {
            return scriptProvider.GetHighestScriptVersion();
        }

        public int GetExtensionCurrentVersion(string connectionString, IScriptProvider scriptProviderToCheck)
        {
            return versionTracker.RecallVersionNumber(connectionString, scriptProviderToCheck.SourceIdentifier, new Log());
        }

        public int GetExtensionVersion(IScriptProvider scriptProviderToCheck)
        {
            return scriptProviderToCheck.GetHighestScriptVersion();
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

        /// <summary>
        /// Performs the upgrade.
        /// </summary>
        /// <returns>
        /// A container of information about the results of the database upgrade.
        /// </returns>
        public DatabaseUpgradeResults PerformUpgrade(string connectionString, IEnumerable<IScriptProvider> extensionScriptProviders, ILog log)
        {
            var result = new DatabaseUpgrader(connectionString, scriptProvider, extensionScriptProviders, versionTracker, scriptExecutor)
                .PerformUpgrade(log);
            return result;
        }
    }
}