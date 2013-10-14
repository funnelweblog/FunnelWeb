using System;
using System.Collections.Generic;
using FunnelWeb.Model.Authentication;
using FunnelWeb.Settings;

namespace FunnelWeb.Authentication.Internal
{
	public class FormsAuthenticator : IAuthenticator
	{
		private readonly Func<ISettingsProvider> settingsProvider;
		private readonly IConfigSettings configSettings;

		public FormsAuthenticator(Func<ISettingsProvider> settingsProvider, IConfigSettings configSettings)
		{
			this.settingsProvider = settingsProvider;
			this.configSettings = configSettings;
		}

		public string GetName()
		{
			return settingsProvider().GetSettings<FunnelWebSettings>().Author;
		}

		public bool AuthenticateAndLogin(string username, string password)
		{
			var requiredUsername = configSettings.Get("funnelweb.configuration.authentication.username");
			var requiredPassword = configSettings.Get("funnelweb.configuration.authentication.password");

			if (username == requiredUsername && password == requiredPassword)
			{
				ClaimsSessionHelper.Login(new User
				{
					Username = username,
					Name = username,
					Roles = new List<Role>
					{
						// Hard coded two roles for non-db login.
						new Role { Name = Authorization.Roles.Admin.Value },
						new Role { Name = Authorization.Roles.Moderator.Value }
					}
				});

				return true;
			}

			return false;
		}

		public void Logout()
		{
			ClaimsSessionHelper.Logout();
		}
	}
}