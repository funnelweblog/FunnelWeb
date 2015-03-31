using System.Security.Claims;
using System.Web.Mvc;
using AuthorizationContext = System.Security.Claims.AuthorizationContext;

namespace FunnelWeb.Authentication.Internal
{
	public class CustomAuthorizationManager : ClaimsAuthorizationManager
	{
		private readonly IAuthorizationService authorizationService;

		public CustomAuthorizationManager()
		{
			authorizationService = DependencyResolver.Current.GetService<IAuthorizationService>();
		}

		public override bool CheckAccess(AuthorizationContext authorizationContext)
		{
			return authorizationService.CheckAccess(authorizationContext);
		}
	}
}