using System;
using System.Collections.Generic;
using System.Linq;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;

namespace FunnelWeb.DatabaseDeployer
{
    public class DatabaseUpgradeDetector : IDatabaseUpgradeDetector
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly IEnumerable<IScriptProvider> extensions;
        private readonly IApplicationDatabase database;
        private bool? updateNeeded;
        private readonly object @lock = new object();

        public DatabaseUpgradeDetector(IConnectionStringProvider connectionStringProvider, IEnumerable<IScriptProvider> extensions, IApplicationDatabase database)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.extensions = extensions;
            this.database = database;
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

                string error;
                if (database.TryConnect(connectionString, out error))
                {
                    var currentVersion = database.GetApplicationCurrentVersion(connectionString);
                    var requiredVersion = database.GetApplicationVersion();
                    updateNeeded = currentVersion != requiredVersion || ExtensionsRequireUpdate(extensions, database, connectionString);
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

        private static bool ExtensionsRequireUpdate(IEnumerable<IScriptProvider> extensions, IApplicationDatabase applicationDatabase, string connectionString)
        {
            return (from extensionScriptProvider in extensions
                    let currentVersion =
                        applicationDatabase.GetExtensionCurrentVersion(connectionString, extensionScriptProvider)
                    let requiredVersion = applicationDatabase.GetExtensionVersion(extensionScriptProvider)
                    where currentVersion != requiredVersion
                    select currentVersion).Any();
        }
    }
}