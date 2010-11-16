using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace FunnelWeb.Web.Application.Authentication
{
    //Installation steps:
    //1. Run InstallSqlAuthentication.sql against your funnelweb database
    //2. Create 'Admin' role using membership utility
    //3. Create user account
    //4. Set funnelweb.configuration.useSqlMembership appSetting in web.config to 'true'
    public class SqlAuthenticator : IAuthenticator
    {
        public bool AuthenticateAndLogin(string username, string password)
        {
            var authenticated = Membership.ValidateUser(username, password);

            if (authenticated)
            {
                var genericIdentity = new GenericIdentity(username);

                // Create generic principal.
                var principal = new GenericPrincipal(genericIdentity, Roles.GetRolesForUser(username));
                HttpContext.Current.User = principal;
                FormsAuthentication.SetAuthCookie(username, true);
            }
            return authenticated;
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}