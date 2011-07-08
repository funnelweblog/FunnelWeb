using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using DbUp;
using DbUp.Engine;
using DbUp.Engine.Output;
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

        public string[] GetCoreExecutedScripts(string connectionString, string schema)
        {
            return CreateJournal(connectionString, CoreSourceIdentifier, schema).GetExecutedScripts();
        }

        public string[] GetCoreRequiredScripts()
        {
            return CreateScriptProvider().GetScripts().Select(x => x.Name).ToArray();
        }

        public string[] GetExtensionExecutedScripts(string connectionString, ScriptedExtension extension, string schema)
        {
            return CreateJournal(connectionString, extension.SourceIdentifier, schema).GetExecutedScripts();
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
        public DatabaseUpgradeResult[] PerformUpgrade(string connectionString, string schema, IEnumerable<ScriptedExtension> scriptedExtensions, IUpgradeLog log)
        {
            var results = new List<DatabaseUpgradeResult>();

            // Upgrade core
            var core = Upgrade(connectionString, CreateScriptProvider(), log, CreateJournal(connectionString, CoreSourceIdentifier, schema), schema);
            results.Add(core);

            // Upgrade extensions
            var databaseUpgradeResults = scriptedExtensions
                .Select(extension =>
                        Upgrade(
                            connectionString,
                            extension.ScriptProvider, log,
                            CreateJournal(connectionString, extension.SourceIdentifier, schema),
                            schema));
            results.AddRange(databaseUpgradeResults);

            return results.ToArray();
        }

        private static DatabaseUpgradeResult Upgrade(string connectionString, IScriptProvider scriptProvider, IUpgradeLog log, IJournal journal, string schema)
        {
            var upgradeEngine = DeployChanges.To
                .SqlDatabase(connectionString, schema)
                .WithScripts(scriptProvider)
                .JournalTo(journal)
                .LogTo(log)
                .Build();

            return upgradeEngine.PerformUpgrade();
        }

        private static EmbeddedScriptProvider CreateScriptProvider()
        {
            return new EmbeddedScriptProvider(
                Assembly.GetExecutingAssembly(),
                script =>
                    script.StartsWith("FunnelWeb.DatabaseDeployer.Scripts.Script", StringComparison.InvariantCultureIgnoreCase)
                    && script.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase));
        }

        private static IJournal CreateJournal(string connectionString, string sourceIdentifier, string schema)
        {
            return new FunnelWebJournal(connectionString, sourceIdentifier, new ConsoleUpgradeLog(), schema);
        }
    }
}