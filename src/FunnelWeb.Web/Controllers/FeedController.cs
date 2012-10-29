using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using FunnelWeb.Filters;
using FunnelWeb.Model;
using FunnelWeb.Settings;
using FunnelWeb.Utilities;
using FunnelWeb.Web.Application.Markup;
using FunnelWeb.Web.Application.Markup.Macros;
using FunnelWeb.Web.Application.Mvc.ActionResults;
using FunnelWeb.Repositories;
using FunnelWeb.Repositories.Queries;

namespace FunnelWeb.Web.Controllers
{
    [FunnelWebRequest]
    public class FeedController : Controller
    {
        public IRepository Repository { get; set; }
        public IContentRenderer Renderer { get; set; }
        public ISettingsProvider Settings { get; set; }

        private FeedResult FeedResult(IList<SyndicationItem> items)
        {
            var settings = Settings.GetSettings<FunnelWebSettings>();

            Debug.Assert(Request.GetOriginalUrl() != null, "Request.GetOriginalUrl() != null");

            var baseUri = Request.GetOriginalUrl();
            var feedUrl = new Uri(baseUri, Url.Action("Recent", "Wiki"));
            return new FeedResult(
                new Atom10FeedFormatter(
                    new SyndicationFeed(settings.SiteTitle, settings.SearchDescription, feedUrl, items)
                    {
                        Id = baseUri.ToString(),
                        Links =
                        {
                            new SyndicationLink(baseUri)
                            {
                                RelationshipType = "self"
                            }
                        },
                        LastUpdatedTime = items.Count() == 0 ? DateTime.Now : items.First().LastUpdatedTime
                    }), items.Count() == 0 ? DateTime.Now : items.Max(i => i.LastUpdatedTime.LocalDateTime))
            {
                ContentType = "application/atom+xml"
            };
        }

        public virtual ActionResult Feed()
        {
            var settings = Settings.GetSettings<FunnelWebSettings>();

            var entries = Repository.Find(new GetFullEntriesQuery(entryStatus: EntryStatus.PublicBlog), 0, 20);

            var baseUri = Request.GetOriginalUrl();

            var items =
                from e in entries
                let itemUri = new Uri(baseUri, Url.Action("Page", "Wiki", new { page = e.Name }))
                let viaFeedUri = new Uri(baseUri, "/via-feed" + Url.Action("Page", "Wiki", new { page = e.Name }))
                orderby e.Published descending
                let content = SyndicationContent.CreateHtmlContent(BuildFeedItemBody(itemUri, viaFeedUri, e))
                select new
                {
                    Item = new SyndicationItem
                    {
                        Id = itemUri.ToString(),
                        Title = SyndicationContent.CreatePlaintextContent(e.Title),
                        Summary = content,
                        Content = content,
                        LastUpdatedTime = TimeZoneInfo.ConvertTimeFromUtc(e.Revised, TimeZoneInfo.Local),
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
                    Keywords = e.TagsCommaSeparated.Split(',')
                };

            return FeedResult(items.Select(i =>
            {
                var item = i.Item;
                foreach (var k in i.Keywords)
                    item.Categories.Add(new SyndicationCategory(k.Trim()));

                return item;
            }).ToList());
        }

        private string BuildFeedItemBody(Uri itemUri, Uri viaFeedUri, EntryRevision latestRevision)
        {
            var result =
                Renderer.RenderTrusted(latestRevision.Body, latestRevision.Format, CreateHelper())
                + string.Format("<img src=\"{0}\" />", viaFeedUri);

            if (Settings.GetSettings<FunnelWebSettings>().FacebookLike)
            {
                var facebook = string.Format(@"  <div class='facebook'>
                      <iframe src='http://www.facebook.com/plugins/like.php?href={0}&amp;layout=standard&amp;show_faces=true&amp;width=450&amp;action=like&amp;colorscheme=light&amp;height=80' scrolling='no' frameborder='0' style='border:none; overflow:hidden; width:450px; height:80px;' allowTransparency='true'></iframe>
                    </div>", Url.Encode(itemUri.AbsoluteUri));
                result += facebook;
            }

            return result;
        }

        private HtmlHelper CreateHelper()
        {
            return new HtmlHelper(new ViewContext(ControllerContext, new DummyView(), new ViewDataDictionary(), new TempDataDictionary(), new StringWriter()), new CustomViewDataContainer());
        }

        public virtual ActionResult CommentFeed()
        {
            var comments = Repository.Find(new GetCommentsQuery(), 0, 20);

            var baseUri = Request.GetOriginalUrl();
            var items =
                from e in comments
                let itemUri = new Uri(baseUri, Url.Action("Page", "Wiki", new { page = e.Entry.Name }) + "#comment-" + e.Id)
                select new SyndicationItem
                {
                    Id = itemUri.ToString(),
                    Title = SyndicationContent.CreatePlaintextContent(e.AuthorName + " on " + e.Entry.Title),
                    Summary = SyndicationContent.CreateHtmlContent(Renderer.RenderUntrusted(e.Body, Formats.Markdown, CreateHelper())),
                    Content = SyndicationContent.CreateHtmlContent(Renderer.RenderUntrusted(e.Body, Formats.Markdown, CreateHelper())),
                    LastUpdatedTime = e.Posted,
                    PublishDate = e.Posted,
                    Links =
                    {
                        new SyndicationLink(itemUri)
                    },
                    Authors =
                    {
                        new SyndicationPerson { Name = e.AuthorName, Uri = e.AuthorUrl }
                    },
                };

            return FeedResult(items.ToList());
        }

        internal class DummyView : IView
        {
            public void Render(ViewContext viewContext, TextWriter writer)
            {

            }
        }
    }
}
