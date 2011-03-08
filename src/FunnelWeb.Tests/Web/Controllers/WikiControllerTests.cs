using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Model.Strings;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Controllers;
using FunnelWeb.Web.Views.Wiki;
using NSubstitute;
using NUnit.Framework;
using PageModel = FunnelWeb.Web.Views.Wiki.PageModel;

namespace FunnelWeb.Tests.Web.Controllers
{
    [TestFixture]
    public class WikiControllerTests
    {
        protected WikiController Controller { get; set; }
        protected ControllerContext ControllerContext { get; set; }
        protected IEntryRepository EntryRepository { get; set; }
        protected ITagRepository FeedRepository { get; set; }
        protected ISpamChecker SpamChecker { get; set; }
        protected IIdentity Identity { get; set; }
        protected IPrincipal User { get; set; }
        
        [SetUp]
        public void SetUp()
        {
            Controller = new WikiController
                             {
                                 EntryRepository = EntryRepository = Substitute.For<IEntryRepository>(),
                                 TagRepository = FeedRepository = Substitute.For<ITagRepository>(),
                                 SpamChecker = SpamChecker = Substitute.For<ISpamChecker>(),
                                 ControllerContext = ControllerContext = CreateControllerContext()
                             };

            Identity = Substitute.For<IIdentity>();
            User = Substitute.For<IPrincipal>();
            User.Identity.Returns(Identity);
            ControllerContext.HttpContext.User.Returns(User);
        }

        [Test]
        public void Search()
        {
            var entries = Substitute.For<IEnumerable<Entry>>();
            EntryRepository.Search(Arg.Is("search")).Returns(entries);

            var result = (ViewResult)Controller.Search("search", false);

            EntryRepository.Received().Search(Arg.Is("search"));
            Assert.That(result.ViewName, Is.EqualTo("Search"));
            Assert.IsInstanceOf<SearchModel>(result.ViewData.Model);
            Assert.AreEqual(entries, ((SearchModel)result.ViewData.Model).Results);
        }

        [Test]
        public void PageLoggedInAndNew()
        {
            EntryRepository.GetEntry(Arg.Any<PageName>(), Arg.Any<int>()).Returns(x => null);
            Identity.IsAuthenticated.Returns(true);
            
            var result = (RedirectToRouteResult)Controller.Page("page", 0);

            Assert.AreEqual("Edit", result.RouteValues["Action"]);
            EntryRepository.Received().GetEntry(Arg.Is<PageName>("page"), Arg.Is(0));
        }

        [Test]
        public void WikiControllerTestsPageReturnsNotFoundForNewPageAndNotLoggedIn()
        {
            var entries = new List<Entry>();
            EntryRepository.GetEntry(Arg.Any<PageName>(), Arg.Any<int>()).Returns(x => null);
            EntryRepository.Search(Arg.Any<string>()).Returns(entries);
            Identity.IsAuthenticated.Returns(false);
            
            var result = (ViewResult)Controller.Page("page", 0);

            Assert.AreEqual("Search", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOf(typeof(SearchModel), result.ViewData.Model);

            var model = (SearchModel)result.ViewData.Model;
            Assert.AreSame(entries, model.Results);

            EntryRepository.Received().Search(Arg.Is("page"));
            EntryRepository.Received().GetEntry(Arg.Is<PageName>("page"), Arg.Is(0));
        }

        [Test]
        public void WikiControllerTestsPageReturnsFoundPage()
        {
            var entry = new Entry { Name = "page" };
            EntryRepository.GetEntry(Arg.Any<PageName>(), Arg.Any<int>()).Returns(entry);

            Controller.Page(entry.Name, 0);

            Assert.IsNotNull(Controller.ViewData.Model);
            Assert.IsInstanceOf<PageModel>(Controller.ViewData.Model);

            var model = (PageModel)Controller.ViewData.Model;
            Assert.AreEqual(entry, model.Entry);
            EntryRepository.Received().GetEntry(Arg.Is(entry.Name), Arg.Is(0));
        }

        private static ControllerContext CreateControllerContext()
        {
            var controllerContext = new ControllerContext();
            var httpContext = Substitute.For<HttpContextBase>();
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri("http://www.google.com"));
            httpContext.Request.Returns(httpRequest);
            controllerContext.HttpContext = httpContext;
            return controllerContext;
        }
    }
}
