using System;
using System.Security.Claims;

namespace FunnelWeb.Authentication.Internal
{
	/// <summary>
	/// An operation class used with access control.
	/// </summary>
	/// <remarks>Can implicitly cast to <see cref="string"/> and <see cref="Claim"/> (where <see cref="Claim.Type"/> will be <see cref="ClaimTypes.Name"/>).</remarks>
	public struct Operation
	{
		private readonly string operation;

		private Operation(string operation)
		{
			if (string.IsNullOrWhiteSpace(operation)) throw new ArgumentNullException("operation");
			this.operation = operation;
		}

		public override string ToString()
		{
			return operation;
		}

		public static implicit operator string(Operation operation)
		{
			return operation.ToString();
		}

		public static implicit operator Claim(Operation operation)
		{
			return new Claim(ClaimTypes.Name, operation);
		}

		public static implicit operator Operation(string operation)
		{
			return new Operation(operation);
		}
	}
}