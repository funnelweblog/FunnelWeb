using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace FunnelWeb.Authentication.Internal
{
	/// <summary>
	/// Service for authorizing users to do things.
	/// </summary>
	public interface IAuthorizationService
	{
		bool CheckAccess(AuthorizationContext authorizationContext);
	}

	public static class AuthorizationServiceExtensions
	{
		public static bool CheckAccess(this IAuthorizationService authorizationService, IPrincipal principal, Operation operation, Resource resource)
		{
			var claimsPrincipal = principal.AsClaimsPrincipal();
			var resources = new Collection<Claim>(new Claim[] { resource }.ToList());
			var operations = new Collection<Claim>(new Claim[] { operation }.ToList());

			var authorizationContext = new AuthorizationContext(claimsPrincipal, resources, operations);

			return authorizationService.CheckAccess(authorizationContext);
		}
	}
}