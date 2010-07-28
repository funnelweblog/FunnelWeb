using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;

namespace FunnelWeb.DatabaseDeployer.Infrastructure.VersionTrackers
{
    /// <summary>
    /// This interface is provided to allow different projects to store version information differently.
    /// </summary>
    public interface IVersionTracker
    {
        /// <summary>
        /// Recalls the version number of a database specified in a given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        int RecallVersionNumber(string connectionString, ILog log);

        /// <summary>
        /// Records a database upgrade for a database specified in a given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="script">The script.</param>
        /// <param name="log">The log.</param>
        void StoreUpgrade(string connectionString, IScript script, ILog log);
    }
}