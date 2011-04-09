using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using DbUp;
using DbUp.Execution;
using DbUp.Journal;
using DbUp.ScriptProviders;
using FunnelWeb.DatabaseDeployer.Infrastructure;

namespace FunnelWeb.DatabaseDeployer
{
    /// <summary>
    /// An implementation of the <see cref="IApplicationDatabase"/>.
    /// </summary>
    public class ApplicationDatabase : IApplicationDatabase
    {
        public const string DefaultConnectionString = "Server=(local)\\SQLEXPRESS;Database=FunnelWeb;Trusted_connection=true";
        private const string CoreSourceIdentifier = "FunnelWeb.DatabaseDeployer";

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDatabase"/> class.
        /// </summary>
        public ApplicationDatabase()
        {
        }

        public string[] GetCoreExecutedScripts(string connectionString)
        {
            return CreateJournal(connectionString, CoreSourceIdentifier).GetExecutedScripts();
        }

        public string[] GetCoreRequiredScripts()
        {
            return CreateScriptProvider().GetScripts().Select(x => x.Name).ToArray();
        }

        public string[] GetExtensionExecutedScripts(string connectionString, ScriptedExtension extension)
        {
            return CreateJournal(connectionString, extension.SourceIdentifier).GetExecutedScripts();
        }

        public string[] GetExtensionRequiredScripts(ScriptedExtension extension)
        {
            return extension.ScriptProvider.GetScripts().Select(x => x.Name).ToArray();
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
        public DatabaseUpgradeResult[] PerformUpgrade(string connectionString, IEnumerable<ScriptedExtension> scriptedExtensions, ILog log)
        {
            var results = new List<DatabaseUpgradeResult>();

            // Upgrade core
            var core = Upgrade(connectionString, CreateScriptProvider(), CreateJournal(connectionString, CoreSourceIdentifier));
            results.Add(core);

            // Upgrade extensions
            foreach (var extension in scriptedExtensions)
            {
                var ex = Upgrade(connectionString, extension.ScriptProvider, CreateJournal(connectionString, extension.SourceIdentifier));
                results.Add(ex);
            }

            return results.ToArray();
        }

        private DatabaseUpgradeResult Upgrade(string connectionString, IScriptProvider scriptProvider, IJournal journal)
        {
            var upgrader = new DatabaseUpgrader(
                connectionString,
                scriptProvider,
                journal,
                new SqlScriptExecutor(connectionString));

            var result = upgrader.PerformUpgrade();
            return result;
        }

        private static EmbeddedScriptProvider CreateScriptProvider()
        {
            return new EmbeddedScriptProvider(
                Assembly.GetExecutingAssembly(),
                script =>
                    script.StartsWith("FunnelWeb.DatabaseDeployer.Scripts.Script", StringComparison.InvariantCultureIgnoreCase)
                    && script.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase));
        }

        private static IJournal CreateJournal(string connectionString, string sourceIdentifier)
        {
            return new FunnelWebJournal(connectionString, sourceIdentifier, new ConsoleLog());
        }
    }
}