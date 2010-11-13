using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Web.Features.Admin;
using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Controllers
{
    [TestFixture]
    public class AdminControllerTests
    {
        protected AdminController Controller { get; set; }
        protected IAdminRepository AdminRepository { get; set; }
        protected IFeedRepository FeedRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            Controller = new AdminController();
            Controller.AdminRepository = AdminRepository = Substitute.For<IAdminRepository>();
            Controller.FeedRepository = FeedRepository = Substitute.For<IFeedRepository>();
            Controller.ControllerContext = CreateControllerContext();
        }

        [Test]
        public void Index()
        {
            var result = (ViewResult)Controller.Index();
            
            Assert.That(result.ViewName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CreateFeed()
        {
            var result = (RedirectToRouteResult)Controller.CreateFeed(Arg.Any<string>(), Arg.Any<string>());

            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void DeleteFeed()
        {
            FeedRepository.GetFeeds().Returns(new List<Feed>().AsQueryable());
            
            var result = (RedirectToRouteResult)Controller.DeleteFeed(0);

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void DeleteRedirect()
        {
            AdminRepository.GetRedirects().Returns(new List<Redirect>().AsQueryable());

            var result = (RedirectToRouteResult)Controller.DeleteRedirect(0);

            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void CreateRedirect()
        {
            var result = (RedirectToRouteResult)Controller.CreateRedirect(Arg.Any<string>(), Arg.Any<string>());

            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void DeleteComment()
        {
            var result = (RedirectToRouteResult)Controller.DeleteComment(0);

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void DeleteAllSpam()
        {
            var result = (RedirectToRouteResult)Controller.DeleteAllSpam();

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void DeletePingback()
        {
            var result = (RedirectToRouteResult)Controller.DeletePingback(0);

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void ToggleSpam()
        {
            var result = (RedirectToRouteResult)Controller.ToggleSpam(0);

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void ToggePingbackSpam()
        {
            var result = (RedirectToRouteResult)Controller.TogglePingbackSpam(0);

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void CreateFeedIsSaved()
        {
            Controller.CreateFeed("name", "title");

            FeedRepository.Received().Save(Arg.Is<Feed>(x => x.Name == "name"));
        }

        private static ControllerContext CreateControllerContext()
        {
            var controllerContext = new ControllerContext();
            var httpContext = Substitute.For<HttpContextBase>();
            var httpServer = Substitute.For<HttpServerUtilityBase>();
            httpServer.MapPath(Arg.Any<string>()).Returns(@"C:\Windows");
            httpContext.Server.Returns(httpServer);
            controllerContext.HttpContext = httpContext;
            return controllerContext;
        }
    }
}
