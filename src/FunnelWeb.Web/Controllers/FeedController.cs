using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Model.Strings;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Application.Mvc.ActionResults;
using FunnelWeb.Web.Application.Views;

namespace FunnelWeb.Web.Controllers
{
    [FunnelWebRequest]
    public class FeedController : Controller
    {
        public IFeedRepository FeedRepository { get; set; }
        public IMarkdownProvider Markdown { get; set; }
        public ISettingsProvider Settings { get; set; }

        private FeedResult FeedResult(IEnumerable<SyndicationItem> items)
        {
            var settings = Settings.GetSettings();

            var feedUrl = new Uri(Request.Url, Url.Action("Recent", "Wiki"));
            return new FeedResult(
                new Atom10FeedFormatter(
                    new SyndicationFeed(settings.SiteTitle, settings.SearchDescription, feedUrl, items)
                    {
                        Id = Request.Url.ToString(),
                        Links = 
                        { 
                            new SyndicationLink(Request.Url) 
                            { 
                                RelationshipType = "self" 
                            }
                        },
                        LastUpdatedTime = items.Count() == 0 ? DateTime.Now : items.First().LastUpdatedTime
                    }))
            {
                ContentType = "application/atom+xml"
            };
        }

        public virtual ActionResult Feed(PageName feedName)
        {
            var settings = Settings.GetSettings();
            if (String.IsNullOrWhiteSpace(feedName))
                feedName = FeedRepository.GetFeeds().OrderBy(f => f.Id).First().Name;

            var entries = FeedRepository.GetFeed(feedName, 0, 20);

            var items =
                from e in entries
                let itemUri = new Uri(Request.Url, Url.Action("Page", "Wiki", new { page = e.Name }))
                orderby e.FeedDate descending
                let content = TextSyndicationContent.CreateHtmlContent(
                            Markdown.Render(e.LatestRevision.Body) +
                            String.Format("<img src=\"{0}\" />", itemUri + "/via-feed"))
                select new
                {
                    Item = new SyndicationItem
                    {
                        Id = itemUri.ToString(),
                        Title = TextSyndicationContent.CreatePlaintextContent(e.Title),
                        Summary = content,
                        Content = content,
                        LastUpdatedTime = e.FeedDate,
                        PublishDate = e.Published,
                        Links =
                            {
                                new SyndicationLink(itemUri)
                            },
                        Authors =
                            {
                                new SyndicationPerson {Name = settings.Author}
                            },
                    },
                    Keywords = e.MetaKeywords.Split(',')
                };

            return FeedResult(items.Select(i =>
            {
                var item = i.Item;
                foreach (var k in i.Keywords)
                    item.Categories.Add(new SyndicationCategory(k.Trim()));
                return item;
            }));
        }

        public virtual ActionResult CommentFeed()
        {
            var comments = FeedRepository.GetCommentFeed(0, 20);

            var items =
                from e in comments
                let itemUri = new Uri(Request.Url, Url.Action("Page", "Wiki", new { page = e.Entry.Name, comment = e.Id }))
                select new SyndicationItem
                {
                    Id = itemUri.ToString(),
                    Title = SyndicationContent.CreatePlaintextContent(e.AuthorName + " on " + e.Entry.Title),
                    Summary = SyndicationContent.CreateHtmlContent(Markdown.Render(e.Body, true)),
                    Content = SyndicationContent.CreateHtmlContent(Markdown.Render(e.Body, true)),
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