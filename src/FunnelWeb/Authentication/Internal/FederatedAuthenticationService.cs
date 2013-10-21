using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using FunnelWeb.Model.Authentication;

namespace FunnelWeb.Authentication.Internal
{
	public class FederatedAuthenticationService : IFederatedAuthenticationService
	{
		public void Login(User user)
		{
			var claims = new List<Claim>(new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Username),
				new Claim(ClaimTypes.Name, user.Name)
			});

			if (!string.IsNullOrWhiteSpace(user.Email))
			{
				claims.Add(new Claim(ClaimTypes.Email, user.Email));
			}

			claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

			var identity = new ClaimsIdentity(claims, "Forms");
			var principal = new ClaimsPrincipal(identity);

			var sessionToken = new SessionSecurityToken(principal);
			var authenticationModule = FederatedAuthentication.SessionAuthenticationModule;
		
			// Persist the authentication cookie.
			authenticationModule.WriteSessionTokenToCookie(sessionToken);

			// Set the current user for the current request
			Thread.CurrentPrincipal = principal;
		}

		public void Logout()
		{
			FederatedAuthentication.SessionAuthenticationModule.SignOut();
		}
	}
}