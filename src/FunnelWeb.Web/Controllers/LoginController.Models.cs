

namespace FunnelWeb.Web.Controllers
{
    public partial class LoginController
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
}
