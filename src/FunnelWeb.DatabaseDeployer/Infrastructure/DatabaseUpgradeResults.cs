using System.Collections.Generic;

namespace FunnelWeb.DatabaseDeployer.Infrastructure
{
    public class DatabaseUpgradeResults
    {
        public DatabaseUpgradeResults(DatabaseUpgradeResult applicationUpgradeResult, IEnumerable<DatabaseUpgradeResult> extensionUpgradeResults)
        {
            ApplicationUpgradeResult = applicationUpgradeResult;
            ExtensionsUpgradeResults = extensionUpgradeResults;
        }

        public DatabaseUpgradeResult ApplicationUpgradeResult { get; private set; }

        public IEnumerable<DatabaseUpgradeResult> ExtensionsUpgradeResults { get; private set; }
    }
}