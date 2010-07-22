using System.Web.Routing;
using Autofac;
using FunnelWeb.Tests.Helpers;
using FunnelWeb.Web.Application.Routes;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web
{
    public class RoutingTests
    {
        protected RouteCollection Routes { get; private set; }

        public RoutingTests()
        {
            Routes = new RouteCollection();
            var builder = new ContainerBuilder();
            builder.RegisterModule(new RoutesModule(Routes));
            builder.Build();
        }

        [TestFixture]
        public class RoutingRules : RoutingTests
        {
            [Test]
            public void ShouldMapRoutesAsSpecifiedInDocumentation()
            {
                // Wiki
                Routes.WillRoute("~/", new {controller = "Wiki", action = "Recent", pageNumber = "0" });
                Routes.WillRoute("~/hello-world", new { controller = "Wiki", action = "Page", page = "hello-world", revision = "0" });
                Routes.WillRoute("~/hello-world/2", new { controller = "Wiki", action = "Page", page = "hello-world", revision = "2" });
                Routes.WillRoute("~/hello-world/revisions", new { controller = "Wiki", action = "Revisions", page = "hello-world" });
                Routes.WillRoute("~/hello-world/edit", new { controller = "Wiki", action = "Edit", page = "hello-world" });
                
                // Feeds
                Routes.WillRoute("~/feeds", new { controller = "Feed", action = "Feed", feedName = "recent" });
                Routes.WillRoute("~/feeds/foo", new { controller = "Feed", action = "Feed", feedName = "foo" });

                // Login
                Routes.WillRoute("~/login", new { controller = "Login", action = "Index" });
            }
        }
    }
}