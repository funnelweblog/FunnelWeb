using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Extensions.SqlAuthentication.Model;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application;
using FunnelWeb.Web.Application.Authentication;
using NHibernate;
using NHibernate.Linq;

namespace FunnelWeb.Extensions.SqlAuthentication
{
    public class SqlRoleProvider : IRoleProvider
    {
        private readonly FormsRoleProvider _formsRoleProvider;

        public SqlRoleProvider()
        {
            _formsRoleProvider = new FormsRoleProvider();
        }

        private static bool UseFormsRoleProvider
        {
            get
            {
                return DependencyResolver.Current.GetService<IDatabaseUpgradeDetector>().UpdateNeeded() ||
                    !DependencyResolver.Current.GetService<ISettingsProvider>()
                    .GetSettings<SqlAuthSettings>().SqlAuthenticationEnabled;
            }
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return UseFormsRoleProvider
                ? _formsRoleProvider.IsUserInRole(username, roleName) 
                : CheckIsUserInRole(username, roleName);
        }

        private static bool CheckIsUserInRole(string username, string roleName)
        {
            var session = DependencyResolver.Current.GetService<ISession>();
            var checkIsUserInRole = session
                                        .Linq<User>()
                                        .Expand("Roles")
                                        .Where(u => u.Username == username && u.Roles.Any(r => r.Name == roleName))
                                        .Count() == 1;
            return checkIsUserInRole;
        }

        public string[] GetRolesForUser(string username)
        {
            var updateNeeded = UseFormsRoleProvider;
            return updateNeeded
                       ? _formsRoleProvider.GetRolesForUser(username)
                       : FetchRolesForUser(username);
        }

        private static string[] FetchRolesForUser(string username)
        {
            var session = DependencyResolver.Current.GetService<ISession>();
            var fetchRolesForUser = session
                .Linq<User>()
                .Expand("Roles")
                .Where(u => u.Username == username)
                .SelectMany(u => u.Roles)
                .ToList()
                .Select(r=>r.Name)
                .ToArray();
            return fetchRolesForUser;
        }

        public void AddUserToRoles(User user, params string[] rolesToAddTo)
        {
            var session = DependencyResolver.Current.GetService<ISession>();
            foreach (var roleToAddTo in rolesToAddTo)
            {
                var role = session.Linq<Role>().Where(r => r.Name == roleToAddTo).SingleOrDefault();
                role.Users.Add(user);
                user.Roles.Add(role);
                session.SaveOrUpdate(role);
            }
        }
    }
}
