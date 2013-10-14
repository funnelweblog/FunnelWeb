﻿using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using FunnelWeb.Model.Authentication;

namespace FunnelWeb.Authentication.Internal
{
	public class ClaimsSessionHelper
	{
		public static void Login(User user)
		{
			var claims = new List<Claim>(new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Username),
				new Claim(ClaimTypes.Name, user.Name)
			});

			claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

			var identity = new ClaimsIdentity(claims, "Forms");
			var principal = new ClaimsPrincipal(identity);

			var sessionToken = new SessionSecurityToken(principal);
			var authenticationModule = FederatedAuthentication.SessionAuthenticationModule;
			authenticationModule.WriteSessionTokenToCookie(sessionToken);
		}

		public static void Logout()
		{
			FederatedAuthentication.SessionAuthenticationModule.SignOut();
		}
	}
}