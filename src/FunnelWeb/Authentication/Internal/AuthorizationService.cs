using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FunnelWeb.Authentication.Internal
{
	/// <summary>
	/// This class implements mappnigs between database roles and code claims to enable claims based auth in the app.
	/// </summary>
	public class AuthorizationService : IAuthorizationService
	{
		private static AuthorizationSetting[] adminAuthorizations;
		private static AuthorizationSetting[] moderatorAuthorizations;
		private static AuthorizationSetting[] guestAuthorizations;

		public bool CheckAccess(AuthorizationContext authorizationContext)
		{
			var adminContributions = FilterAuthorizationSettingsForContext(authorizationContext, Authorization.Roles.Admin, AdminAuthorizations);
			var moderatorContributions = FilterAuthorizationSettingsForContext(authorizationContext, Authorization.Roles.Moderator, ModeratorAuthorizations);
			var guestContributions = FilterAuthorizationSettingsForContext(authorizationContext, Authorization.Roles.Guest, GuestAuthorizations);

			return adminContributions.Union(moderatorContributions).Union(guestContributions).Any();
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
		/// Hard coded access rights for the <see cref="Authorization.Roles.Admin"/> role.
		/// </summary>
		private IEnumerable<AuthorizationSetting> AdminAuthorizations
		{
			get
			{
				return adminAuthorizations ??
					(adminAuthorizations = new[]
					{
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.Admin.Index),
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.Admin.Settings), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.Update, Authorization.Resources.Admin.Settings), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.Admin.AcsSettings), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.Update, Authorization.Resources.Admin.AcsSettings), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.Admin.Comments), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.Delete, Authorization.Resources.Admin.Comment), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.Delete, Authorization.Resources.Admin.AllSpam), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.Update, Authorization.Resources.Admin.Spam), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.Admin.Pingbacks), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.Delete, Authorization.Resources.Admin.Pingback), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.Update, Authorization.Resources.Admin.Pingback), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.Admin.Tasks), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.Admin.Task), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.Admin.BlogMl), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.Update, Authorization.Resources.Admin.BlogMl), 
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.Admin.Pages),
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.SqlAuthentications.SqlAuthentication),
						Allow(Authorization.Roles.Admin, Authorization.Operations.Update, Authorization.Resources.SqlAuthentications.SqlAuthentication),
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.SqlAuthentications.NewAccount),
						Allow(Authorization.Roles.Admin, Authorization.Operations.Update, Authorization.Resources.SqlAuthentications.NewAccount),
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.SqlAuthentications.Setup),
						Allow(Authorization.Roles.Admin, Authorization.Operations.Update, Authorization.Resources.SqlAuthentications.Setup),
						Allow(Authorization.Roles.Admin, Authorization.Operations.Insert, Authorization.Resources.SqlAuthentications.AdminAccount),
						Allow(Authorization.Roles.Admin, Authorization.Operations.Delete, Authorization.Resources.SqlAuthentications.RemoveRole),
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.SqlAuthentications.RoleAdd),
						Allow(Authorization.Roles.Admin, Authorization.Operations.Update, Authorization.Resources.SqlAuthentications.RoleAdd),
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.Install.Index),
						Allow(Authorization.Roles.Admin, Authorization.Operations.Update, Authorization.Resources.Install.ChangeProvider),
						Allow(Authorization.Roles.Admin, Authorization.Operations.View, Authorization.Resources.Install.Test),
						Allow(Authorization.Roles.Admin, Authorization.Operations.Update, Authorization.Resources.Install.Upgrade)				
					});
			}
		}

		/// <summary>
		/// Hard coded access rights for the <see cref="Authorization.Roles.Moderator"/> role.
		/// </summary>
		private IEnumerable<AuthorizationSetting> ModeratorAuthorizations
		{
			get
			{
				return moderatorAuthorizations ??
					(moderatorAuthorizations = new[]
				  {
					  Allow(Authorization.Roles.Moderator, Authorization.Operations.View, Authorization.Resources.Upload.Index),
					  Allow(Authorization.Roles.Moderator, Authorization.Operations.Update, Authorization.Resources.Upload.Index),
					  Allow(Authorization.Roles.Moderator, Authorization.Operations.Update, Authorization.Resources.Upload.CreateDirectory),
					  Allow(Authorization.Roles.Moderator, Authorization.Operations.Update, Authorization.Resources.Upload.Move),
					  Allow(Authorization.Roles.Moderator, Authorization.Operations.Delete, Authorization.Resources.Upload.Delete),
					  Allow(Authorization.Roles.Moderator, Authorization.Operations.View, Authorization.Resources.WikiAdmin.Edit),
					  Allow(Authorization.Roles.Moderator, Authorization.Operations.Update, Authorization.Resources.WikiAdmin.Edit),
					  Allow(Authorization.Roles.Moderator, Authorization.Operations.Delete, Authorization.Resources.WikiAdmin.Page)
				  });
			}
		}

		/// <summary>
		/// Hard coded access rights for the <see cref="Authorization.Roles.Guest"/> role.
		/// </summary>
		private IEnumerable<AuthorizationSetting> GuestAuthorizations
		{
			get
			{
				return guestAuthorizations ??
					(guestAuthorizations = new[]
				  {
					  Allow(Authorization.Roles.Guest, Authorization.Operations.View, Authorization.Resources.Blog.Comment),
					  Allow(Authorization.Roles.Guest, Authorization.Operations.Insert, Authorization.Resources.Blog.Comment)
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