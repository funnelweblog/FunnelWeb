using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Operation = FunnelWeb.Authentication.Internal.Authorization.Operation;
using Resource = FunnelWeb.Authentication.Internal.Authorization.Resource;
using Roles = FunnelWeb.Authentication.Internal.Authorization.Roles;

namespace FunnelWeb.Authentication.Internal
{
	public class CustomAuthorizationManager : ClaimsAuthorizationManager
	{
		private static AuthorizationSetting[] adminAuthorizations;
		private static AuthorizationSetting[] moderatorAuthorizations;

		public override bool CheckAccess(AuthorizationContext authorizationContext)
		{
			var adminContributions = FilterAuthorizationSettingsForContext(authorizationContext, Roles.Admin, AdminAuthorizations);
			var moderatorContributions = FilterAuthorizationSettingsForContext(authorizationContext, Roles.Moderator, ModeratorAuthorizations);

			return adminContributions.Union(moderatorContributions).Any();
		}

		private static IEnumerable<AuthorizationSetting> FilterAuthorizationSettingsForContext(
			AuthorizationContext authorizationContext,
			Claim role,
			IEnumerable<AuthorizationSetting> authorizationSettings)
		{
			return authorizationContext.Principal.IsInRole(role) ?
						 authorizationSettings.Where(authorizationSetting =>
							 authorizationSetting.Resource.Value == authorizationContext.Resource.First().Value &&
							 authorizationSetting.Operation.Value == authorizationContext.Action.First().Value &&
							 authorizationSetting.Role.Value == role.Value) :
						 Enumerable.Empty<AuthorizationSetting>();
		}

		/// <summary>
		/// Hard coded access rights for the <see cref="Roles.Admin"/> role.
		/// </summary>
		private IEnumerable<AuthorizationSetting> AdminAuthorizations
		{
			get
			{
				return adminAuthorizations ??
					(adminAuthorizations = new[]
					{
						Allow(Roles.Admin, Operation.View, Resource.Admin.Index),
						Allow(Roles.Admin, Operation.View, Resource.Admin.Settings), 
						Allow(Roles.Admin, Operation.Update, Resource.Admin.Settings), 
						Allow(Roles.Admin, Operation.View, Resource.Admin.Comments), 
						Allow(Roles.Admin, Operation.Delete, Resource.Admin.Comment), 
						Allow(Roles.Admin, Operation.Delete, Resource.Admin.AllSpam), 
						Allow(Roles.Admin, Operation.Update, Resource.Admin.Spam), 
						Allow(Roles.Admin, Operation.View, Resource.Admin.Pingbacks), 
						Allow(Roles.Admin, Operation.Delete, Resource.Admin.Pingback), 
						Allow(Roles.Admin, Operation.Update, Resource.Admin.Pingback), 
						Allow(Roles.Admin, Operation.View, Resource.Admin.Tasks), 
						Allow(Roles.Admin, Operation.View, Resource.Admin.Task), 
						Allow(Roles.Admin, Operation.View, Resource.Admin.BlogMl), 
						Allow(Roles.Admin, Operation.Update, Resource.Admin.BlogMl), 
						Allow(Roles.Admin, Operation.View, Resource.Admin.Pages),
						Allow(Roles.Admin, Operation.View, Resource.SqlAuthentications.SqlAuthentication),
						Allow(Roles.Admin, Operation.Update, Resource.SqlAuthentications.SqlAuthentication),
						Allow(Roles.Admin, Operation.View, Resource.SqlAuthentications.NewAccount),
						Allow(Roles.Admin, Operation.Update, Resource.SqlAuthentications.NewAccount),
						Allow(Roles.Admin, Operation.View, Resource.SqlAuthentications.Setup),
						Allow(Roles.Admin, Operation.Update, Resource.SqlAuthentications.Setup),
						Allow(Roles.Admin, Operation.Insert, Resource.SqlAuthentications.AdminAccount),
						Allow(Roles.Admin, Operation.Delete, Resource.SqlAuthentications.RemoveRole),
						Allow(Roles.Admin, Operation.View, Resource.SqlAuthentications.RoleAdd),
						Allow(Roles.Admin, Operation.Update, Resource.SqlAuthentications.RoleAdd),
						Allow(Roles.Admin, Operation.View, Resource.Install.Index),
						Allow(Roles.Admin, Operation.Update, Resource.Install.ChangeProvider),
						Allow(Roles.Admin, Operation.View, Resource.Install.Test),
						Allow(Roles.Admin, Operation.Update, Resource.Install.Upgrade)				
					});
			}
		}

		/// <summary>
		/// Hard coded access rights for the <see cref="Roles.Moderator"/> role.
		/// </summary>
		private IEnumerable<AuthorizationSetting> ModeratorAuthorizations
		{
			get
			{
				return moderatorAuthorizations ??
					(moderatorAuthorizations = new[]
				  {
					  Allow(Roles.Moderator, Operation.View, Resource.Upload.Index),
					  Allow(Roles.Moderator, Operation.Update, Resource.Upload.Index),
					  Allow(Roles.Moderator, Operation.Update, Resource.Upload.CreateDirectory),
					  Allow(Roles.Moderator, Operation.Update, Resource.Upload.Move),
					  Allow(Roles.Moderator, Operation.Delete, Resource.Upload.Delete),
					  Allow(Roles.Moderator, Operation.View, Resource.WikiAdmin.Edit),
					  Allow(Roles.Moderator, Operation.Update, Resource.WikiAdmin.Edit),
					  Allow(Roles.Moderator, Operation.Delete, Resource.WikiAdmin.Page)
				  });
			}
		}

		private static AuthorizationSetting Allow(Claim role, string operation, string resource)
		{
			return new AuthorizationSetting
			{
				Role = role,
				Operation = new Claim(ClaimTypes.Name, operation),
				Resource = new Claim(ClaimTypes.Name, resource)
			};
		}

		private class AuthorizationSetting
		{
			public Claim Role { get; set; }
			public Claim Operation { get; set; }
			public Claim Resource { get; set; }
		}
	}
}