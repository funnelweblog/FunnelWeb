using System;
using System.Web.Mvc;
using FunnelWeb.Authentication;
using FunnelWeb.Settings;
using FunnelWeb.Web.Areas.Admin.Views.Login;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    public class LoginController : Controller
    {
        readonly Lazy<IBootstrapSettings> bootstrapSettings;

        public LoginController(Lazy<IBootstrapSettings> bootstrapSettings)
        {
            this.bootstrapSettings = bootstrapSettings;
        }

        public IAuthenticator Authenticator { get; set; }

        [HttpGet]
        public virtual ActionResult Login(LoginModel model)
        {
            ModelState.Clear();

            if (model.DatabaseIssue == true)
            {
                model.ConfigFileMissing = bootstrapSettings.Value.ConfigFileMissing();
            }

            return View(model);
        }
        
        [ActionName("Login")]
        [HttpPost]
        public virtual ActionResult LoginPost(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var authenticated = Authenticator.AuthenticateAndLogin(model.Username, model.Password);
            if (authenticated)
            {
                return (model.DatabaseIssue ?? false)
                    ? (ActionResult)RedirectToAction("Index", "Install")
                    : Redirect(string.IsNullOrWhiteSpace(model.ReturnUrl) ? "~/" : model.ReturnUrl);
            }

            ModelState.AddModelError("", "Invalid username or password. Please try again.");
            return View(model);
        }
        
        public virtual ActionResult Logout()
        {
            Authenticator.Logout();
            return Redirect(Url.Content("~/"));  
        }
    }
}