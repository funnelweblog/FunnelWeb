using System;
using System.Collections.Generic;
using System.Diagnostics;
using FunnelWeb.DatabaseDeployer.Infrastructure.Execution;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;
using FunnelWeb.DatabaseDeployer.Infrastructure.VersionTrackers;

namespace FunnelWeb.DatabaseDeployer.Infrastructure
{
    /// <summary>
    /// This class orchestrates the database upgrade process.
    /// </summary>
    public class DatabaseUpgrader
    {
        private readonly string _connectionString;
        private readonly IScriptProvider _scriptProvider;
        private readonly IVersionTracker _versionTracker;
        private readonly IScriptExecutor _scriptExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseUpgrader"/> class.
        /// </summary>
        public DatabaseUpgrader(string connectionString, IScriptProvider scriptProvider, IVersionTracker versionTracker, IScriptExecutor scriptExecutor)
        {
            _connectionString = connectionString;
            _scriptExecutor = scriptExecutor;
            _versionTracker = versionTracker;
            _scriptProvider = scriptProvider;
        }

        /// <summary>
        /// Performs the database upgrade.
        /// </summary>
        public DatabaseUpgradeResult PerformUpgrade(ILog log)
        {
            var originalVersion = 0;
            var currentVersion = 0;
            var maximumVersion = 0;
            var scripts = new List<IScript>();
            try
            {
                log.WriteInformation("Beginning database upgrade. Connection string is: '{0}'", _connectionString);

                originalVersion = _versionTracker.RecallVersionNumber(_connectionString, log);
                maximumVersion = _scriptProvider.GetHighestScriptVersion();

                currentVersion = originalVersion;
                while (currentVersion < maximumVersion)
                {
                    currentVersion++;
                    log.WriteInformation("Upgrading to version: '{0}'", currentVersion);
                    using (log.Indent())
                    {
                        var script = _scriptProvider.GetScript(currentVersion);
                        _scriptExecutor.Execute(_connectionString, script, log);
                        _versionTracker.StoreUpgrade(_connectionString, script, log);
                        scripts.Add(script);
                    }
                }
                log.WriteInformation("Upgrade successful");
                return new DatabaseUpgradeResult(scripts, originalVersion, currentVersion, true, null);
            }
            catch (Exception ex)
            {
                log.WriteError("Upgrade failed", ex);
                return new DatabaseUpgradeResult(scripts, originalVersion, currentVersion, false, ex);
            }
        }
    }
}