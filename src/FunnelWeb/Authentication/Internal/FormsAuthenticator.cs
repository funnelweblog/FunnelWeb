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
		private readonly IFederatedAuthenticationService federatedAuthenticationService;

		public FormsAuthenticator(Func<ISettingsProvider> settingsProvider, IConfigSettings configSettings, IFederatedAuthenticationService federatedAuthenticationService)
		{
			this.settingsProvider = settingsProvider;
			this.configSettings = configSettings;
			this.federatedAuthenticationService = federatedAuthenticationService;
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
				federatedAuthenticationService.Login(new User
				{
					Username = username,
					Name = username,
					Roles = new List<Model.Authentication.Role>
					{
						// Hard coded roles for non-db login.
						new Model.Authentication.Role { Name = Authorization.Roles.Admin },
						new Model.Authentication.Role { Name = Authorization.Roles.Moderator },
						new Model.Authentication.Role { Name = Authorization.Roles.Guest }
					}
				});

				return true;
			}

			return false;
		}

		public void Logout()
		{
			federatedAuthenticationService.Logout();
		}
	}
}