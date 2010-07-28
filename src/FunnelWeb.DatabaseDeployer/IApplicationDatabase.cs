using FunnelWeb.DatabaseDeployer.Infrastructure;

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
        int GetCurrentVersion(string connectionString);

        /// <summary>
        /// Gets the current schema version number that the application requires.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The application version number.</returns>
        int GetApplicationVersion(string connectionString);

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
        DatabaseUpgradeResult PerformUpgrade(string connectionString, ILog log);
    }
}