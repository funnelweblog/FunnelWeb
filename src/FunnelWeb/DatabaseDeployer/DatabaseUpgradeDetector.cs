using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using FunnelWeb.DatabaseDeployer.DbProviders;

namespace FunnelWeb.DatabaseDeployer
{
    public class DatabaseUpgradeDetector : IDatabaseUpgradeDetector
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly IEnumerable<ScriptedExtension> extensions;
        private readonly IApplicationDatabase database;
        private readonly IIndex<string, IDatabaseProvider> currentProviderLookup;
        private bool? updateNeeded;
        private readonly object @lock = new object();

        public DatabaseUpgradeDetector(
            IConnectionStringProvider connectionStringProvider, 
            IEnumerable<ScriptedExtension> extensions, 
            IApplicationDatabase database,
            IIndex<string, IDatabaseProvider> currentProviderLookup)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.extensions = extensions;
            this.database = database;
            this.currentProviderLookup = currentProviderLookup;
        }

        public bool UpdateNeeded()
        {
            if (updateNeeded != null)
                return updateNeeded.Value;

            lock (@lock)
            {
                if (updateNeeded != null)
                    return updateNeeded.Value;

                var connectionString = connectionStringProvider.ConnectionString;
                var schema = connectionStringProvider.Schema;

                string error;
                var databaseProvider = currentProviderLookup[connectionStringProvider.DatabaseProvider.ToLower()];
                if (databaseProvider.TryConnect(connectionString, out error))
                {
                    var currentScripts = database.GetCoreExecutedScripts(databaseProvider.GetConnectionFactory(connectionString), schema);
                    var requiredScripts = database.GetCoreRequiredScripts();
                    var notRun = requiredScripts.Select(x => x.Trim().ToLowerInvariant())
                        .Except(currentScripts.Select(x => x.Trim().ToLowerInvariant()))
                        .ToList();

                    updateNeeded = notRun.Count > 0
                        || ExtensionsRequireUpdate(extensions, database, databaseProvider, connectionString, schema);
                }
                else
                {
                    updateNeeded = true;
                }

                return updateNeeded.Value;
            }
        }

        public void Reset()
        {
            lock (@lock)
                updateNeeded = null;
        }

        private static bool ExtensionsRequireUpdate(IEnumerable<ScriptedExtension> extensions, IApplicationDatabase applicationDatabase, 
            IDatabaseProvider databaseProvider, string connectionString, string schema)
        {
            return (from x in extensions
                    let current = applicationDatabase.GetExtensionExecutedScripts(databaseProvider.GetConnectionFactory(connectionString), x, schema)
                    let required = applicationDatabase.GetExtensionRequiredScripts(x)
                    let notRun = required.Select(z => z.Trim().ToLowerInvariant())
                        .Except(current.Select(z => z.Trim().ToLowerInvariant()))
                        .ToList()
                    where notRun.Count > 0
                    select current).Any();
        }
    }
}