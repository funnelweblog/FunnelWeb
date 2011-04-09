using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Authentication.Internal;
using FunnelWeb.Extensions.SqlAuthentication.Model;
using FunnelWeb.Filters;
using FunnelWeb.Settings;
using NHibernate;

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
            var users = _sqlAuthSettings.SqlAuthenticationEnabled ? _sqlMembership.GetUsers() : Enumerable.Empty<User>();

            var indexModel = new IndexModel
                                 {
                                     IsUsingSqlAuthentication = _sqlAuthSettings.SqlAuthenticationEnabled,
                                     Users = users
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
        public ActionResult NewAccount()
        {
            return View(new NewUser());
        }

        [HttpPost]
        public ActionResult NewAccount(NewUser user)
        {
            if (user.Password != user.RepeatPassword)
            {
                ModelState.AddModelError("Password", "Passwords must match");
                ModelState.AddModelError("RepeatPassword", "Passwords must match");
                return RedirectToAction("Setup", new { user });
            }

            _sqlMembership.CreateAccount(user.Name, user.Email, user.Username, user.Password);

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
            
        public ActionResult RemoveRole(int userId, int roleId)
        {
            var user = DependencyResolver.Current.GetService<ISession>()
                .Get<User>(userId);

            var role = user.Roles.SingleOrDefault(r => r.Id == roleId);

            user.Roles.Remove(role);
            role.Users.Remove(user);

            return RedirectToAction("Index");
        }

        public ActionResult AddRole(int userId)
        {
            var service = DependencyResolver.Current.GetService<ISession>();
            var user = service.QueryOver<User>()
                .Where(u => u.Id == userId)
                .Left.JoinQueryOver(u => u.Roles)
                .SingleOrDefault();

            var roles = service
                .QueryOver<Role>()
                .List()
                .Except(user.Roles)
                .ToList();

            return View(new AddRoleModel { User = user, Roles = roles });
        }

        public ActionResult AddUserToRole(int userId, int roleId)
        {
            var service = DependencyResolver.Current.GetService<ISession>();
            var user = service.QueryOver<User>()
                .Where(u => u.Id == userId)
                .Left.JoinQueryOver(u => u.Roles)
                .SingleOrDefault();

            if (user.Roles.SingleOrDefault(r => r.Id == roleId) == null)
            {
                var role = service
                    .Get<Role>(roleId);
                user.Roles.Add(role);
                role.Users.Add(user);
            }


            return RedirectToAction("AddRole", new { userId });
        }
    }
}
