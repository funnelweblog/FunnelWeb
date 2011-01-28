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