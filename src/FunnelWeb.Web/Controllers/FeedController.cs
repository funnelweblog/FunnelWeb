using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
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
        public ITagRepository TagRepository { get; set; }
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

        public virtual ActionResult Feed()
        {
            var settings = Settings.GetSettings();
            
            var entries = FeedRepository.GetRecentEntries(0, 20);

            var items =
                from e in entries
                let itemUri = new Uri(Request.Url, Url.Action("Page", "Wiki", new { page = e.Name }))
                let viaFeedUri = new Uri(Request.Url, "/via-feed" + Url.Action("Page", "Wiki", new { page = e.Name }))
                orderby e.Published descending
                let content = SyndicationContent.CreateHtmlContent(
                            BuildFeedItemBody(itemUri, viaFeedUri, e.LatestRevision))
                select new
                {
                    Item = new SyndicationItem
                    {
                        Id = itemUri.ToString(),
                        Title = SyndicationContent.CreatePlaintextContent(e.Title),
                        Summary = content,
                        Content = content,
                        LastUpdatedTime = e.LatestRevision.Revised,
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
                    Keywords = e.Tags.Select(x => x.Name).ToArray()
                };

            return FeedResult(items.Select(i =>
            {
                var item = i.Item;
                foreach (var k in i.Keywords)
                    item.Categories.Add(new SyndicationCategory(k.Trim()));
                return item;
            }));
        }

        private string BuildFeedItemBody(Uri itemUri, Uri viaFeedUri, Revision latestRevision)
        {
            var result = Markdown.Render(latestRevision.Body)
                         + string.Format("<img src=\"{0}\" />", viaFeedUri);

            if (Settings.GetSettings().FacebookLike)
            {
                var facebook = string.Format(@"  <div class='facebook'>
                      <iframe src='http://www.facebook.com/plugins/like.php?href={0}&amp;layout=standard&amp;show_faces=true&amp;width=450&amp;action=like&amp;colorscheme=light&amp;height=80' scrolling='no' frameborder='0' style='border:none; overflow:hidden; width:450px; height:80px;' allowTransparency='true'></iframe>
                    </div>", Url.Encode(itemUri.AbsoluteUri));
                result += facebook;
            }

            return result;
        }

        public virtual ActionResult CommentFeed()
        {
            var comments = FeedRepository.GetRecentComments(0, 20);

            var items =
                from e in comments
                let itemUri = new Uri(Request.Url, Url.Action("Page", "Wiki", new { page = e.Entry.Name }) + "#comment-" + e.Id)
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