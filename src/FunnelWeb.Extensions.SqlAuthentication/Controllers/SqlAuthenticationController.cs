using System.Web.Mvc;
using FunnelWeb.Extensions.SqlAuthentication.Model;
using FunnelWeb.Settings;
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

        public SqlAuthenticationController(IFunnelWebSqlMembership sqlMembership, ISettingsProvider settingsProvider,
            SqlRoleProvider sqlRoleProvider)
        {
            _sqlMembership = sqlMembership;
            _settingsProvider = settingsProvider;
            _sqlRoleProvider = sqlRoleProvider;
        }

        public ActionResult Index()
        {
            var indexModel = new IndexModel();
            indexModel.IsUsingSqlAuthentication =
                _settingsProvider.GetSettings<SqlAuthSettings>().SqlAuthenticationEnabled;
            return View(indexModel);
        }

        public ActionResult EnableSqlAuthentication()
        {
            if (!_sqlMembership.HasAdminAccount())
            {
                return RedirectToAction("Setup");
            }
            var sqlAuthSettings = _settingsProvider.GetSettings<SqlAuthSettings>();
            sqlAuthSettings.SqlAuthenticationEnabled = true;
            _settingsProvider.SaveSettings(sqlAuthSettings);
            return RedirectToAction("Index");
        }

        public ActionResult DisableSqlAuthentication()
        {
            if (!_sqlMembership.HasAdminAccount())
            {
                return RedirectToAction("Setup");
            }
            var sqlAuthSettings = _settingsProvider.GetSettings<SqlAuthSettings>();
            sqlAuthSettings.SqlAuthenticationEnabled = false;
            _settingsProvider.SaveSettings(sqlAuthSettings);
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

            var sqlAuthSettings = _settingsProvider.GetSettings<SqlAuthSettings>();
            sqlAuthSettings.SqlAuthenticationEnabled = true;
            _settingsProvider.SaveSettings(sqlAuthSettings);

            return RedirectToAction("Index");
        }
    }
}
