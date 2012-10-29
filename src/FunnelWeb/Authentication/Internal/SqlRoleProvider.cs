using System;
using System.Linq;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Model.Authentication;
using FunnelWeb.Settings;
using NHibernate;

namespace FunnelWeb.Authentication.Internal
{
    public class SqlRoleProvider : IRoleProvider
    {
        private readonly Func<IDatabaseUpgradeDetector> upgradeDetector;
        private readonly Func<ISettingsProvider> settingsProvider;
        private readonly Func<ISession> sessionCallback;
        private readonly FormsRoleProvider formsRoleProvider;

        public SqlRoleProvider(Func<IDatabaseUpgradeDetector> upgradeDetector, Func<ISettingsProvider> settingsProvider, Func<ISession> sessionCallback)
        {
            this.upgradeDetector = upgradeDetector;
            this.settingsProvider = settingsProvider;
            this.sessionCallback = sessionCallback;
            formsRoleProvider = new FormsRoleProvider();
        }

        private bool UseFormsRoleProvider
        {
            get
            {
                return upgradeDetector().UpdateNeeded() ||
                    !settingsProvider()
                    .GetSettings<SqlAuthSettings>().SqlAuthenticationEnabled;
            }
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return UseFormsRoleProvider
                ? formsRoleProvider.IsUserInRole(username, roleName)
                : CheckIsUserInRole(username, roleName);
        }

        private bool CheckIsUserInRole(string username, string roleName)
        {
            var session = sessionCallback();

            var checkIsUserInRole = session
                .QueryOver<User>()
                .Where(u => u.Username == username)
                .JoinQueryOver<Role>(r => r.Roles)
                .Where(r => r.Name == roleName)
                .RowCount() == 1;

            return checkIsUserInRole;
        }

        public string[] GetRolesForUser(string username)
        {
            var updateNeeded = UseFormsRoleProvider;

            return updateNeeded
                ? formsRoleProvider.GetRolesForUser(username)
                : FetchRolesForUser(username);
        }

        private string[] FetchRolesForUser(string username)
        {
            var session = sessionCallback();

            var roles = session
                .QueryOver<Role>()
                .JoinQueryOver<User>(u => u.Users)
                .Where(u => u.Username == username)
                .Select(r => r.Name)
                .List<string>();

            return roles.ToArray();
        }

        public void AddUserToRoles(User user, params string[] rolesToAddTo)
        {
            var session = sessionCallback();

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
