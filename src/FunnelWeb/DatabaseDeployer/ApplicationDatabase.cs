using System;
using System.Collections.Generic;
using System.Data;
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
        private const string CoreSourceIdentifier = "FunnelWeb.DatabaseDeployer";

        public string[] GetCoreExecutedScripts(Func<IDbConnection> connectionFactory, string schema)
        {
            return CreateJournal(connectionFactory, CoreSourceIdentifier, schema).GetExecutedScripts();
        }

        public string[] GetCoreRequiredScripts()
        {
            return CreateScriptProvider().GetScripts().Select(x => x.Name).ToArray();
        }

        public string[] GetExtensionExecutedScripts(Func<IDbConnection> connectionFactory, ScriptedExtension extension, string schema)
        {
            return CreateJournal(connectionFactory, extension.SourceIdentifier, schema).GetExecutedScripts();
        }

        public string[] GetExtensionRequiredScripts(ScriptedExtension extension)
        {
            return extension.ScriptProvider.GetScripts().Select(x => x.Name).ToArray();
        }

        /// <summary>
        /// Performs the upgrade.
        /// </summary>
        /// <returns>
        /// A container of information about the results of the database upgrade.
        /// </returns>
        public DatabaseUpgradeResult[] PerformUpgrade(Func<IDbConnection> connectionFactory, string schema, IEnumerable<ScriptedExtension> scriptedExtensions, IUpgradeLog log)
        {
            var results = new List<DatabaseUpgradeResult>();

            // Upgrade core
            var core = Upgrade(connectionFactory, CreateScriptProvider(), log, CreateJournal(connectionFactory, CoreSourceIdentifier, schema), schema);
            results.Add(core);

            // Upgrade extensions
            var databaseUpgradeResults = scriptedExtensions
                .Select(extension =>
                        Upgrade(
                            connectionFactory,
                            extension.ScriptProvider, log,
                            CreateJournal(connectionFactory, extension.SourceIdentifier, schema),
                            schema));
            results.AddRange(databaseUpgradeResults);

            return results.ToArray();
        }

        private static DatabaseUpgradeResult Upgrade(Func<IDbConnection> connectionFactory, IScriptProvider scriptProvider, IUpgradeLog log, IJournal journal, string schema)
        {
            var upgradeEngine = DeployChanges.To
                .SqlDatabase(connectionFactory, schema)
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

        private static IJournal CreateJournal(Func<IDbConnection> connectionFactory, string sourceIdentifier, string schema)
        {
            return new FunnelWebJournal(connectionFactory, sourceIdentifier, new ConsoleUpgradeLog(), schema);
        }
    }
}