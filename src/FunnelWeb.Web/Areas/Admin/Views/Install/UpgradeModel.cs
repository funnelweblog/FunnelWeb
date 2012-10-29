using DbUp.Engine;

namespace FunnelWeb.Web.Areas.Admin.Views.Install
{
    public class UpgradeModel
    {
        public UpgradeModel(DatabaseUpgradeResult[] results, string log)
        {
            Results = results;
            Log = log;
        }

        public string Log { get; set; }
        public DatabaseUpgradeResult[] Results { get; set; }
    }
}