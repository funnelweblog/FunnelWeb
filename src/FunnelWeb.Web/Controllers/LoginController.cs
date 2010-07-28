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

        public ActionResult Index(bool? databaseIssue, string ReturnUrl)
        {
            ViewData.Model = new IndexModel(databaseIssue ?? false, false);
            return View();
        }
        
        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Login(bool? databaseIssue, string name, string password)
        {
            var authenticated = _authenticator.AuthenticateAndLogin(name, password);
            if (authenticated)
            {
                if (databaseIssue ?? false)
                {
                    return RedirectToAction("Index", "Install");
                }
                return Redirect("~/");    
            }
            ViewData.Model = new IndexModel(databaseIssue ?? false, true);
            return View("Index");
        }

        public ActionResult Logout()
        {
            _authenticator.Logout();
            return Redirect("/");  
        }
    }
}