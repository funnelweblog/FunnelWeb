using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Autofac.Features.Indexed;
using DbUp.Engine;
using DbUp.Engine.Output;
using FunnelWeb.DatabaseDeployer.DbProviders;
using FunnelWeb.DatabaseDeployer.Infrastructure;

namespace FunnelWeb.DatabaseDeployer
{
    /// <summary>
    /// An implementation of the <see cref="IApplicationDatabase"/>.
    /// </summary>
    public class ApplicationDatabase : IApplicationDatabase
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly IIndex<string, IDatabaseProvider> providerLookup;
        private const string CoreSourceIdentifier = "FunnelWeb.DatabaseDeployer";

        public ApplicationDatabase(
            IConnectionStringProvider connectionStringProvider,
            IIndex<string, IDatabaseProvider> providerLookup)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.providerLookup = providerLookup;
        }

        public string[] GetCoreExecutedScripts(Func<IDbConnection> connectionFactory)
        {
            var provider = providerLookup[connectionStringProvider.DatabaseProvider.ToLower()];

            return CreateJournal(connectionFactory, CoreSourceIdentifier, provider.SupportSchema ? connectionStringProvider.Schema : null).GetExecutedScripts();
        }

        public string[] GetCoreRequiredScripts()
        {
            return CreateScriptProvider(connectionStringProvider.DatabaseProvider).GetScripts().Select(x => x.Name).ToArray();
        }

        public string[] GetExtensionExecutedScripts(Func<IDbConnection> connectionFactory, ScriptedExtension extension)
        {
            return CreateJournal(connectionFactory, extension.SourceIdentifier, connectionStringProvider.Schema).GetExecutedScripts();
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
        public DatabaseUpgradeResult[] PerformUpgrade(IEnumerable<ScriptedExtension> scriptedExtensions, IUpgradeLog log)
        {
            var results = new List<DatabaseUpgradeResult>();
            
            var provider = providerLookup[connectionStringProvider.DatabaseProvider.ToLower()];
            var schema = provider.SupportSchema ? connectionStringProvider.Schema : null;
            var connectionFactory = provider.GetConnectionFactory(connectionStringProvider.ConnectionString);

            // Upgrade core
            var core = Upgrade(
                provider, CreateScriptProvider(connectionStringProvider.DatabaseProvider), log,
                CreateJournal(connectionFactory, CoreSourceIdentifier, schema), schema);
            results.Add(core);

            // Upgrade extensions
            var databaseUpgradeResults = scriptedExtensions
                .Select(extension =>
                        Upgrade(
                        provider, extension.ScriptProvider, log,
                            CreateJournal(connectionFactory, extension.SourceIdentifier, schema),
                            schema));
            results.AddRange(databaseUpgradeResults);

            return results.ToArray();
        }

        private DatabaseUpgradeResult Upgrade(IDatabaseProvider provider, IScriptProvider scriptProvider, IUpgradeLog log, IJournal journal, string schema)
        {
            var upgradeEngine = provider.GetUpgradeEngineBuilder(connectionStringProvider.ConnectionString, schema)
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