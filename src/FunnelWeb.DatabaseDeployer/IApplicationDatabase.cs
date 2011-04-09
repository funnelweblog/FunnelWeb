using System.Collections.Generic;
using DbUp;

namespace FunnelWeb.DatabaseDeployer
{
    /// <summary>
    /// Provides services for dealing with the application database.
    /// </summary>
    public interface IApplicationDatabase
    {
        string[] GetCoreExecutedScripts(string connectionString);
        string[] GetCoreRequiredScripts();
        string[] GetExtensionExecutedScripts(string connectionString, ScriptedExtension extension);
        string[] GetExtensionRequiredScripts(ScriptedExtension extension);

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
        /// <param name="extensions">The extensions.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// A container of information about the results of the database upgrade.
        /// </returns>
        DatabaseUpgradeResult[] PerformUpgrade(string connectionString, IEnumerable<ScriptedExtension> extensions, ILog log);
    }
}