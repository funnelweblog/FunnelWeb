using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Authentication;
using FunnelWeb.Extensions.SqlAuthentication.Model;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application;
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
                                        .QueryOver<User>()
                                        .Where(u=>u.Username == username)
                                        .JoinQueryOver<Role>(r=>r.Roles)
                                        .Where(r => r.Name == roleName)
                                        .RowCount() == 1;
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
            var roles = session
                .QueryOver<Role>()
                .JoinQueryOver<User>(u=>u.Users)
                .Where(u => u.Username == username)
                .Select(r=>r.Name)
                .List<string>();
            return roles.ToArray();
        }

        public void AddUserToRoles(User user, params string[] rolesToAddTo)
        {
            var session = DependencyResolver.Current.GetService<ISession>();
            foreach (var roleToAddTo in rolesToAddTo)
            {
                var role = session.QueryOver<Role>().Where(r => r.Name == roleToAddTo).SingleOrDefault();
                role.Users.Add(user);
                user.Roles.Add(role);
                session.SaveOrUpdate(role);
            }
        }
    }
}
