using System.Security.Claims;
using System.Threading;

namespace FunnelWeb.Authentication.Internal
{
	public static class ClaimsSessionHelper
	{
		public static ClaimsPrincipal CurrentPrincipal { get { return (ClaimsPrincipal)Thread.CurrentPrincipal; } }

		public static string GetUserName(this ClaimsPrincipal principal)
		{
			return principal.FindFirst(ClaimTypes.NameIdentifier).Value;
		}
	}
}