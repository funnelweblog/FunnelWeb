using System.Web;
using FunnelWeb.Web.Controllers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Controllers
{
	[TestFixture]
	public class LoginControllerTests
	{
		[Test]
		public void Parse_ReturnUrl()
		{
			var expectedReturnUrl = "http://foo.com";

			string contextQuery = HttpUtility.UrlEncode(string.Format("foo=bar&ReturnUrl={0}&baz=cux", expectedReturnUrl));

			var returnUrl = LoginController.GetReturnUrl(contextQuery);

			Assert.AreEqual(expectedReturnUrl, returnUrl);
		}
	}
}