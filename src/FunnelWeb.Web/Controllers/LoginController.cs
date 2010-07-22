using System.Web.Mvc;
using FunnelWeb.Web.Application.Authentication;
using FunnelWeb.Web.Application.Extensions;

namespace FunnelWeb.Web.Controllers
{
    public partial class LoginController : Controller
    {
        private readonly IAuthenticator _authenticator;

        public LoginController(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public ActionResult Index()
        {
            ViewData.Model = new IndexModel(false);
            return View();
        }
        
        public ActionResult Login(string name, string password)
        {
            var authenticated = _authenticator.AuthenticateAndLogin(name, password);
            if (authenticated)
            {
                return Redirect("/");    
            }

            ViewData.Model = new IndexModel(true);
            return View("Index");
        }

        public ActionResult Logout()
        {
            _authenticator.Logout();
            return Redirect("/");  
        }
    }
}