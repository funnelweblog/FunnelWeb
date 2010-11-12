
namespace FunnelWeb.Web.Features.Login.Views
{
    public class IndexModel
    {
        public bool DatabaseIssue { get; set; }
        public bool PreviousLoginFailed { get; set; }

        public IndexModel(bool databaseIssue, bool previousLoginFailed)
        {
            DatabaseIssue = databaseIssue;
            PreviousLoginFailed = previousLoginFailed;
        }
    }
}