using System;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Authentication.Internal;
using FunnelWeb.Filters;
using FunnelWeb.Model.Authentication;
using FunnelWeb.Settings;
using FunnelWeb.Web.Areas.Admin.Views.SqlAuthentication;
using NHibernate;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
    [FunnelWebRequest]
    [Authorize(Roles="Admin")]
    public class SqlAuthenticationController : Controller
    {
        private readonly Func<ISession> sessionFactory;
        private readonly SqlFunnelWebMembership sqlMembership;
        private readonly ISettingsProvider settingsProvider;
        private readonly SqlRoleProvider sqlRoleProvider;
        private readonly FormsAuthenticator formsAuthenticator;
        private readonly SqlAuthenticator sqlAuthenticator;
        private readonly SqlAuthSettings sqlAuthSettings;

        public SqlAuthenticationController(
            Func<ISession> sessionFactory,
            SqlFunnelWebMembership sqlMembership, 
            ISettingsProvider settingsProvider,
            SqlRoleProvider sqlRoleProvider, 
            FormsAuthenticator formsAuthenticator, 
            SqlAuthenticator sqlAuthenticator)
        {
            this.sessionFactory = sessionFactory;
            this.sqlMembership = sqlMembership;
            this.settingsProvider = settingsProvider;
            this.sqlRoleProvider = sqlRoleProvider;
            this.formsAuthenticator = formsAuthenticator;
            this.sqlAuthenticator = sqlAuthenticator;
            sqlAuthSettings = this.settingsProvider.GetSettings<SqlAuthSettings>();
        }

        public ActionResult Index()
        {
            var users = sqlAuthSettings.SqlAuthenticationEnabled ? sqlMembership.GetUsers() : Enumerable.Empty<User>();

            var indexModel = new IndexModel
                                 {
                                     IsUsingSqlAuthentication = sqlAuthSettings.SqlAuthenticationEnabled,
                                     Users = users
                                 };
            return View(indexModel);
        }

        public ActionResult EnableSqlAuthentication()
        {
            if (!sqlMembership.HasAdminAccount())
            {
                return RedirectToAction("Setup");
            }

            sqlAuthSettings.SqlAuthenticationEnabled = true;
            settingsProvider.SaveSettings(sqlAuthSettings);

            formsAuthenticator.Logout();
            return RedirectToAction("Index");
        }

        public ActionResult DisableSqlAuthentication()
        {
            if (!sqlMembership.HasAdminAccount())
            {
                return RedirectToAction("Setup");
            }

            sqlAuthSettings.SqlAuthenticationEnabled = false;
            settingsProvider.SaveSettings(sqlAuthSettings);
            sqlAuthenticator.Logout();
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

            sqlMembership.CreateAccount(user.Name, user.Email, user.Username, user.Password);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Setup()
        {
            var setupModel = new SetupModel
                                 {
                                     HasAdminAccount = sqlMembership.HasAdminAccount()
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
            if (!sqlMembership.HasAdminAccount())
            {
                if (setupModel.Password != setupModel.RepeatPassword)
                {
                    ModelState.AddModelError("Password", "Passwords must match");
                    ModelState.AddModelError("RepeatPassword", "Passwords must match");
                    return RedirectToAction("Setup", new{setupModel});
                }

                var user = sqlMembership.CreateAccount(setupModel.Name, setupModel.Email, setupModel.Username, setupModel.Password);
                sqlRoleProvider.AddUserToRoles(user, "Admin", "Moderator");
            }

            sqlAuthSettings.SqlAuthenticationEnabled = true;
            settingsProvider.SaveSettings(sqlAuthSettings);

            formsAuthenticator.Logout();
            return RedirectToAction("Index");
        }
            
        public ActionResult RemoveRole(int userId, int roleId)
        {
            var user = sessionFactory().Get<User>(userId);

            var role = user.Roles.SingleOrDefault(r => r.Id == roleId);

            if (role == null)
                return RedirectToAction("Index");

            user.Roles.Remove(role);
            role.Users.Remove(user);

            return RedirectToAction("Index");
        }

        public ActionResult AddRole(int userId)
        {
            var session = sessionFactory();
            var user = session.QueryOver<User>()
                .Where(u => u.Id == userId)
                .Left.JoinQueryOver(u => u.Roles)
                .SingleOrDefault();

            var roles = session
                .QueryOver<Role>()
                .List()
                .Except(user.Roles)
                .ToList();

            return View(new AddRoleModel { User = user, Roles = roles });
        }

        public ActionResult AddUserToRole(int userId, int roleId)
        {
            var session = sessionFactory();

            var user = session.QueryOver<User>()
                .Where(u => u.Id == userId)
                .Left.JoinQueryOver(u => u.Roles)
                .SingleOrDefault();

            if (user.Roles.SingleOrDefault(r => r.Id == roleId) == null)
            {
                var role = session
                    .Get<Role>(roleId);
                user.Roles.Add(role);
                role.Users.Add(user);
            }


            return RedirectToAction("AddRole", new { userId });
        }
    }
}
