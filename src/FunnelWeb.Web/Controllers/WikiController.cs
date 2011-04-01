using System;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Eventing;
using FunnelWeb.Filters;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Model.Strings;
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
        public IEntryRepository EntryRepository { get; set; }
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
                var entry = EntryRepository.GetEntry(settings.CustomHomePage);
                if (entry != null)
                {
                    ViewData.Model = new PageModel(entry.Name, entry, false);
                    return new PageTemplateActionResult(entry.PageTemplate, "Page");
                }
            }

            return Recent(pageNumber ?? 0);
        }

        public virtual ActionResult Recent(int pageNumber)
        {
            var entries = FeedRepository.GetRecentEntries(pageNumber * ItemsPerPage, ItemsPerPage);
            var totalItems = FeedRepository.GetEntryCount();
            ViewData.Model = new RecentModel("Recent Posts", entries, pageNumber, (int)((decimal)totalItems / ItemsPerPage + 1), ControllerContext.RouteData.Values["action"].ToString());
            return View("Recent");
        }

        public virtual ActionResult Search([Bind(Prefix = "q")] string searchText, bool? is404)
        {
            var results = EntryRepository.Search(searchText);
            return View("Search", new SearchModel(searchText, is404 ?? false, results));
        }

        public virtual ActionResult Page(PageName page, int? revision)
        {
            var entry = EntryRepository.GetEntry(page, revision ?? 0);
            if (entry == null)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
					return RedirectToAction("Edit", "WikiAdmin", new { Area = "Admin", page });
                }
                return Search(page, true);
            }

            if (entry.Status == EntryStatus.Private && !HttpContext.User.Identity.IsAuthenticated)
            {
                return Search(page, true);
            }

            ViewData.Model = new PageModel(page, entry, revision > 0 && revision < entry.LatestRevision.RevisionNumber);
            return new PageTemplateActionResult(entry.PageTemplate);
        }

        // Posting a comment
        [HttpPost]
        public virtual ActionResult Page(PageName page, PageModel model)
        {
            var entry = EntryRepository.GetEntry(page);
            if (entry == null)
                return RedirectToAction("Recent");

            if (!ModelState.IsValid)
            {
                model.Entry = entry;
                model.IsPriorVersion = false;
                model.Page = page;
                ViewData.Model = model;
                return new PageTemplateActionResult(entry.PageTemplate, "Page")
                    .AndFlash("Your comment was not posted - please check the validation errors below.");
            }

            var comment = entry.Comment();
            comment.AuthorEmail = model.CommenterEmail ?? string.Empty;
            comment.AuthorName = model.CommenterName ?? string.Empty;
            comment.AuthorUrl = model.CommenterBlog ?? string.Empty;
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

            EntryRepository.Save(entry);

            EventPublisher.Publish(new CommentPostedEvent(entry, comment));

            return RedirectToAction("Page", new { page })
				.AndFlash("Thanks, your comment has been posted.");
        }

        public virtual ActionResult Revisions(PageName page)
        {
            var entry = EntryRepository.GetEntry(page);
            if (entry == null)
            {
                return RedirectToAction("Edit", "WikiAdmin", new { page });
            }

            ViewData.Model = new RevisionsModel(page, entry);
            return View();
        }

        public virtual ActionResult SiteMap()
        {
            var allPosts = EntryRepository.GetEntries().OrderBy(x => x.Published).ToList();
            ViewData.Model = new SiteMapModel(allPosts);
            return View();
        }
    }
}
