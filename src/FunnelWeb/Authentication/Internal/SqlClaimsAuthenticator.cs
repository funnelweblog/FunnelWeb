using System;
using System.Web.Mvc;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Model.Authentication;
using FunnelWeb.Settings;
using NHibernate;

namespace FunnelWeb.Authentication.Internal
{
	public class SqlClaimsAuthenticator : IAuthenticator
	{
		private readonly FormsAuthenticator formsAuthenticator;
		private readonly Func<IDatabaseUpgradeDetector> upgradeDetector;
		private readonly Func<ISettingsProvider> settingsProvider;
		private readonly Func<ISession> sessionFactory;
		private readonly IFederatedAuthenticationService federatedAuthenticationService;

		public SqlClaimsAuthenticator(
				FormsAuthenticator formsAuthenticator,
				Func<IDatabaseUpgradeDetector> upgradeDetector,
				Func<ISettingsProvider> settingsProvider,
				Func<ISession> sessionFactory,
				IFederatedAuthenticationService federatedAuthenticationService)
		{
			this.formsAuthenticator = formsAuthenticator;
			this.upgradeDetector = upgradeDetector;
			this.settingsProvider = settingsProvider;
			this.sessionFactory = sessionFactory;
			this.federatedAuthenticationService = federatedAuthenticationService;
		}

		public string GetName()
		{
			return UseFormsAuthentication
								 ? formsAuthenticator.GetName()
								 : SqlGetName();
		}

		private static string SqlGetName()
		{
			var username = ClaimsSessionHelper.CurrentPrincipal.GetUserName();

			var session = DependencyResolver.Current.GetService<ISession>();
			var user = session.QueryOver<User>()
					.Where(u => u.Username == username)
					.SingleOrDefault();

			return user.Name;
		}

		public bool AuthenticateAndLogin(string username, string password)
		{
			return UseFormsAuthentication ?
					formsAuthenticator.AuthenticateAndLogin(username, password) :
					SqlAuthenticateAndLogin(username, password);
		}

		private bool UseFormsAuthentication
		{
			get
			{
				return upgradeDetector().UpdateNeeded() ||
						!settingsProvider().GetSettings<SqlAuthSettings>().SqlAuthenticationEnabled;
			}
		}

		private bool SqlAuthenticateAndLogin(string username, string password)
		{
			var user = sessionFactory()
				.QueryOver<User>()
				.Where(u => u.Username == username && u.Password == SqlFunnelWebMembership.HashPassword(password))
				.SingleOrDefault();

			if (user == null)
			{
				return false;
			}

			federatedAuthenticationService.Login(user);

			return true;
		}

		public void Logout()
		{
			federatedAuthenticationService.Logout();
		}
	}
}