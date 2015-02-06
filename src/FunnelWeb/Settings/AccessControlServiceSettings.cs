using System.ComponentModel;

namespace FunnelWeb.Settings
{
	public class AccessControlServiceSettings : ISettings
	{
		[DisplayName("External Access Control Enabled")]
		[DefaultValue(false)]
		[Description("True if external users are allowed to authorize using Windows Azure Access Control Service.")]
		[SettingStorage(StorageLocation.Database, "acs-authentication")]
		public bool Enabled { get; set; }

		[DisplayName("Authenticated commenters")]
		[DefaultValue(false)]
		[Description("True if comment form is shown only for authenticated users.")]
		[SettingStorage(StorageLocation.Database, "acs-authenticatedcommenters")]
		public bool RequireAuthenticationForComments { get; set; }

		[DisplayName("Namespace")]
		[DefaultValue("{acsNamespace}")]
		[Description("The namespace of your ACS tennant.")]
		[SettingStorage(StorageLocation.Database, "acs-namespace")]
		public string Namespace { get; set; }

		[DisplayName("Realm")]
		[DefaultValue("{realm}")]
		[Description("The realm of your Relying Party.")]
		[SettingStorage(StorageLocation.Database, "acs-realm")]
		public string Realm { get; set; }

		[DisplayName("Issuer Thumbprint")]
		[DefaultValue("{IssuerThumbprint}")]
		[Description("The thumbprint of the ACS Relying Party.")]
		[SettingStorage(StorageLocation.Database, "acs-issuerthumbprint")]
		public string IssuerThumbprint { get; set; }

		[DisplayName("Audience Uris")]
		[DefaultValue("{AudienceUris}")]
		[Description("The Audience Uris of the ACS Relying Party.")]
		[SettingStorage(StorageLocation.Database, "acs-audienceuris")]
		public string AudienceUris { get; set; }
	}
}