using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using FunnelWeb.Authentication.Internal;

namespace FunnelWeb.Tests.Web.Areas.Admin.Controllers
{
	public static class TestAuthenticationAndAuthorization
	{
		public static void SetTestUserToCurrentPrincipal(bool isAuthenticated = true)
		{
			IEnumerable<Claim> claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, "tester"),
				new Claim(ClaimTypes.Name, "Test User"),
				Authorization.Roles.Admin,
				Authorization.Roles.Moderator
			};

			var claimsIdentity = isAuthenticated ? new ClaimsIdentity(claims, "Test") : new NotAuthenticatedClaimsIdentity();
			Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);
		}

		public class NotAuthenticatedClaimsIdentity : ClaimsIdentity
		{
			public override bool IsAuthenticated
			{
				get { return false; }
			}
		}
	}
}