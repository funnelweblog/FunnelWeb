using System;
using System.Security.Claims;

namespace FunnelWeb.Authentication.Internal
{
	/// <summary>
	/// A resourece class used with access control.
	/// </summary>
	/// <remarks>Can implicitly cast to <see cref="string"/> and <see cref="Claim"/> (where <see cref="Claim.Type"/> will be <see cref="ClaimTypes.Name"/>).</remarks>
	public class Resource
	{
		private readonly string resource;

		private Resource(string resource)
		{
			if (string.IsNullOrWhiteSpace(resource)) throw new ArgumentNullException("resource");
			this.resource = resource;
		}

		public override string ToString()
		{
			return resource;
		}

		public static implicit operator string(Resource resource)
		{
			return resource.ToString();
		}

		public static implicit operator Claim(Resource resource)
		{
			return new Claim(ClaimTypes.Name, resource);
		}

		public static implicit operator Resource(string operation)
		{
			return new Resource(operation);
		}
	}
}