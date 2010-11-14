using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Model.Strings;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Features.Wiki;
using FunnelWeb.Web.Features.Wiki.Views;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Controllers
{
    [TestFixture]
    public class WikiControllerTests
    {
        protected WikiController Controller { get; set; }
        protected ControllerContext ControllerContext { get; set; }
        protected IEntryRepository EntryRepository { get; set; }
        protected IFeedRepository FeedRepository { get; set; }
        protected ISpamChecker SpamChecker { get; set; }
        protected IIdentity Identity { get; set; }
        protected IPrincipal User { get; set; }
        
        [SetUp]
        public void SetUp()
        {
            Controller = new WikiController();
            Controller.EntryRepository = EntryRepository = Substitute.For<IEntryRepository>();
            Controller.FeedRepository = FeedRepository = Substitute.For<IFeedRepository>();
            Controller.SpamChecker = SpamChecker = Substitute.For<ISpamChecker>();
            Controller.ControllerContext = ControllerContext = CreateControllerContext();
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

            var result = (ViewResult)Controller.Search("search");

            EntryRepository.Received().Search(Arg.Is<string>("search"));
            Assert.That(result.ViewName, Is.EqualTo("NotFound"));
            Assert.IsInstanceOf<NotFoundModel>(result.ViewData.Model);
            Assert.AreEqual(entries, ((NotFoundModel)result.ViewData.Model).Results);
        }

        [Test]
        public void NotFoundReturnsMatchedPageIfFound()
        {
            var redirect = new Redirect() { To = "some-page" };
            EntryRepository.GetClosestRedirect(Arg.Any<string>()).Returns(redirect);

            var result = (RedirectResult)Controller.NotFound("search");

            EntryRepository.Received().GetClosestRedirect(Arg.Any<string>());
            Assert.AreEqual(result.Url, "~/" + redirect.To);
        }

        [Test]
        public void NotFoundReturnsMatchedWebsiteIfFound()
        {
            Redirect redirect = new Redirect() { To = "http://www.google.com" };
            EntryRepository.GetClosestRedirect(Arg.Any<string>()).Returns(redirect);

            var result = (RedirectResult)Controller.NotFound("search");

            EntryRepository.Received().GetClosestRedirect(Arg.Any<string>());
            Assert.AreEqual(result.Url, redirect.To);
        }

        [Test]
        public void NotFound()
        {
            var entries = new List<Entry>();
            EntryRepository.Search(Arg.Is<string>("search")).Returns(entries);
            
            var result = (ViewResult)Controller.NotFound("search");

            Assert.AreEqual("NotFound", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOf(typeof(NotFoundModel), result.ViewData.Model);
            var model = (NotFoundModel)result.ViewData.Model;
            Assert.AreSame(entries, model.Results);
            EntryRepository.Received().Search(Arg.Is<string>("search"));
        }

        [Test]
        public void PageLoggedInAndNew()
        {
            EntryRepository.GetEntry(Arg.Any<PageName>(), Arg.Any<int>()).Returns(x => null);
            Identity.IsAuthenticated.Returns(true);
            
            var result = (RedirectToRouteResult)Controller.Page("page", 0);

            Assert.AreEqual("Edit", result.RouteValues["Action"]);
            EntryRepository.Received().GetEntry(Arg.Is<PageName>("page"), Arg.Is<int>(0));
        }

        [Test]
        public void WikiControllerTests_Page_Returns_NotFound_For_New_Page_And_Not_Logged_In()
        {
            var entries = new List<Entry>();
            EntryRepository.GetEntry(Arg.Any<PageName>(), Arg.Any<int>()).Returns(x => null);
            EntryRepository.Search(Arg.Any<string>()).Returns(entries);
            Identity.IsAuthenticated.Returns(false);
            
            var result = (ViewResult)Controller.Page("page", 0);

            Assert.AreEqual("NotFound", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOf(typeof(NotFoundModel), result.ViewData.Model);

            var model = (NotFoundModel)result.ViewData.Model;
            Assert.AreSame(entries, model.Results);

            EntryRepository.Received().Search(Arg.Is<string>("page"));
            EntryRepository.Received().GetEntry(Arg.Is<PageName>("page"), Arg.Is<int>(0));
        }

        [Test]
        public void WikiControllerTests_Page_Returns_Found_Page()
        {
            var entry = new Entry() { Name = "page" };
            EntryRepository.GetEntry(Arg.Any<PageName>(), Arg.Any<int>()).Returns(entry);
            
            var result = (ViewResult)Controller.Page(entry.Name, 0);

            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOf<PageModel>(result.ViewData.Model);
            
            var model = (PageModel)result.ViewData.Model;
            Assert.AreEqual(entry, model.Entry);
            EntryRepository.Received().GetEntry(Arg.Is<PageName>(entry.Name), Arg.Is<int>(0));
        }

        [Test]
        public void New()
        {
            var feeds = new List<Feed>().AsQueryable();
            FeedRepository.GetFeeds().Returns(feeds);
            
            var result = (ViewResult)Controller.New();

            Assert.AreEqual("Edit", result.ViewName);
            Assert.AreEqual(feeds, ((EditModel)result.ViewData.Model).Feeds);
            FeedRepository.Received().GetFeeds();
        }

        [Test]
        public void EditReturnsExistingPageWhenFound()
        {
            var entry = new Entry() { Name = "Awesome Post", LatestRevision = new Revision() };
            EntryRepository.GetEntry(Arg.Any<PageName>()).Returns(entry);
            var feeds = new List<Feed>().AsQueryable();
            FeedRepository.GetFeeds().Returns(feeds);
            
            var result = (ViewResult)Controller.Edit(entry.Name);

            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
            Assert.AreEqual(feeds, ((EditModel)result.ViewData.Model).Feeds);
            Assert.AreEqual(entry.Name.ToString(), ((EditModel)result.ViewData.Model).Page);
            EntryRepository.Received().GetEntry(Arg.Any<PageName>());
            FeedRepository.Received().GetFeeds();
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
