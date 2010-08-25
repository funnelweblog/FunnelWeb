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
using FunnelWeb.Web.Application.Views;
using System.Xml;
using System.Collections.Generic;

namespace FunnelWeb.Web.Controllers
{
    [Transactional]
    public partial class FeedController : Controller
    {
        public FeedController(IFeedRepository feedRepository, IMarkdownProvider markdown, ISettingsProvider settings)
        {
            _feedRepository = feedRepository;
            _markdown = markdown;
            _settings = settings;
        }

        private readonly IFeedRepository _feedRepository;
        private readonly IMarkdownProvider _markdown;
        private readonly ISettingsProvider _settings;

        private FeedResult FeedResult(IEnumerable<SyndicationItem> items)
        {
            return new FeedResult(
                new Atom10FeedFormatter(
                    new SyndicationFeed(_settings.SiteTitle, _settings.SearchDescription, new Uri(Request.Url, Url.Action("Recent", "Wiki")), items)
            {
                Id = Request.Url.ToString(),
                Links = 
                { 
                    new SyndicationLink(Request.Url) 
                    { 
                        RelationshipType = "self" 
                    }
                }
            }))
            {
                ContentType = "application/atom+xml"
            };
        }

        public virtual ActionResult Feed(PageName feedName)
        {
            if (String.IsNullOrWhiteSpace(feedName))
            {
                feedName = _feedRepository.GetFeeds().OrderBy(f => f.Id).First().Name;
            }
            var entries = _feedRepository.GetFeed(feedName, 0, 20);

            var items = (from e in entries
                         let itemUri = new Uri(Request.Url, Url.Action("Page", "Wiki", new { page = e.Name }))
                         select new
                         {
                             Item = new SyndicationItem
                             {
                                 Id = itemUri.ToString(),
                                 Title = TextSyndicationContent.CreatePlaintextContent(e.Title),
                                 Summary = TextSyndicationContent.CreateHtmlContent(
                                     _markdown.Render(e.LatestRevision.Body) + String.Format("<img src=\"{0}\" />", itemUri + "/via-feed")),
                                 LastUpdatedTime = e.LatestRevision.Revised,
                                 Links = 
                                {
                                    new SyndicationLink(itemUri) 
                                },
                                 Authors = 
                                {
                                    new SyndicationPerson { Name = _settings.Author } 
                                },
                             },
                             Keywords = e.MetaKeywords.Split(',')
                         }).ToList();

            foreach (var item in items)
            {
                foreach (var keyword in item.Keywords)
                {
                    item.Item.Categories.Add(new SyndicationCategory(keyword));
                }
            }

            return FeedResult(items.Select(i => i.Item));
        }

        public virtual ActionResult CommentFeed()
        {
            var comments = _feedRepository.GetCommentFeed(0, 20);

            var items = from e in comments
                        let itemUri = new Uri(Request.Url, Url.Action("Page", "Wiki", new { page = e.Entry.Name, comment = e.Id }))
                        select new SyndicationItem
                        {
                            Id = itemUri.ToString(),
                            Title = TextSyndicationContent.CreatePlaintextContent(e.AuthorName + " on " + e.Entry.Title),
                            Summary = TextSyndicationContent.CreateHtmlContent(_markdown.Render(e.Body, true)),
                            LastUpdatedTime = e.Posted,
                            Links = 
                            {
                                new SyndicationLink(itemUri) 
                            },
                            Authors = 
                            {
                                new SyndicationPerson { Name = e.AuthorName, Uri = e.AuthorUrl } 
                            },
                        };

            return FeedResult(items);
        }
    }
}