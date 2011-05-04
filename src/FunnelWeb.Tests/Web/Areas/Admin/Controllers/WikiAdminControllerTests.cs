using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Model.Strings;
using FunnelWeb.Repositories;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Areas.Admin.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Areas.Admin.Controllers
{
    [TestFixture]
    public class WikiAdminControllerTests
    {
        protected WikiAdminController AdminController { get; set; }
        protected ControllerContext ControllerContext { get; set; }
        protected IRepository Repository { get; set; }
        protected ITagRepository FeedRepository { get; set; }
        protected ISpamChecker SpamChecker { get; set; }
        protected IIdentity Identity { get; set; }
        protected IPrincipal User { get; set; }
        
        [TestFixtureSetUp]
        public void Setup()
        {
            AdminController = new WikiAdminController
                                  {
                                      Repository = Repository = Substitute.For<IRepository>(),
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
            var entry = new EntryRevision { Name = "awesome-post" };
            Repository.FindFirstOrDefault(Arg.Any<EntryByNameAndRevisionQuery>()).Returns(entry);

            var feeds = new List<Tag>().AsQueryable();
            FeedRepository.GetTags().Returns(feeds);

            var result = (ViewResult)AdminController.Edit(entry.Name, null);

            Assert.AreEqual("Edit", result.ViewName);
            Assert.AreEqual(feeds, ((EntryRevision)result.ViewData.Model).AllTags);
            Assert.AreEqual(entry.Name.ToString(), ((EntryRevision)result.ViewData.Model).Name.ToString());

            Repository.Received().FindFirstOrDefault(Arg.Any<EntryByNameAndRevisionQuery>());
            FeedRepository.Received().GetTags();
        }

        [Test]
        public void EditReturnsNewModelWhenEntryNotFound()
        {
            // arrange
            const string newPostName = "new-awesome-post";

            // act
            var result = (ViewResult)AdminController.Edit(newPostName, null);

            // assert
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsInstanceOf(typeof(EntryRevision), result.ViewData.Model);
            Assert.NotNull(((EntryRevision)result.ViewData.Model).Tags);
            Assert.AreEqual("new-awesome-post", ((EntryRevision)result.ViewData.Model).Name.ToString());
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
