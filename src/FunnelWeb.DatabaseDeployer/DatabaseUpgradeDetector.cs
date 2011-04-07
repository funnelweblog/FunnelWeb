using System;
using System.Collections.Generic;
using System.Linq;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;

namespace FunnelWeb.DatabaseDeployer
{
    public class DatabaseUpgradeDetector : IDatabaseUpgradeDetector
    {
        private readonly Func<string> connectionStringCallback;
        private readonly IEnumerable<IScriptProvider> extensions;
        private bool? updateNeeded;
        private readonly object @lock = new object();

        public DatabaseUpgradeDetector(Func<string> connectionStringCallback, IEnumerable<IScriptProvider> extensions)
        {
            this.connectionStringCallback = connectionStringCallback;
            this.extensions = extensions;
        }

        public bool UpdateNeeded()
        {
            if (updateNeeded != null)
                return updateNeeded.Value;

            lock (@lock)
            {
                if (updateNeeded != null)
                    return updateNeeded.Value;

                var applicationDatabase = new ApplicationDatabase();

                var connectionString = connectionStringCallback();

                string error;
                if (applicationDatabase.TryConnect(connectionString, out error))
                {
                    var currentVersion = applicationDatabase.GetApplicationCurrentVersion(connectionString);
                    var requiredVersion = applicationDatabase.GetApplicationVersion();
                    updateNeeded = currentVersion != requiredVersion || ExtensionsRequireUpdate(extensions, applicationDatabase, connectionString);
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