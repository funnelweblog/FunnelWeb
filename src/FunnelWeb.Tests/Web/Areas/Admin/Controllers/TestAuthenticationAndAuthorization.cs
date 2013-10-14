using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using FunnelWeb.Authentication.Internal;

namespace FunnelWeb.Tests.Web.Areas.Admin.Controllers
{
	public static class TestAuthenticationAndAuthorization
	{
		public static void SetTestUserToCurrentPrincipal()
		{
			IEnumerable<Claim> claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, "tester"),
				new Claim(ClaimTypes.Name, "Test User"),
				new Claim(ClaimTypes.Role, Authorization.Roles.Admin.Value),
				new Claim(ClaimTypes.Role, Authorization.Roles.Moderator.Value)
			};
			Thread.CurrentPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
		}	}
}