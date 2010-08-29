using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Controllers;
using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Repositories;
using NSubstitute;
using NUnit.Framework;
using System.Security.Principal;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Tests.Web.Controllers
{
    [TestFixture]
    public class WikiControllerTests
    {
        private static ControllerContext CreateControllerContext()
        {
            var controllerContext = new ControllerContext();
            HttpContextBase httpContext = Substitute.For<HttpContextBase>();
            HttpRequestBase httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri("http://www.google.com"));
            httpContext.Request.Returns(httpRequest);
            controllerContext.HttpContext = httpContext;
            return controllerContext;
        }

        [Test]
        public void WikiControllerTests_Search_Returns_NotFound_View()
        {
            //Arrange
            var entryRepo = Substitute.For<IEntryRepository>();
            IEnumerable<Entry> entries = Substitute.For<IEnumerable<Entry>>();
            entryRepo.Search(Arg.Is<string>("search")).Returns(entries);

            var feedRepo = Substitute.For<IFeedRepository>();
            var spamChecker = Substitute.For<ISpamChecker>();

            var controller = new WikiController(entryRepo, feedRepo, spamChecker);
            //Act
            var result = (ViewResult)controller.Search("search");

            //Assert
            entryRepo.Received().Search(Arg.Is<string>("search"));
            Assert.That(result.ViewName, Is.EqualTo("NotFound"));
            Assert.IsInstanceOf<WikiController.NotFoundModel>(result.ViewData.Model);
            Assert.AreEqual(entries, ((WikiController.NotFoundModel)result.ViewData.Model).Results);
        }

        [Test]
        public void WikiControllerTests_NotFound_Returns_Matched_Page_If_Found()
        {
            //Arrange
            var entryRepo = Substitute.For<IEntryRepository>();
            Redirect redirect = new Redirect() { To = "some-page" };
            entryRepo.GetClosestRedirect(Arg.Any<string>()).Returns(redirect);

            var feedRepo = Substitute.For<IFeedRepository>();
            var spamChecker = Substitute.For<ISpamChecker>();

            var controller = new WikiController(entryRepo, feedRepo, spamChecker)
            {
                ControllerContext = CreateControllerContext()
            };
            //Act
            var result = (RedirectResult)controller.NotFound("search");

            //Assert
            entryRepo.Received().GetClosestRedirect(Arg.Any<string>());
            Assert.AreEqual(result.Url, "~/" + redirect.To);
        }

        [Test]
        public void WikiControllerTests_NotFound_Returns_Matched_Website_If_Found()
        {
            //Arrange
            var entryRepo = Substitute.For<IEntryRepository>();
            Redirect redirect = new Redirect() { To = "http://www.google.com" };
            entryRepo.GetClosestRedirect(Arg.Any<string>()).Returns(redirect);

            var feedRepo = Substitute.For<IFeedRepository>();
            var spamChecker = Substitute.For<ISpamChecker>();

            var controller = new WikiController(entryRepo, feedRepo, spamChecker)
            {
                ControllerContext = CreateControllerContext()
            };
            //Act
            var result = (RedirectResult)controller.NotFound("search");

            //Assert
            entryRepo.Received().GetClosestRedirect(Arg.Any<string>());
            Assert.AreEqual(result.Url, redirect.To);
        }

        [Test]
        public void WikiControllerTests_NotFound_Returns_NotFoundView_If_No_Site()
        {
            //Arrange
            var entryRepo = Substitute.For<IEntryRepository>();
            var entries = new List<Entry>();
            entryRepo.Search(Arg.Is<string>("search")).Returns(entries);
            var feedRepo = Substitute.For<IFeedRepository>();
            var spamChecker = Substitute.For<ISpamChecker>();

            var controller = new WikiController(entryRepo, feedRepo, spamChecker)
            {
                ControllerContext = CreateControllerContext()
            };
            //Act
            var result = (ViewResult)controller.NotFound("search");

            //Assert
            Assert.AreEqual("NotFound", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOf(typeof(WikiController.NotFoundModel), result.ViewData.Model);

            var model = (WikiController.NotFoundModel)result.ViewData.Model;
            Assert.AreSame(entries, model.Results);

            entryRepo.Received().Search(Arg.Is<string>("search"));
        }

        [Test]
        public void WikiControllerTests_Page_Returns_Edit_For_New_Page_And_Logged_In()
        {
            //Arrange
            var entryRepo = Substitute.For<IEntryRepository>();
            entryRepo.GetEntry(Arg.Any<PageName>(), Arg.Any<int>()).Returns(x => null);
            var feedRepo = Substitute.For<IFeedRepository>();
            var spamChecker = Substitute.For<ISpamChecker>();
            var controllerContext = CreateControllerContext();

            IPrincipal user = Substitute.For<IPrincipal>();
            IIdentity identity = Substitute.For<IIdentity>();
            identity.IsAuthenticated.Returns(true);
            user.Identity.Returns(identity);
            controllerContext.HttpContext.User.Returns(user);

            var controller = new WikiController(entryRepo, feedRepo, spamChecker)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = (RedirectToRouteResult)controller.Page("page", 0);

            //Assert
            Assert.AreEqual("Edit", result.RouteValues["Action"]);
            entryRepo.Received().GetEntry(Arg.Is<PageName>("page"), Arg.Is<int>(0));

        }

        [Test]
        public void WikiControllerTests_Page_Returns_NotFound_For_New_Page_And_Not_Logged_In()
        {
            //Arrange
            var entryRepo = Substitute.For<IEntryRepository>();
            entryRepo.GetEntry(Arg.Any<PageName>(), Arg.Any<int>()).Returns(x => null);
            var entries = new List<Entry>();
            entryRepo.Search(Arg.Any<string>()).Returns(entries);
            var feedRepo = Substitute.For<IFeedRepository>();
            var spamChecker = Substitute.For<ISpamChecker>();
            var controllerContext = CreateControllerContext();

            IPrincipal user = Substitute.For<IPrincipal>();
            IIdentity identity = Substitute.For<IIdentity>();
            identity.IsAuthenticated.Returns(false);
            user.Identity.Returns(identity);
            controllerContext.HttpContext.User.Returns(user);

            var controller = new WikiController(entryRepo, feedRepo, spamChecker)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = (ViewResult)controller.Page("page", 0);

            //Assert
            Assert.AreEqual("NotFound", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOf(typeof(WikiController.NotFoundModel), result.ViewData.Model);

            var model = (WikiController.NotFoundModel)result.ViewData.Model;
            Assert.AreSame(entries, model.Results);

            entryRepo.Received().Search(Arg.Is<string>("page"));
            entryRepo.Received().GetEntry(Arg.Is<PageName>("page"), Arg.Is<int>(0));

        }

        [Test]
        public void WikiControllerTests_Page_Returns_Found_Page()
        {
            //Arrange
            var entryRepo = Substitute.For<IEntryRepository>();
            var entry = new Entry() { Name = "page" };
            entryRepo.GetEntry(Arg.Any<PageName>(), Arg.Any<int>()).Returns(entry);
            var feedRepo = Substitute.For<IFeedRepository>();
            var spamChecker = Substitute.For<ISpamChecker>();
            var controllerContext = CreateControllerContext();

            var controller = new WikiController(entryRepo, feedRepo, spamChecker)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = (ViewResult)controller.Page(entry.Name, 0);

            //Assert
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOf<WikiController.PageModel>(result.ViewData.Model);

            var model = (WikiController.PageModel)result.ViewData.Model;

            Assert.AreEqual(entry, model.Entry);

            entryRepo.Received().GetEntry(Arg.Is<PageName>(entry.Name), Arg.Is<int>(0));

        }
    }
}
