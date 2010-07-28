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

        public ActionResult Index(bool databaseIssue)
        {
            ViewData.Model = new IndexModel(databaseIssue);
            return View();
        }
        
        public ActionResult Login(bool databaseIssue, string name, string password)
        {
            var authenticated = _authenticator.AuthenticateAndLogin(name, password);
            if (authenticated)
            {
                if (databaseIssue)
                {
                    return RedirectToAction("Index", "Install");
                }
                return Redirect("~/");    
            }

            ViewData.Flash("The username or password could not be authenticated. Please try again.");
            ViewData.Model = new IndexModel(databaseIssue);
            return View("Index");
        }

        public ActionResult Logout()
        {
            _authenticator.Logout();
            return Redirect("/");  
        }
    }
}