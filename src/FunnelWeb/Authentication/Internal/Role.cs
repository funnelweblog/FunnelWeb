using System;
using System.Security.Claims;

namespace FunnelWeb.Authentication.Internal
{
	/// <summary>
	/// An role class used with access control.
	/// </summary>
	/// <remarks>Can implicitly cast to <see cref="string"/> and <see cref="Claim"/> (where <see cref="Claim.Type"/> will be <see cref="ClaimTypes.Role"/>).</remarks>
	public class Role
	{
		private readonly string role;

		private Role(string role)
		{
			if (string.IsNullOrWhiteSpace(role)) throw new ArgumentNullException("role");
			this.role = role;
		}

		public override string ToString()
		{
			return role;
		}

		public static implicit operator string(Role role)
		{
			return role.ToString();
		}

		public static implicit operator Claim(Role role)
		{
			return new Claim(ClaimTypes.Role, role);
		}

		public static implicit operator Role(string role)
		{
			return new Role(role);
		}
	}
}