using System.Diagnostics;
using FunnelWeb.Authentication.Internal;
using NUnit.Framework;

namespace FunnelWeb.Tests.Authentication.Internal
{
	[TestFixture]
	public class SqlFunnelWebMembershipTests
	{
		[Test]
		public void HashPassword()
		{
			string hashPassword = SqlFunnelWebMembership.HashPassword("ThisIsMyPassword");
			Debug.WriteLine(hashPassword);
			Assert.AreEqual("30B8BD5829888900D15D2BBE6270D9BC65B0702F", hashPassword);
		}
	}
}