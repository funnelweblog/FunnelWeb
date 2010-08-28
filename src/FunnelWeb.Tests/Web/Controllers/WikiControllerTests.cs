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
    }
}
