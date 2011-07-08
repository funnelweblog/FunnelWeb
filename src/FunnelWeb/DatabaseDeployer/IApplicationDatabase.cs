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
        string[] GetCoreExecutedScripts(Func<IDbConnection> connectionFactory, string schema);
        string[] GetCoreRequiredScripts();
        string[] GetExtensionExecutedScripts(Func<IDbConnection> connectionFactory, ScriptedExtension extension, string schema);
        string[] GetExtensionRequiredScripts(ScriptedExtension extension);

        /// <summary>
        /// Performs the upgrade.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="schema">The schema to use</param>
        /// <param name="extensions">The extensions.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// A container of information about the results of the database upgrade.
        /// </returns>
        DatabaseUpgradeResult[] PerformUpgrade(Func<IDbConnection> connectionFactory, string schema, IEnumerable<ScriptedExtension> extensions, IUpgradeLog log);
    }
}