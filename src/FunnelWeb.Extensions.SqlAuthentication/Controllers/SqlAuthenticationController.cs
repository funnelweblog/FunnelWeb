using System.Web.Mvc;
using FunnelWeb.Extensions.SqlAuthentication.Model;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application.Authentication;
using FunnelWeb.Web.Application.Filters;

namespace FunnelWeb.Extensions.SqlAuthentication.Controllers
{
    [FunnelWebRequest]
    [Authorize(Roles="Admin")]
    public class SqlAuthenticationController : Controller
    {
        private readonly IFunnelWebSqlMembership _sqlMembership;
        private readonly ISettingsProvider _settingsProvider;
        private readonly SqlRoleProvider _sqlRoleProvider;
        private readonly FormsAuthenticator _formsAuthenticator;
        private readonly SqlAuthenticator _sqlAuthenticator;
        private readonly SqlAuthSettings _sqlAuthSettings;

        public SqlAuthenticationController(
            IFunnelWebSqlMembership sqlMembership, 
            ISettingsProvider settingsProvider,
            SqlRoleProvider sqlRoleProvider, 
            FormsAuthenticator formsAuthenticator, 
            SqlAuthenticator sqlAuthenticator)
        {
            _sqlMembership = sqlMembership;
            _settingsProvider = settingsProvider;
            _sqlRoleProvider = sqlRoleProvider;
            _formsAuthenticator = formsAuthenticator;
            _sqlAuthenticator = sqlAuthenticator;
            _sqlAuthSettings = _settingsProvider.GetSettings<SqlAuthSettings>();
        }

        public ActionResult Index()
        {
            var indexModel = new IndexModel
                                 {
                                     IsUsingSqlAuthentication = _sqlAuthSettings.SqlAuthenticationEnabled
                                 };
            return View(indexModel);
        }

        public ActionResult EnableSqlAuthentication()
        {
            if (!_sqlMembership.HasAdminAccount())
            {
                return RedirectToAction("Setup");
            }
            var sqlAuthSettings = _sqlAuthSettings;
            sqlAuthSettings.SqlAuthenticationEnabled = true;
            _settingsProvider.SaveSettings(sqlAuthSettings);
            _formsAuthenticator.Logout();
            return RedirectToAction("Index");
        }

        public ActionResult DisableSqlAuthentication()
        {
            if (!_sqlMembership.HasAdminAccount())
            {
                return RedirectToAction("Setup");
            }
            var sqlAuthSettings = _sqlAuthSettings;
            sqlAuthSettings.SqlAuthenticationEnabled = false;
            _settingsProvider.SaveSettings(sqlAuthSettings);
            _sqlAuthenticator.Logout();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Setup()
        {
            var setupModel = new SetupModel
                                 {
                                     HasAdminAccount = _sqlMembership.HasAdminAccount()
                                 };

            return View(setupModel);
        }

        [HttpPost]
        public ActionResult Setup(SetupModel setupModel)
        {
            return View(setupModel);
        }

        public ActionResult CreateAdminAccount(SetupModel setupModel)
        {
            if (!_sqlMembership.HasAdminAccount())
            {
                if (setupModel.Password != setupModel.RepeatPassword)
                {
                    ModelState.AddModelError("Password", "Passwords must match");
                    ModelState.AddModelError("RepeatPassword", "Passwords must match");
                    return RedirectToAction("Setup", new{setupModel});
                }

                var user = _sqlMembership.CreateAccount(setupModel.Name, setupModel.Email, setupModel.Username, setupModel.Password);
                _sqlRoleProvider.AddUserToRoles(user, "Admin", "Moderator");
            }

            var sqlAuthSettings = _sqlAuthSettings;
            sqlAuthSettings.SqlAuthenticationEnabled = true;
            _settingsProvider.SaveSettings(sqlAuthSettings);
            _formsAuthenticator.Logout();
            return RedirectToAction("Index");
        }
    }
}
