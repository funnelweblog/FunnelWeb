using FunnelWeb.Web.Application.Authentication;

namespace FunnelWeb.Extensions.SqlAuthentication
{
    public class SqlAuthenticator : IAuthenticator
    {
        private readonly FormsAuthenticator _formsAuthenticator;

        public SqlAuthenticator()
        {
            _formsAuthenticator = new FormsAuthenticator();
        }

        public bool AuthenticateAndLogin(string username, string password, bool databaseIssue)
        {
            return _formsAuthenticator.AuthenticateAndLogin(username, password, databaseIssue);
        }

        public void Logout()
        {
            _formsAuthenticator.Logout();
        }
    }
}
