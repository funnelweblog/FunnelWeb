using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Tasks;
using FunnelWeb.Web.Application.Authentication;
using FunnelWeb.Web.Views.Login;

namespace FunnelWeb.Web.Controllers
{
    [ValidateInput(false)]
    public class LoginController : Controller
    {
        public IAuthenticator Authenticator { get; set; }
        public ITaskExecutor<MyTask> TaskExecutor { get; set; }

        [HttpGet]
        public virtual ActionResult Login(LoginModel model)
        {
            ModelState.Clear();
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
            TaskExecutor.Execute(new { foo = "bar" });

            Authenticator.Logout();
            return Redirect("/");  
        }
    }
}