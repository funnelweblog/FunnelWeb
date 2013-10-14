﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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

		public SqlClaimsAuthenticator(
				FormsAuthenticator formsAuthenticator,
				Func<IDatabaseUpgradeDetector> upgradeDetector,
				Func<ISettingsProvider> settingsProvider,
				Func<ISession> sessionFactory)
		{
			this.formsAuthenticator = formsAuthenticator;
			this.upgradeDetector = upgradeDetector;
			this.settingsProvider = settingsProvider;
			this.sessionFactory = sessionFactory;
		}

		public string GetName()
		{
			return UseFormsAuthentication
								 ? formsAuthenticator.GetName()
								 : SqlGetName();
		}

		private static string SqlGetName()
		{
			var username = ((FormsIdentity)HttpContext.Current.User.Identity).Name;

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

			ClaimsSessionHelper.Login(user);

			return true;
		}

		public void Logout()
		{
			ClaimsSessionHelper.Logout();
		}
	}
}