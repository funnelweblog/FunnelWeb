using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;

namespace FunnelWeb.Authentication.Internal
{
	public static class PrincipalExtensions
	{
		public static bool IsInRole(this ClaimsPrincipal claimsPrincipal, Claim roleClaim)
		{
			return roleClaim.Type == ClaimTypes.Role && claimsPrincipal.IsInRole(roleClaim.Value);
		}

		public static ClaimsPrincipal AsClaimsPrincipal(this IPrincipal claimsPrincipal)
		{
			return claimsPrincipal as ClaimsPrincipal;
		}

		public static bool TryFindFirstClaim(this IPrincipal principal, Predicate<Claim> match, out Claim claim)
		{
			claim = principal.AsClaimsPrincipal().Claims.FirstOrDefault(c => match(c));
			return claim != null;
		}

		public static bool IsAuthenticated(this IPrincipal principal)
		{
			return principal.Identity.IsAuthenticated;
		}

		public static bool CheckAccess(this IPrincipal principal, Operation operation, Resource resource)
		{
			var authorizationService = DependencyResolver.Current.GetService<IAuthorizationService>();
			return authorizationService.CheckAccess(principal, operation, resource);
		}
	}
}