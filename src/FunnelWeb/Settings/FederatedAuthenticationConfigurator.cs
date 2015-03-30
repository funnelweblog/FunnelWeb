using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using FunnelWeb.DatabaseDeployer;

namespace FunnelWeb.Settings
{
	public class FederatedAuthenticationConfigurator : IFederatedAuthenticationConfigurator
	{
		private readonly Func<ISettingsProvider> settingsProviderFactory;
		private readonly Func<IDatabaseUpgradeDetector> databaseUpgradeDetectorFactory;
		private ISettingsProvider settingsProvider;
		private ISettingsProvider SettingsProvider { get { return settingsProvider ?? (settingsProvider = settingsProviderFactory()); } }

		public FederatedAuthenticationConfigurator(
			Func<ISettingsProvider> settingsProviderFactory,
			Func<IDatabaseUpgradeDetector> databaseUpgradeDetectorFactory)
		{
			this.settingsProviderFactory = settingsProviderFactory;
			this.databaseUpgradeDetectorFactory = databaseUpgradeDetectorFactory;
		}

		public void InitiateFederatedAuthentication(AccessControlServiceSettings accessControlServiceSettings = null)
		{
			if (accessControlServiceSettings == null)
			{
				if (!databaseUpgradeDetectorFactory().UpdateNeeded())
				{
					// Database needs an upgrade or is not reachable. We cannot configure Fed Auth at this time.
					return;
				}

				if (!SettingsProvider.TryGetSettings(out accessControlServiceSettings))
				{
					// Unable to load the settings from the databse. We cannot configure Fed Auth at this time.
					return;
				}
			}

			string realm = accessControlServiceSettings.Realm;
			string acsNamespace = accessControlServiceSettings.Namespace;
			string thumbprint = accessControlServiceSettings.IssuerThumbprint;
			IEnumerable<Uri> audienceUris = accessControlServiceSettings
				.AudienceUris
				.Split(Constants.Chars.NewLine, Constants.Chars.Space)
				.Where(a => { Uri uri; return Uri.TryCreate(a, UriKind.Absolute, out uri); })
				.Select(a => new Uri(a));

			var defaultSettings = SettingsProvider.GetDefaultSettings<AccessControlServiceSettings>();
			if (!accessControlServiceSettings.Enabled ||
					realm == defaultSettings.Realm || acsNamespace == defaultSettings.Namespace || thumbprint == defaultSettings.IssuerThumbprint)
			{
				return;
			}

			// system.identityModel -> identityConfiguration
			IdentityConfiguration identityConfiguration = FederatedAuthentication.FederationConfiguration.IdentityConfiguration;
			identityConfiguration.AudienceRestriction.AllowedAudienceUris.Clear();
			foreach (var audienceUri in audienceUris)
			{
				identityConfiguration.AudienceRestriction.AllowedAudienceUris.Add(audienceUri);
			}

			var validatingIssuerNameRegistry = identityConfiguration.IssuerNameRegistry as ValidatingIssuerNameRegistry;
			if (validatingIssuerNameRegistry != null)
			{
				string acsAddress = string.Format("https://{0}.accesscontrol.windows.net/", acsNamespace);
				var authority = new IssuingAuthority(acsAddress);
				authority.Issuers.Add(acsAddress);
				authority.Thumbprints.Add(thumbprint);

				validatingIssuerNameRegistry.IssuingAuthorities = new[] { authority };
			}

			// system.identityModel.services -> federationConfiguration -> wsFederation
			string issuer = string.Format("https://{0}.accesscontrol.windows.net/v2/wsfederation", acsNamespace);
			FederatedAuthentication.FederationConfiguration.WsFederationConfiguration.Issuer = issuer;
			FederatedAuthentication.FederationConfiguration.WsFederationConfiguration.Realm = realm;
		}

		public Uri GenerateMetadataScript(string returnUrl)
		{
			var accessControlServiceSettings = SettingsProvider.GetSettings<AccessControlServiceSettings>();
			var acsNamespace = accessControlServiceSettings.Namespace;
			var realm = accessControlServiceSettings.Realm;
			var context = string.IsNullOrWhiteSpace(returnUrl) ?
				string.Empty :
				string.Format("&context={0}", HttpUtility.UrlEncode("ReturnUrl=" + returnUrl));

			var metaDataScript = string.Format("https://{0}.accesscontrol.windows.net/v2/metadata/identityProviders.js?protocol=wsfederation&realm={1}&version=1.0&callback=ShowSigninPage{2}", acsNamespace, realm, context);

			return new Uri(metaDataScript);
		}
	}
}