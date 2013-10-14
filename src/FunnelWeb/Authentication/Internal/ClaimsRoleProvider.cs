using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Model.Authentication;
using FunnelWeb.Settings;
using NHibernate;

namespace FunnelWeb.Authentication.Internal
{
	public class ClaimsRoleProvider : IRoleProvider
	{
		private readonly Func<IDatabaseUpgradeDetector> upgradeDetector;
		private readonly Func<ISettingsProvider> settingsProvider;
		private readonly Func<ISession> sessionCallback;
		private readonly FormsRoleProvider formsRoleProvider;

		public ClaimsRoleProvider(Func<IDatabaseUpgradeDetector> upgradeDetector, Func<ISettingsProvider> settingsProvider, Func<ISession> sessionCallback)
		{
			this.upgradeDetector = upgradeDetector;
			this.settingsProvider = settingsProvider;
			this.sessionCallback = sessionCallback;
			formsRoleProvider = new FormsRoleProvider();
		}

		private bool UseFormsRoleProvider
		{
			get { return upgradeDetector().UpdateNeeded() || !settingsProvider().GetSettings<SqlAuthSettings>().SqlAuthenticationEnabled; }
		}

		public bool IsUserInRole(string username, string roleName)
		{
			return UseFormsRoleProvider
					? formsRoleProvider.IsUserInRole(username, roleName)
					: ClaimsPrincipal.IsInRole(roleName);
		}

		public string[] GetRolesForUser(string username)
		{
			var updateNeeded = UseFormsRoleProvider;

			return updateNeeded
					? formsRoleProvider.GetRolesForUser(username)
					: FetchRolesForUser();
		}

		public void AddUserToRoles(User user, params Claim[] rolesToAdd)
		{
			var session = sessionCallback();

			foreach (var roleToAddTo in rolesToAdd)
			{
				Claim roleClaim = roleToAddTo;
				var role = session.QueryOver<Role>().Where(r => r.Name == roleClaim.Value).SingleOrDefault();
				role.Users.Add(user);
				user.Roles.Add(role);
				session.SaveOrUpdate(role);
			}
		}

		private static ClaimsPrincipal ClaimsPrincipal
		{
			get { return ((ClaimsPrincipal)Thread.CurrentPrincipal); }
		}

		private string[] FetchRolesForUser()
		{
			return ClaimsPrincipal.FindAll(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
		}
	}
}
