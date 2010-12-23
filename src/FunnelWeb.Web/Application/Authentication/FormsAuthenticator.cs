using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace FunnelWeb.Web.Application.Authentication
{
    public class FormsAuthenticator : IAuthenticator
    {
        public bool AuthenticateAndLogin(string username, string password, bool databaseIssue)
        {
            var authenticated = FormsAuthentication.Authenticate(username, password);
            if (authenticated)
            {
                var genericIdentity = new GenericIdentity(username);

                // Create generic principal.
                var principal = new GenericPrincipal(genericIdentity, new[]{"Admin", "Moderator"});
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