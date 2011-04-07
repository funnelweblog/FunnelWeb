using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FunnelWeb.Authentication;
using FunnelWeb.Authentication.Internal;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Extensions.SqlAuthentication.Model;
using FunnelWeb.Settings;
using NHibernate;

namespace FunnelWeb.Extensions.SqlAuthentication
{
    public class SqlAuthenticator : IAuthenticator
    {
        private readonly FormsAuthenticator formsAuthenticator;
        private readonly Func<IDatabaseUpgradeDetector> upgradeDetector;
        private readonly Func<ISettingsProvider> settingsProvider;
        private readonly ISession session;

        public SqlAuthenticator(FormsAuthenticator formsAuthenticator, Func<IDatabaseUpgradeDetector> upgradeDetector, Func<ISettingsProvider> settingsProvider, ISession session)
        {
            this.formsAuthenticator = formsAuthenticator;
            this.upgradeDetector = upgradeDetector;
            this.settingsProvider = settingsProvider;
            this.session = session;
        }

        public string GetName()
        {
            return UseFormsAuthentication
                       ? formsAuthenticator.GetName()
                       : SqlGetName();
        }

        private static string SqlGetName()
        {
            var username = ((FormsIdentity) HttpContext.Current.User.Identity).Name;

            var session = DependencyResolver.Current.GetService<ISession>();
            var user = session.QueryOver<User>()
                .Where(u => u.Username == username)
                .SingleOrDefault();

            return user.Name;
        }

        public bool AuthenticateAndLogin(string username, string password)
        {
            return UseFormsAuthentication ?
                formsAuthenticator.AuthenticateAndLogin(username, password) :
                SqlAuthenticateAndLogin(username, password);
        }

        private bool UseFormsAuthentication
        {
            get
            {
                return upgradeDetector().UpdateNeeded() ||
                    !settingsProvider().GetSettings<SqlAuthSettings>().SqlAuthenticationEnabled;
            }
        }

        private bool SqlAuthenticateAndLogin(string username, string password)
        {
            var user = session.QueryOver<User>()
                .Where(u => u.Username == username && u.Password == FunnelWebSqlMembership.HashPassword(password))
                .SingleOrDefault();

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(username, true);
                return true;
            }

            return false;
        }

        public void Logout()
        {
            formsAuthenticator.Logout();
        }
    }
}
