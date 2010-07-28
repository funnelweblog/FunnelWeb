

namespace FunnelWeb.Web.Controllers
{
	public partial class LoginController
	{
        public class IndexModel
        {
            public bool DatabaseIssue { get; set; }

            public IndexModel(bool databaseIssue)
            {
                DatabaseIssue = databaseIssue;
            }
        }
	}
}
