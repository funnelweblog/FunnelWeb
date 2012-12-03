using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FunnelWeb.Tests.Helpers;
using FunnelWeb.Web.App_Start;
using FunnelWeb.Web.Areas.Admin;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web
{
    public class RoutingTests
    {
        protected RouteCollection Routes { get; private set; }

        public RoutingTests()
        {
            Routes = new RouteCollection();

            var httpContextBase = Substitute.For<HttpContextBase>();
            httpContextBase.Server.MapPath(Arg.Any<string>()).Returns(@"c:\");
            var adminAreaRegistration = new AdminAreaRegistration(new Lazy<HttpContextBase>(()=>httpContextBase));
            var areaRegistrationContext = new AreaRegistrationContext(adminAreaRegistration.AreaName, Routes);
            adminAreaRegistration.RegisterArea(areaRegistrationContext);

            RouteConfig.RegisterRoutes(Routes);
        }

        [TestFixture]
        public class RoutingRules : RoutingTests
        {
            [Test]
            public void ShouldMapRoutesAsSpecifiedInDocumentation()
            {
                // Wiki
                Routes.WillRoute("~/", new {controller = "Wiki", action = "Home", pageNumber = "0" });
                Routes.WillRoute("~/blog", new {controller = "Wiki", action = "Recent", pageNumber = "0" });
                Routes.WillRoute("~/hello-world", new { controller = "Wiki", action = "Page", page = "hello-world", revision = (int?)null });
                Routes.WillRoute("~/history-of/hello-world", new { controller = "Wiki", action = "Revisions", page = "hello-world" });
                Routes.WillRoute("~/trackbacks-for/hello-world", new { controller = "Wiki", action = "Pingbacks", page = "hello-world" });
                Routes.WillRoute("~/edit/hello-world", new { controller = "WikiAdmin", action = "Edit", page = "hello-world" });
                
                // Feeds
                Routes.WillRoute("~/feeds", new { controller = "Feed", action = "Feed", feedName = (string)null });
                Routes.WillRoute("~/feeds/foo", new { controller = "Feed", action = "Feed", feedName = "foo" });

                // Login
                Routes.WillRoute("~/admin/login", new { controller = "Login", action = "Login" });
            }
        }
    }
}