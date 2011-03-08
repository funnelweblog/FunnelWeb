using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Model.Repositories.Internal;
using FunnelWeb.Model.Strings;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Areas.Admin.Controllers;
using FunnelWeb.Web.Areas.Admin.Views.WikiAdmin;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Areas.Admin.Controllers
{
    [TestFixture]
    public class WikiAdminControllerTests
    {
        protected WikiAdminController AdminController { get; set; }
        protected ControllerContext ControllerContext { get; set; }
        protected IEntryRepository EntryRepository { get; set; }
        protected ITagRepository FeedRepository { get; set; }
        protected ISpamChecker SpamChecker { get; set; }
        protected IIdentity Identity { get; set; }
        protected IPrincipal User { get; set; }
        
        [TestFixtureSetUp]
        public void Setup()
        {
            AdminController = new WikiAdminController
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
        public void EditReturnsExistingPageWhenFound()
        {
            var entry = new Entry { Name = "awesome-post", LatestRevision = new Revision() };
            EntryRepository.GetEntry(Arg.Any<PageName>(), Arg.Any<int>()).Returns(entry);

            var feeds = new List<Tag>().AsQueryable();
            FeedRepository.GetTags().Returns(feeds);

            var result = (ViewResult)AdminController.Edit(entry.Name, null);

            Assert.AreEqual("Edit", result.ViewName);
            Assert.AreEqual(feeds, ((EditModel)result.ViewData.Model).AllTags);
            Assert.AreEqual(entry.Name.ToString(), ((EditModel)result.ViewData.Model).Page);

            EntryRepository.Received().GetEntry(Arg.Any<PageName>(), Arg.Any<int>());
            FeedRepository.Received().GetTags();
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
