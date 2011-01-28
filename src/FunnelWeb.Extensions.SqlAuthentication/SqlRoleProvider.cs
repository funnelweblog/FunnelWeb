using System.Linq;
using System.Web;
using FunnelWeb.Web.Application.Authentication;

namespace FunnelWeb.Extensions.SqlAuthentication
{
    public class SqlRoleProvider : IRoleProvider
    {
        private readonly FormsRoleProvider _formsRoleProvider;

        public SqlRoleProvider()
        {
            _formsRoleProvider = new FormsRoleProvider();
        }

        private static bool DatabaseError
        {
            get
            {
                return HttpContext.Current.Request.QueryString.AllKeys.Contains("databaseIssue")
                           ? bool.Parse(HttpContext.Current.Request.QueryString["databaseIssue"])
                           : false;
            }
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return DatabaseError ? _formsRoleProvider.IsUserInRole(username, roleName) : true;
        }

        public string[] GetRolesForUser(string username)
        {
            return DatabaseError ? _formsRoleProvider.GetRolesForUser(username) : new[]{"Admin", "Moderator"};
        }
    }
}
