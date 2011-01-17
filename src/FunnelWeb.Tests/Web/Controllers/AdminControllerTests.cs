using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Web.Controllers;
using FunnelWeb.Web.Views.Admin;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Controllers
{
    [TestFixture]
    public class AdminControllerTests
    {
        protected AdminController Controller { get; set; }
        protected IAdminRepository AdminRepository { get; set; }
        protected ITagRepository FeedRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            Controller = new AdminController();
            Controller.AdminRepository = AdminRepository = Substitute.For<IAdminRepository>();
            Controller.FeedRepository = FeedRepository = Substitute.For<ITagRepository>();
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
            var result = (RedirectToRouteResult)Controller.Feeds(new FeedsModel());

            Assert.That(result.RouteValues["Action"], Is.EqualTo("Feeds"));
        }

        [Test]
        public void DeleteFeed()
        {
            FeedRepository.GetTags().Returns(new List<Tag>().AsQueryable());
            
            var result = (RedirectToRouteResult)Controller.DeleteFeed(0);

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Feeds"));
        }

        [Test]
        public void DeleteComment()
        {
            var result = (RedirectToRouteResult)Controller.DeleteComment(0);

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Comments"));
        }

        [Test]
        public void DeleteAllSpam()
        {
            var result = (RedirectToRouteResult)Controller.DeleteAllSpam();

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Comments"));
        }

        [Test]
        public void DeletePingback()
        {
            var result = (RedirectToRouteResult)Controller.DeletePingback(0);

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Pingbacks"));
        }

        [Test]
        public void ToggleSpam()
        {
            var result = (RedirectToRouteResult)Controller.ToggleSpam(0);

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Comments"));
        }

        [Test]
        public void ToggePingbackSpam()
        {
            var result = (RedirectToRouteResult)Controller.TogglePingbackSpam(0);

            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Pingbacks"));
        }

        [Test]
        public void CreateFeedIsSaved()
        {
            Controller.Feeds(new FeedsModel {FeedName = "name", FeedTitle = "title"});

            FeedRepository.Received().Save(Arg.Is<Tag>(x => x.Name == "name"));
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
