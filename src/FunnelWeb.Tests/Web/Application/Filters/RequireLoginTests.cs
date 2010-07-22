using System.Web;
using System.Web.Mvc;
using FunnelWeb.Web.Application.Filters;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Application.Filters
{
    public class RequireLoginTests
    {
        protected ActionResult MakeRequest(bool loggedIn)
        {
            var context = Substitute.For<HttpContextBase>();
            context.User.Identity.IsAuthenticated.Returns(loggedIn);

            var filter = new RequireLoginAttribute();
            var request = new ActionExecutingContext() {HttpContext = context};
            filter.OnActionExecuting(request);
            return request.Result;
        }

        [TestFixture]
        public class WhenLoggedIn : RequireLoginTests
        {
            [Test]
            public void ShouldContinueWithRequest()
            {
                Assert.IsNull(MakeRequest(true));
            }
        }

        [TestFixture]
        public class WhenNotLoggedIn : RequireLoginTests
        {
            [Test]
            public void ShouldBeAskedToLogin()
            {
                Assert.IsAssignableFrom<RedirectResult>(MakeRequest(false));
            }
        }
    }
}
