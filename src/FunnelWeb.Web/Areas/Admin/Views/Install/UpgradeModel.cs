using FunnelWeb.DatabaseDeployer.Infrastructure;

namespace FunnelWeb.Web.Areas.Admin.Views.Install
{
    public class UpgradeModel
    {
        public DatabaseUpgradeResults Results { get; set; }
        public Log Log { get; set; }

        public UpgradeModel(DatabaseUpgradeResults results, Log log)
        {
            Results = results;
            Log = log;
        }
    }
}