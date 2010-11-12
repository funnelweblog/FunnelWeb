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
        private readonly List<IScript> scripts;
        private readonly int originalVersion;
        private readonly int upgradedVersion;
        private readonly bool successful;
        private readonly Exception error;

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
            this.scripts = new List<IScript>();
            this.scripts.AddRange(scripts);
            originalVersion = original;
            upgradedVersion = upgraded;
            this.successful = successful;
            this.error = error;
        }

        /// <summary>
        /// Gets the scripts that were executed.
        /// </summary>
        public IEnumerable<IScript> Scripts
        {
            get { return scripts; }
        }

        /// <summary>
        /// Gets the original version that the database was at before the upgrade.
        /// </summary>
        public int OriginalVersion
        {
            get { return originalVersion; }
        }

        /// <summary>
        /// Gets the version number that the database was upgraded to.
        /// </summary>
        public int UpgradedVersion
        {
            get { return upgradedVersion; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="DatabaseUpgradeResult"/> is successful.
        /// </summary>
        public bool Successful
        {
            get { return successful; }
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Exception Error
        {
            get { return error; }
        }
    }
}