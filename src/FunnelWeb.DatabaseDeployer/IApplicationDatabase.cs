using Bindable.DatabaseManagement;

namespace FunnelWeb.DatabaseDeployer
{
    /// <summary>
    /// Provides services for dealing with the application database.
    /// </summary>
    public interface IApplicationDatabase
    {
        /// <summary>
        /// Gets the database name.
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        /// Gets the server name.
        /// </summary>
        string ServerName { get; }

        /// <summary>
        /// Returns a value indicating whether the database can be found.
        /// </summary>
        /// <returns>True if the database exists and can be contacted, otherwise false.</returns>
        bool DoesDatabaseExist();

        /// <summary>
        /// Creates the database if it does not already exist.
        /// </summary>
        void CreateDatabase();

        /// <summary>
        /// Destroys the database if it exists.
        /// </summary>
        void DestroyDatabase();

        /// <summary>
        /// Grants the given login membership of the DBO role.
        /// </summary>
        /// <param name="windowsUsername">The windows username.</param>
        void GrantAccessToLogin(string windowsUsername);

        /// <summary>
        /// Gets the current schema version number of the database. 
        /// </summary>
        /// <returns>The current version number.</returns>
        int GetCurrentVersion();

        /// <summary>
        /// Gets the current schema version number that the application requires. 
        /// </summary>
        /// <returns>The application version number.</returns>
        int GetApplicationVersion();

        /// <summary>
        /// Performs the upgrade.
        /// </summary>
        /// <returns>A container of information about the results of the database upgrade.</returns>
        DatabaseUpgradeResult PerformUpgrade();
    }
}