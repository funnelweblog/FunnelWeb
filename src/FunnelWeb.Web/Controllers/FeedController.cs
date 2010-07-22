using System.Web.Mvc;
using FunnelWeb.Web.Application;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Model.Repositories;
using FunnelWeb.Web.Model.Strings;
using FunnelWeb.Web.Application.ActionResults;
using System.ServiceModel.Syndication;
using System.Linq;
using FunnelWeb.Web.Application.Settings;
using System;
using FunnelWeb.Web.Application.Markup;

namespace FunnelWeb.Web.Controllers
{
    [Transactional]
    public partial class FeedController : Controller
    {
        public FeedController(IFeedRepository feedRepository, ISettingsProvider settings)
        {
            _feedRepository = feedRepository;
            _settings = settings;
        }

        private readonly IFeedRepository _feedRepository;
        private readonly ISettingsProvider _settings;

        public ActionResult Feed(PageName feedName)
        {
            var entries = _feedRepository.GetFeed(feedName, 0, 20);

            var markdown = new MarkdownRenderer(false, HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));

            var items = from e in entries
                        let itemUri = new Uri(Request.Url, Url.Action("Page", "Wiki", new { page = e.Name }))
                        select new SyndicationItem
                        {
                            Id = itemUri.ToString(),
                            Title = TextSyndicationContent.CreatePlaintextContent(e.Title),
                            Summary = TextSyndicationContent.CreateHtmlContent(
                                markdown.Render(e.LatestRevision.Body) + String.Format("<img src=\"{0}\" />", itemUri + "/via-feed")),
                            Links = { new SyndicationLink(itemUri) },
                            LastUpdatedTime = e.LatestRevision.Revised,
                            Authors = { new SyndicationPerson { Name = _settings.Author } },
                        };

            var feedUrl = new Uri(Request.Url, Url.Action("Recent", "Wiki"));
            var feed = new SyndicationFeed(_settings.SiteTitle, _settings.SearchDescription, feedUrl, items)
            {
                Id = Request.Url.ToString(),
                Links = { new SyndicationLink(Request.Url) { RelationshipType = "self" } }
            };

            return new FeedResult(new Atom10FeedFormatter(feed)) 
            { 
                ContentType = "application/atom+xml",
            };
        }

        public ActionResult CommentFeed()
        {
            var comments = _feedRepository.GetCommentFeed(0, 20);
            ViewData.Model = new CommentFeedModel(comments);
            return View();
        }
    }
}