using System;
using System.Web;
using FunnelWeb.Web.Application.Filters;
using NSubstitute;
using NUnit.Framework;
using FunnelWeb.Tests.Helpers;

namespace FunnelWeb.Tests.Web.Application.Filters
{
    public class RequireLoginTests
    {
        public class Already_logged_in_users_will_be_allowed_to_continue : Specification<ActionExecutingContext>
        {
            public override ActionExecutingContext Given()
            {
                var context = Substitute.For<HttpContextBase>();
                context.User.Identity.IsAuthenticated.Returns(true); // Logged in already
                
                return new ActionExecutingContext { HttpContext = context};
            }

            public override void When()
            {
                var filter = new RequireLoginAttribute();
                filter.OnActionExecuting(Subject);
            }

            [Then]
            public void Should_allow_request_to_continue()
            {
                Assert.IsNull(Subject.Result);
            }
        }

        public class Anonymous_users_will_be_redirected_to_login_page : Specification<ActionExecutingContext>
        {
            public override ActionExecutingContext Given()
            {
                var context = Substitute.For<HttpContextBase>();
                context.User.Identity.IsAuthenticated.Returns(false); // Not logged in already

                return new ActionExecutingContext { HttpContext = context };
            }

            public override void When()
            {
                var filter = new RequireLoginAttribute();
                filter.OnActionExecuting(Subject);
            }

            [Then]
            public void Should_allow_request_to_continue()
            {
                Assert.IsAssignableFrom<RedirectResult>(Subject.Result);
            }
        }
    }
}
