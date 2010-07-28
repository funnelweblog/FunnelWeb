
using System;
using System.Web.Mvc;
using FunnelWeb.DatabaseDeployer.Infrastructure;

namespace FunnelWeb.Web.Controllers
{
    public partial class InstallController
    {
        public class IndexModel
        {
            public bool CanConnect { get; set; }
            public int CurrentVersion { get; set; }
            public int NewVersion { get; set; }
            public string ConnectionError { get; set; }
            public string ConnectionString { get; set; }
            public bool IsInstall { get { return CurrentVersion == 0; } }
        }

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
}