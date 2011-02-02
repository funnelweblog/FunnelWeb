using System.Collections.Generic;
using System.Linq;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;
using FunnelWeb.Settings;

namespace FunnelWeb.Web.Application
{
    public class DatabaseUpgradeDetector : IDatabaseUpgradeDetector
    {
        private readonly IEnumerable<IScriptProvider> _extensions;
        private bool? _updateNeeded;
        private readonly object _lock = new object();

        public DatabaseUpgradeDetector(IEnumerable<IScriptProvider> extensions)
        {
            _extensions = extensions;
        }

        public bool UpdateNeeded()
        {
            if (_updateNeeded != null)
                return _updateNeeded.Value;

            lock (_lock)
            {
                if (_updateNeeded != null)
                    return _updateNeeded.Value;

                var applicationDatabase = new ApplicationDatabase();
                var connectionStringProvider = new ConnectionStringProvider();

                var connectionString = connectionStringProvider.ConnectionString;

                string error;
                if (applicationDatabase.TryConnect(connectionString, out error))
                {
                    var currentVersion = applicationDatabase.GetApplicationCurrentVersion(connectionString);
                    var requiredVersion = applicationDatabase.GetApplicationVersion();
                    var updateNeeded = currentVersion != requiredVersion || ExtensionsRequireUpdate(_extensions, applicationDatabase, connectionString);
                    _updateNeeded = updateNeeded;
                    return updateNeeded;
                }

                _updateNeeded = true;
                return _updateNeeded.Value;
            }
        }

        private static bool ExtensionsRequireUpdate(IEnumerable<IScriptProvider> extensions,
                                                    IApplicationDatabase applicationDatabase, string connectionString)
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