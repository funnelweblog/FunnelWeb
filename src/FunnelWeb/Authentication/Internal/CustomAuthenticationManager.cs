using System.Security.Claims;

namespace FunnelWeb.Authentication.Internal
{
	public class CustomAuthenticationManager : ClaimsAuthenticationManager
	{
		public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
		{
			var claimsIdentity = incomingPrincipal.Identity as ClaimsIdentity;
			if (claimsIdentity != null)
			{
				// All authenticated users are always at least "Guest" in the system.
				claimsIdentity.AddClaim(Authorization.Roles.Guest);
			}

			return base.Authenticate(resourceName, incomingPrincipal);
		}
	}
}