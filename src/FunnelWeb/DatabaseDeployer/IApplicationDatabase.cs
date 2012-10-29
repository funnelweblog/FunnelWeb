using System;
using System.Collections.Generic;
using System.Data;
using DbUp.Engine;
using DbUp.Engine.Output;

namespace FunnelWeb.DatabaseDeployer
{
    /// <summary>
    /// Provides services for dealing with the application database.
    /// </summary>
    public interface IApplicationDatabase
    {
        string[] GetCoreExecutedScripts(Func<IDbConnection> connectionFactory);
        string[] GetCoreRequiredScripts(Func<IDbConnection> connectionFactory);
        string[] GetExtensionExecutedScripts(Func<IDbConnection> connectionFactory, ScriptedExtension extension);
        string[] GetExtensionRequiredScripts(Func<IDbConnection> connectionFactory, ScriptedExtension extension);

        /// <summary>
        /// Performs the upgrade.
        /// </summary>
        /// <param name="scriptedExtensions">The extensions</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// A container of information about the results of the database upgrade.
        /// </returns>
        DatabaseUpgradeResult[] PerformUpgrade(IEnumerable<ScriptedExtension> scriptedExtensions, IUpgradeLog log);
    }
}