using FunnelWeb.DatabaseDeployer.Infrastructure;

namespace FunnelWeb.Web.Features.Install.Views
{
    public class UpgradeModel
    {
        public DatabaseUpgradeResult Result { get; set; }
        public Log Log { get; set; }

        public UpgradeModel(DatabaseUpgradeResult result, Log log)
        {
            Result = result;
            Log = log;
        }
    }
}