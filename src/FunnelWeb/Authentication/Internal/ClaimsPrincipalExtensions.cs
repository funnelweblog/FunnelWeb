using System.Security.Claims;

namespace FunnelWeb.Authentication.Internal
{
	public static class ClaimsPrincipalExtensions
	{
		public static bool IsInRole(this ClaimsPrincipal claimsPrincipal, Claim roleClaim)
		{
			return roleClaim.Type == ClaimTypes.Role && claimsPrincipal.IsInRole(roleClaim.Value);
		}
	}
}