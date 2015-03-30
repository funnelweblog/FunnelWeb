using System;

namespace FunnelWeb.Settings
{
	public interface IFederatedAuthenticationConfigurator
	{
		void InitiateFederatedAuthentication(AccessControlServiceSettings accessControlServiceSettings = null);
		Uri GenerateMetadataScript(string returnUrl);
	}
}