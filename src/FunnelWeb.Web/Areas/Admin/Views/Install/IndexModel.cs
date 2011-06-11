using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FunnelWeb.Web.Areas.Admin.Views.Install
{
    public class IndexModel
    {
        public bool CanConnect { get; set; }
        public string ConnectionError { get; set; }

        public string[] ScriptsToRun { get; set; }

        [Required]
        [DisplayName("Connection string")]
        [Description("Enter the connection string to the Microsoft SQL Server database given to you by your web host.")]
        public string ConnectionString { get; set; }
        public bool IsInstall { get; set; }

        [Required]
        [DisplayName("Schema Name")]
        [Description("Enter the database schema to use. (To host multiple FunnelWeb sites in a single database)")]
        public string Schema { get; set; }
    }
}