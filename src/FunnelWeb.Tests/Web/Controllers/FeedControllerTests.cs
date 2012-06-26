using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using FunnelWeb.Model;
using FunnelWeb.Repositories;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Web.Application.Mvc.ActionResults;
using FunnelWeb.Web.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Controllers
{
    [TestFixture]
    public class FeedControllerTests : ControllerTests
    {
        protected StringWriter Output { get; set; }
        protected FeedController Controller { get; set; }
        protected HttpResponseBase Response { get; set; }
        protected HttpCachePolicyBase Cache { get; set; }

        [SetUp]
        public void SetUp()
        {
            Response = Substitute.For<HttpResponseBase>();
            Output = new StringWriter();
            Response.Output.Returns(Output);
            Cache = Substitute.For<HttpCachePolicyBase>();
            ControllerContext.HttpContext.Response.Returns(Response);
            Response.Cache.Returns(Cache);
            Controller = new FeedController
            {
                Repository = Repository,
                Settings = SettingsProvider,
                ControllerContext = ControllerContext,
                Url = UrlHelper,
                Renderer = ContentRenderer
            };
        }

        [Test]
        public void SetsLastModifiedOnResponseCache()
        {
            var published = DateTime.Now.AddDays(-1);
            var entries = new List<EntryRevision>
                              {
                                  new EntryRevision
                                      {
                                          Title = "Title",
                                          Author = "Test",
                                          Body = "Some Body",
                                          Name = "Name",
                                          Published = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now.AddDays(-3)),
                                          Revised = TimeZoneInfo.ConvertTimeToUtc(published),
                                      }
                              };
            Repository.Find(Arg.Any<GetFullEntriesQuery>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(new PagedResult<EntryRevision>(entries, 1, 0, 1));
            var result = (FeedResult)Controller.Feed();
            result.ExecuteResult(ControllerContext);

            Cache.Received().SetLastModified(published);
        }

        [Test]
        public void FeedOutputsCorrectly()
        {
            var entries = new List<EntryRevision>
                              {
                                  new EntryRevision
                                      {
                                          Author = "Test",
                                          Body = "Some Body",
                                          Name = "Title",
                                          Published = DateTime.UtcNow.AddDays(-3),
                                          Revised = DateTime.UtcNow.AddDays(-1),
                                      }
                              };
            Repository.Find(Arg.Any<GetFullEntriesQuery>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(new PagedResult<EntryRevision>(entries, 1, 0, 1));

            var result = (FeedResult)Controller.Feed();
            
            result.ExecuteResult(ControllerContext);

            var feed = Output.ToString();
            Assert.True(feed.Contains("Some Body"));
        }
    }
}