using System.Reflection;
using Bindable.DatabaseManagement;
using Bindable.DatabaseManagement.Execution;
using Bindable.DatabaseManagement.ScriptProviders;
using Bindable.DatabaseManagement.VersionTrackers;
using System.Transactions;

namespace FunnelWeb.DatabaseDeployer
{
    /// <summary>
    /// An implementation of the <see cref="IApplicationDatabase"/>.
    /// </summary>
    public class ApplicationDatabase : IApplicationDatabase
    {
        public const string DefaultConnectionString = "Server=(local)\\SQLEXPRESS;Database=FunnelWeb;Trusted_connection=true";

        private readonly string _connectionString;
        private readonly IScriptExecutor _scriptExecutor;
        private readonly IVersionTracker _versionTracker;
        private readonly IScriptProvider _scriptProvider;
        private readonly DatabaseUpgrader _upgrader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDatabase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ApplicationDatabase(string connectionString)
        {
            _connectionString = connectionString;
            _scriptExecutor = new SqlScriptExecutor();
            _versionTracker = new SchemaVersionsTableSqlVersionTracker();
            _scriptProvider = new EmbeddedSqlScriptProvider(
                Assembly.GetExecutingAssembly(),
                versionNumber => string.Format(
                                     "FunnelWeb.DatabaseDeployer.Scripts.Script{0}.sql",
                                     versionNumber.ToString().PadLeft(4, '0')));
            _upgrader = new DatabaseUpgrader(connectionString, _scriptProvider, _versionTracker, _scriptExecutor);
        }

        /// <summary>
        /// Gets the database name.
        /// </summary>
        /// <value></value>
        public string DatabaseName
        {
            get { return SqlDatabaseHelper.GetDatabaseName(_connectionString); }
        }

        /// <summary>
        /// Gets the server name.
        /// </summary>
        /// <value></value>
        public string ServerName
        {
            get { return SqlDatabaseHelper.GetServerName(_connectionString); }
        }

        /// <summary>
        /// Returns a value indicating whether the database can be found.
        /// </summary>
        /// <returns>
        /// True if the database exists and can be contacted, otherwise false.
        /// </returns>
        public bool DoesDatabaseExist()
        {
            return SqlDatabaseHelper.Exists(_connectionString);
        }

        /// <summary>
        /// Creates the database if it does not already exist.
        /// </summary>
        public void CreateDatabase()
        {
            SqlDatabaseHelper.CreateOrContinue(_connectionString);
        }

        /// <summary>
        /// Destroys the database if it exists.
        /// </summary>
        public void DestroyDatabase()
        {
            SqlDatabaseHelper.DestroyOrContinue(_connectionString);
        }

        /// <summary>
        /// Grants the given login membership of the DBO role.
        /// </summary>
        /// <param name="windowsUsername">The windows username.</param>
        public void GrantAccessToLogin(string windowsUsername)
        {
            SqlDatabaseHelper.EnableServerLoginForWindowsUser(_connectionString, windowsUsername);
            SqlDatabaseHelper.EnableDatabaseRoleForLogin(_connectionString, windowsUsername, "db_owner");
        }

        /// <summary>
        /// Gets the current schema version number of the database.
        /// </summary>
        /// <returns>The current version number.</returns>
        public int GetCurrentVersion()
        {
            return _versionTracker.RecallVersionNumber(_connectionString);
        }

        /// <summary>
        /// Gets the current schema version number that the application requires.
        /// </summary>
        /// <returns>The application version number.</returns>
        public int GetApplicationVersion()
        {
            return _scriptProvider.GetHighestScriptVersion();
        }

        /// <summary>
        /// Performs the upgrade.
        /// </summary>
        /// <returns>
        /// A container of information about the results of the database upgrade.
        /// </returns>
        public DatabaseUpgradeResult PerformUpgrade()
        {
            var result = _upgrader.PerformUpgrade();
            return result;
        }
    }
}