using System.Linq;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Web.Controllers;
using FunnelWeb.Web.Model.Repositories;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using FunnelWeb.Web.Model;

namespace FunnelWeb.Tests.Web.Controllers
{
    public class AdminControllerTests
    {
        [TestFixture]
        public class ActionResultTests
        {

            [Test]
            public void AdminControllerTests_Index_Action_Returns_Index_View()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var controller = new AdminController(Substitute.For<IAdminRepository>(), Substitute.For<IFeedRepository>())
                {
                    ControllerContext = controllerContext
                };

                //Act
                var result = (ViewResult)controller.Index();

                //Assert
                Assert.That(result.ViewName, Is.EqualTo(string.Empty));
            }

            [Test]
            public void AdminControllerTests_CreateFeed_Action_Returns_Index_View()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var controller = new AdminController(Substitute.For<IAdminRepository>(), Substitute.For<IFeedRepository>())
                {
                    ControllerContext = controllerContext
                };

                //Act
                var result = (RedirectToRouteResult)controller.CreateFeed(Arg.Any<string>(), Arg.Any<string>());

                //Assert
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }

            [Test]
            public void AdminControllerTests_DeleteFeed_Action_Returns_Index_View()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                IFeedRepository feeds = Substitute.For<IFeedRepository>();
                feeds.GetFeeds().Returns(new List<Feed>().AsQueryable());
                var controller = new AdminController(Substitute.For<IAdminRepository>(), feeds)
                {
                    ControllerContext = controllerContext
                };

                //Act
                var result = (RedirectToRouteResult)controller.DeleteFeed(0);

                //Assert
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }

            [Test]
            public void AdminControllerTests_DeleteRedirect_Action_Returns_Index_View()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                IAdminRepository admin = Substitute.For<IAdminRepository>();
                admin.GetRedirects().Returns(new List<Redirect>().AsQueryable());
                var controller = new AdminController(admin, Substitute.For<IFeedRepository>())
                {
                    ControllerContext = controllerContext
                };

                //Act
                var result = (RedirectToRouteResult)controller.DeleteRedirect(0);

                //Assert
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }

            [Test]
            public void AdminControllerTests_CreateRedirect_Action_Returns_Index_View()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var controller = new AdminController(Substitute.For<IAdminRepository>(), Substitute.For<IFeedRepository>())
                {
                    ControllerContext = controllerContext
                };

                //Act
                var result = (RedirectToRouteResult)controller.CreateRedirect(Arg.Any<string>(), Arg.Any<string>());

                //Assert
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }

            [Test]
            public void AdminControllerTests_UpdateSettings_Action_Returns_Index_View()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var controller = new AdminController(Substitute.For<IAdminRepository>(), Substitute.For<IFeedRepository>())
                {
                    ControllerContext = controllerContext
                };

                //Act
                var result = (RedirectToRouteResult)controller.UpdateSettings(Arg.Any<Dictionary<string, string>>());

                //Assert
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }

            [Test]
            public void AdminControllerTests_DeleteComment_Action_Returns_Index_View()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var controller = new AdminController(Substitute.For<IAdminRepository>(), Substitute.For<IFeedRepository>())
                {
                    ControllerContext = controllerContext
                };

                //Act
                var result = (RedirectToRouteResult)controller.DeleteComment(0);

                //Assert
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }

            [Test]
            public void AdminControllerTests_DeleteAllSpam_Returns_Index_View()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var controller = new AdminController(Substitute.For<IAdminRepository>(), Substitute.For<IFeedRepository>())
                {
                    ControllerContext = controllerContext
                };

                //Act
                var result = (RedirectToRouteResult)controller.DeleteAllSpam();

                //Assert
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }

            [Test]
            public void AdminControllerTests_DeletePingback_Returns_Index_View()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var controller = new AdminController(Substitute.For<IAdminRepository>(), Substitute.For<IFeedRepository>())
                {
                    ControllerContext = controllerContext
                };

                //Act
                var result = (RedirectToRouteResult)controller.DeletePingback(0);

                //Assert
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }

            [Test]
            public void AdminControllerTests_ToggleSpam_Returns_Index_View()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var controller = new AdminController(Substitute.For<IAdminRepository>(), Substitute.For<IFeedRepository>())
                {
                    ControllerContext = controllerContext
                };

                //Act
                var result = (RedirectToRouteResult)controller.ToggleSpam(0);

                //Assert
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }

            [Test]
            public void AdminControllerTests_ToggePingbackSpam_Returns_Index_View()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var controller = new AdminController(Substitute.For<IAdminRepository>(), Substitute.For<IFeedRepository>())
                {
                    ControllerContext = controllerContext
                };

                //Act
                var result = (RedirectToRouteResult)controller.TogglePingbackSpam(0);

                //Assert
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }
        }

        [TestFixture]
        public class FeedsRepositoryTests
        {
            [Test]
            public void FeedsRepositoryTests_CreateFeed_Saved_To_Repo()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var feedsRepo = Substitute.For<IFeedRepository>();
                var controller = new AdminController(Substitute.For<IAdminRepository>(), feedsRepo);

                //Act
                controller.CreateFeed("name", "title");

                //Assert
                feedsRepo.Received().Save(Arg.Is<Feed>(x => x.Name == "name"));
            }
        }

        private static ControllerContext CreateControllerContext()
        {
            var controllerContext = new ControllerContext();
            HttpContextBase httpContext = Substitute.For<HttpContextBase>();
            HttpServerUtilityBase httpServer = Substitute.For<HttpServerUtilityBase>();
            httpServer.MapPath(Arg.Any<string>()).Returns(@"C:\Windows");
            httpContext.Server.Returns(httpServer);
            controllerContext.HttpContext = httpContext;
            return controllerContext;
        }
    }
}
