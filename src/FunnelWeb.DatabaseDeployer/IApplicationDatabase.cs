using System.Collections.Generic;
using FunnelWeb.DatabaseDeployer.Infrastructure;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;

namespace FunnelWeb.DatabaseDeployer
{
    /// <summary>
    /// Provides services for dealing with the application database.
    /// </summary>
    public interface IApplicationDatabase
    {
        /// <summary>
        /// Gets the current schema version number of the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The current version number.</returns>
        int GetApplicationCurrentVersion(string connectionString);

        /// <summary>
        /// Gets the current schema version number that the application requires.
        /// </summary>
        /// <returns>The application version number.</returns>
        int GetApplicationVersion();

        /// <summary>
        /// Gets the current schema version number of the extension.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="scriptProviderToCheck">The script provider to check the current version of</param>
        /// <returns>The current version number.</returns>
        int GetExtensionCurrentVersion(string connectionString, IScriptProvider scriptProviderToCheck);

        /// <summary>
        /// Gets the current schema version number that the extension requires.
        /// </summary>
        /// <param name="scriptProviderToCheck">The script provider to check the highest version</param>
        /// <returns>The application version number.</returns>
        int GetExtensionVersion(IScriptProvider scriptProviderToCheck);

        /// <summary>
        /// Tries to connect to the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="errorMessage">Any error message encountered.</param>
        /// <returns></returns>
        bool TryConnect(string connectionString, out string errorMessage);

        /// <summary>
        /// Performs the upgrade.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// A container of information about the results of the database upgrade.
        /// </returns>
        DatabaseUpgradeResults PerformUpgrade(string connectionString, IEnumerable<IScriptProvider> extensionScriptProviders, ILog log);
    }
}