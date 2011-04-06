using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FunnelWeb.Authentication;
using FunnelWeb.Authentication.Internal;
using FunnelWeb.Extensions.SqlAuthentication.Model;
using FunnelWeb.Settings;
using NHibernate;

namespace FunnelWeb.Extensions.SqlAuthentication
{
    public class SqlAuthenticator : IAuthenticator
    {
        private readonly FormsAuthenticator _formsAuthenticator;

        public SqlAuthenticator(FormsAuthenticator formsAuthenticator)
        {
            _formsAuthenticator = formsAuthenticator;
        }

        public string GetName()
        {
            return UseFormsAuthentication
                       ? _formsAuthenticator.GetName()
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
                _formsAuthenticator.AuthenticateAndLogin(username, password) :
                SqlAuthenticateAndLogin(username, password);
        }

        private static bool UseFormsAuthentication
        {
            get
            {
                return DependencyResolver.Current.GetService<IDatabaseUpgradeDetector>().UpdateNeeded() ||
                    !DependencyResolver.Current.GetService<ISettingsProvider>().GetSettings<SqlAuthSettings>().SqlAuthenticationEnabled;
            }
        }

        private static bool SqlAuthenticateAndLogin(string username, string password)
        {
            var session = DependencyResolver.Current.GetService<ISession>();
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
            _formsAuthenticator.Logout();
        }
    }
}
