
namespace FunnelWeb.Web.Controllers
{
	public partial class LoginController
	{
        public class IndexModel
        {
            public IndexModel(bool previousLoginFailed)
            {
                PreviousLoginFailed = previousLoginFailed;
            }

            public bool PreviousLoginFailed { get; set; }

        }
	}
}
