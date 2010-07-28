using System;
using System.Collections.Generic;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;

namespace FunnelWeb.DatabaseDeployer.Infrastructure
{
    /// <summary>
    /// Represents the results of a database upgrade.
    /// </summary>
    public sealed class DatabaseUpgradeResult
    {
        private readonly List<IScript> _scripts;
        private readonly int _originalVersion;
        private readonly int _upgradedVersion;
        private readonly bool _successful;
        private readonly Exception _error;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseUpgradeResult"/> class.
        /// </summary>
        /// <param name="scripts">The scripts.</param>
        /// <param name="original">The original.</param>
        /// <param name="upgraded">The upgraded.</param>
        /// <param name="successful">if set to <c>true</c> [successful].</param>
        /// <param name="error">The error.</param>
        public DatabaseUpgradeResult(IEnumerable<IScript> scripts, int original, int upgraded, bool successful, Exception error)
        {
            _scripts = new List<IScript>();
            _scripts.AddRange(scripts);
            _originalVersion = original;
            _upgradedVersion = upgraded;
            _successful = successful;
            _error = error;
        }

        /// <summary>
        /// Gets the scripts that were executed.
        /// </summary>
        public IEnumerable<IScript> Scripts
        {
            get { return _scripts; }
        }

        /// <summary>
        /// Gets the original version that the database was at before the upgrade.
        /// </summary>
        public int OriginalVersion
        {
            get { return _originalVersion; }
        }

        /// <summary>
        /// Gets the version number that the database was upgraded to.
        /// </summary>
        public int UpgradedVersion
        {
            get { return _upgradedVersion; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="DatabaseUpgradeResult"/> is successful.
        /// </summary>
        public bool Successful
        {
            get { return _successful; }
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Exception Error
        {
            get { return _error; }
        }
    }
}