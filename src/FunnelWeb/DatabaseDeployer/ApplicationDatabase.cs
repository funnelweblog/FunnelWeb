using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using DbUp.Engine;
using DbUp.Engine.Output;
using FunnelWeb.DatabaseDeployer.Infrastructure;
using FunnelWeb.Providers.Database;

namespace FunnelWeb.DatabaseDeployer
{
    /// <summary>
    /// An implementation of the <see cref="IApplicationDatabase"/>.
    /// </summary>
    public class ApplicationDatabase : IApplicationDatabase
    {
        private readonly IConnectionStringSettings connectionStringSettings;
        private readonly Func<IDatabaseProvider> databaseProvider;
        private const string CoreSourceIdentifier = "FunnelWeb.DatabaseDeployer";

        public ApplicationDatabase(
            IConnectionStringSettings connectionStringSettings,
            Func<IDatabaseProvider> databaseProvider)
        {
            this.connectionStringSettings = connectionStringSettings;
            this.databaseProvider = databaseProvider;
        }

        public string[] GetCoreExecutedScripts(Func<IDbConnection> connectionFactory)
        {
            return CreateJournal(connectionFactory, CoreSourceIdentifier, databaseProvider().SupportSchema ? connectionStringSettings.Schema : null).GetExecutedScripts();
        }

        public string[] GetCoreRequiredScripts(Func<IDbConnection> connectionFactory)
        {
            return CreateScriptProvider(connectionStringSettings.DatabaseProvider).GetScripts(connectionFactory).Select(x => x.Name).ToArray();
        }

        public string[] GetExtensionExecutedScripts(Func<IDbConnection> connectionFactory, ScriptedExtension extension)
        {
            return CreateJournal(connectionFactory, extension.SourceIdentifier, connectionStringSettings.Schema).GetExecutedScripts();
        }

        public string[] GetExtensionRequiredScripts(Func<IDbConnection> connectionFactory, ScriptedExtension extension)
        {
            return extension.ScriptProvider.GetScripts(connectionFactory).Select(x => x.Name).ToArray();
        }

        /// <summary>
        /// Performs the upgrade.
        /// </summary>
        /// <returns>
        /// A container of information about the results of the database upgrade.
        /// </returns>
        public DatabaseUpgradeResult[] PerformUpgrade(IEnumerable<ScriptedExtension> scriptedExtensions, IUpgradeLog log)
        {
            var results = new List<DatabaseUpgradeResult>();
            
            var schema = databaseProvider().SupportSchema ? connectionStringSettings.Schema : null;
            var connectionFactory = databaseProvider().GetConnectionFactory(connectionStringSettings.ConnectionString);

            // Upgrade core
            var core = Upgrade(
                databaseProvider(), CreateScriptProvider(connectionStringSettings.DatabaseProvider), log,
                CreateJournal(connectionFactory, CoreSourceIdentifier, schema), schema);
            results.Add(core);

            // Upgrade extensions
            var databaseUpgradeResults = scriptedExtensions
                .Select(extension =>
                        Upgrade(
                        databaseProvider(), extension.ScriptProvider, log,
                            CreateJournal(connectionFactory, extension.SourceIdentifier, schema),
                            schema));
            results.AddRange(databaseUpgradeResults);

            return results.ToArray();
        }

        private DatabaseUpgradeResult Upgrade(IDatabaseProvider provider, IScriptProvider scriptProvider, IUpgradeLog log, IJournal journal, string schema)
        {
            var upgradeEngine = provider.GetUpgradeEngineBuilder(connectionStringSettings.ConnectionString, schema)
                .WithScripts(scriptProvider)
                .JournalTo(journal)
                .LogTo(log)
                .Build();

            return upgradeEngine.PerformUpgrade();
        }

        private static IScriptProvider CreateScriptProvider(string databaseProviderName)
        {
            return new FunnelWebScriptProvider(
                Assembly.GetExecutingAssembly(),
                script =>
                    script.StartsWith("FunnelWeb.DatabaseDeployer.Scripts.Script", StringComparison.InvariantCultureIgnoreCase)
                    && script.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase),
                    databaseProviderName);
        }

        private static IJournal CreateJournal(Func<IDbConnection> connectionFactory, string sourceIdentifier, string schema)
        {
            return new FunnelWebJournal(connectionFactory, sourceIdentifier, new ConsoleUpgradeLog(), schema);
        }
    }
}