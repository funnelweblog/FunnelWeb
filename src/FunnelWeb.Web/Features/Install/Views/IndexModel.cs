using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FunnelWeb.Web.Features.Install.Views
{
    public class IndexModel
    {
        public bool CanConnect { get; set; }
        public int CurrentVersion { get; set; }
        public int NewVersion { get; set; }
        public string ConnectionError { get; set; }

        [Required]
        [DisplayName("Connection string")]
        [Description("Enter the connection string to the Microsoft SQL Server database given to you by your web host.")]
        public string ConnectionString { get; set; }
        public bool IsInstall { get { return CurrentVersion == 0; } }
    }
}