namespace FunnelWeb.Web.Features.Installation.Views
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
}