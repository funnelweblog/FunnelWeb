using System;
using System.Web.Mvc;
using FunnelWeb.Eventing;
using FunnelWeb.Filters;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Model.Strings;
using FunnelWeb.Repositories;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Application.Mvc.ActionResults;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Views.Wiki;

namespace FunnelWeb.Web.Controllers
{
    [FunnelWebRequest]
    [HandleError]
    [ValidateInput(false)]
    public class WikiController : Controller
    {
        private const int ItemsPerPage = 30;
        public IRepository Repository { get; set; }
        public ITagRepository TagRepository { get; set; }
        public IFeedRepository FeedRepository { get; set; }
        public ISpamChecker SpamChecker { get; set; }
        public IEventPublisher EventPublisher { get; set; }
        public ISettingsProvider SettingsProvider { get; set; }

        public virtual ActionResult Home(int? pageNumber)
        {
            var settings = SettingsProvider.GetSettings<FunnelWebSettings>();
            if (!string.IsNullOrWhiteSpace(settings.CustomHomePage))
            {
                var entry = Repository.FindFirstOrDefault(new EntryByNameQuery(settings.CustomHomePage));
                if (entry != null)
                {
                    ViewData.Model = new PageModel(entry.Name, entry);
                    return new PageTemplateActionResult(
                        pageTemplate: entry.PageTemplate,
                        actionName: "Page"
                    );
                }
            }

            return Recent(pageNumber ?? 0);
        }

        public virtual ActionResult Recent(int pageNumber)
        {
            var result = Repository.Find(new GetEntriesQuery(), pageNumber, ItemsPerPage);
            ViewData.Model = new RecentModel("Recent Posts", result, ControllerContext.RouteData.Values["action"].ToString());
            return View("Recent");
        }

        public virtual ActionResult Search([Bind(Prefix = "q")] string searchText, bool? is404)
        {
            var results = Repository.Find(new SearchEntriesQuery(searchText), 0, 30);
            return View("Search", new SearchModel(searchText, is404 ?? false, results));
        }

        public virtual ActionResult Page(PageName page, int? revision)
        {
            if (revision != null && !SettingsProvider.GetSettings<FunnelWebSettings>().EnablePublicHistory)
            {
                return RedirectToAction("Page", "Wiki", new { page, revision = (int?)null });
            }

            var entry = revision == null
                            ? Repository.FindFirstOrDefault(new EntryByNameQuery(page))
                            : Repository.FindFirstOrDefault(new EntryByNameAndRevisionQuery(page, revision.Value));

            if (entry == null)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                    return RedirectToAction("Edit", "WikiAdmin", new { Area = "Admin", page });
                return Search(page, true);
            }

            if (entry.Status == EntryStatus.Private && !HttpContext.User.Identity.IsAuthenticated)
            {
                return Search(page, true);
            }

            ViewData.Model = new PageModel(page, entry);
            return new PageTemplateActionResult(
                pageTemplate: entry.PageTemplate
            );
        }

        // Posting a comment
        [HttpPost]
        public virtual ActionResult Page(PageName page, PageModel model)
        {
            var entry = Repository.FindFirstOrDefault(new EntryByNameQuery(page));
            if (entry == null)
                return RedirectToAction("Recent");

            if (!ModelState.IsValid)
            {
                model.Entry = entry;
                model.Page = page;
                ViewData.Model = model;
                return new PageTemplateActionResult(entry.PageTemplate, "Page");
            }

            var comment = entry.Entry.Value.Comment();
            comment.AuthorEmail = model.CommenterEmail ?? string.Empty;
            comment.AuthorName = model.CommenterName ?? string.Empty;
            comment.AuthorUrl = model.CommenterBlog ?? string.Empty;
            comment.AuthorIp = Request.UserHostAddress;
            comment.EntryRevisionNumber = entry.LatestRevisionNumber;
            comment.Body = model.Comments;

            try
            {
                SpamChecker.Verify(comment);
            }
            catch (Exception ex)
            {
                HttpContext.Trace.Warn("Akismet is offline, comment cannot be validated: " + ex);
            }

            // Anything posted after the disable date is considered spam (the comment box shouldn't be visible anyway)
            var settings = SettingsProvider.GetSettings<FunnelWebSettings>();
            if (settings.DisableCommentsOlderThan > 0 && DateTime.UtcNow.AddDays(settings.DisableCommentsOlderThan) > entry.Published)
            {
                comment.IsSpam = true;
            }

            EventPublisher.Publish(new CommentPostedEvent(entry.Entry.Value, comment));

            return RedirectToAction("Page", new { page })
                .AndFlash("Thanks, your comment has been posted.");
        }

        public virtual ActionResult Revisions(PageName page)
        {
            var settings = SettingsProvider.GetSettings<FunnelWebSettings>();
            if (!settings.EnablePublicHistory)
            {
                return RedirectToAction("Page", "Wiki", new { page });
            }

            var entry = Repository.FindFirstOrDefault(new EntryByNameQuery(page));
            if (entry == null)
            {
                return RedirectToAction("Edit", "WikiAdmin", new { page });
            }

            ViewData.Model = new RevisionsModel(page, entry.Entry.Value);
            return View();
        }

        public virtual ActionResult SiteMap()
        {
            var allPosts = Repository.Find(new GetFullEntriesQuery(true), 0, 500);
            ViewData.Model = new SiteMapModel(allPosts);
            return View();
        }
    }
}
