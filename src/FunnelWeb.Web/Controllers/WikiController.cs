using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Eventing;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Model.Strings;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Application.Mvc;
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
            var settings = SettingsProvider.GetSettings();
            if (!string.IsNullOrWhiteSpace(settings.CustomHomePage))
            {
                var entry = EntryRepository.GetEntry(settings.CustomHomePage);
                if (entry != null)
                {
                    return View("Page", new PageModel(entry.Name, entry, false));
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
                    return RedirectToAction("Edit", "Wiki", new {page});
                }
                return Search(page, true);
            }

            if (entry.Status == EntryStatus.Private && !HttpContext.User.Identity.IsAuthenticated)
            {
                return Search(page, true);
            }

            ViewData.Model = new PageModel(page, entry, revision > 0);
            return View();
        }

        [Authorize]
        public virtual ActionResult Unpublished()
        {
            var allPosts = EntryRepository.GetUnpublished();
            return View("Recent", new RecentModel("Unpublished Posts", allPosts, 1, 1, "Unpublished"));
        }

        [Authorize]
        public virtual ActionResult Edit(PageName page, int? revertToRevision)
        {
            var entry = EntryRepository.GetEntry(page, revertToRevision ?? 0) 
                ?? new Entry
                {
                    Title = "New post",
                    MetaTitle = "New post", 
                    Name = page, 
                    Status = EntryStatus.PublicBlog,
                    LatestRevision = new Revision()
                };
            
            var allTags = TagRepository.GetTags();
            var model = new EditModel(page, entry.Id, allTags);
            model.DisableComments = !entry.IsDiscussionEnabled;
            model.Content = entry.LatestRevision.Body;
            model.Format = entry.LatestRevision.Format;
            model.HideChrome = entry.HideChrome;
            model.Status = entry.Status;
            model.MetaDescription = entry.MetaDescription;
            model.MetaTitle = entry.MetaTitle;
            model.PublishDate = entry.Published.ToLocalTime().ToString("yyyy-MM-dd");
            model.Sidebar = entry.Summary;
            model.Title = entry.Title;
            model.SelectedTags = entry.Tags;
            model.ChangeSummary = entry.Id == 0 ? "Initial create" : (revertToRevision == null ? "" :  "Reverted to version " + revertToRevision);

            if (revertToRevision != null)
            {
                return View("Edit", model).AndFlash("You are editing an old version of this page. This will become the current version when you save.");
            }
            return View("Edit", model);
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Edit(EditModel model)
        {
            model.AllTags = TagRepository.GetTags().ToList();
                
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Does an entry with that name already exist?
            var existing = EntryRepository.GetEntry(model.Page);
            if (existing != null && existing.Id != model.OriginalEntryId)
            {
                ModelState.AddModelError("PageExists", string.Format("A page with SLUG '{0}' already exists. You should edit that page instead.", model.Page));
                return View(model);
            }

            var entry = EntryRepository.GetEntry(model.OriginalEntryId) ?? new Entry();
            entry.Name = model.Page;
            entry.Title = model.Title ?? string.Empty;
            entry.Summary = model.Sidebar ?? string.Empty;
            entry.MetaTitle = string.IsNullOrWhiteSpace(model.MetaTitle) ? model.Title : model.MetaTitle;
            entry.IsDiscussionEnabled = !model.DisableComments;
            entry.MetaDescription = model.MetaDescription ?? string.Empty;
            entry.HideChrome = model.HideChrome;
            entry.Published = DateTime.Parse(model.PublishDate, CultureInfo.InvariantCulture).ToUniversalTime();
            entry.Status = model.Status;

            var revision = entry.Revise();
            revision.Body = model.Content;
            revision.Reason = model.ChangeSummary ?? string.Empty;
            revision.Format = model.Format;

            EntryRepository.Save(entry);

            entry.Tags.Clear();
            foreach (var tagName in model.TagsString.Split(','))
            {
                int id;
                Tag tag;
                if (int.TryParse(tagName, out id))
                {
                    tag = TagRepository.GetTag(id);
                }
                else
                {
                    tag = new Tag
                              {
                                  Name = tagName
                              };
                    TagRepository.Save(tag);
                }

                entry.Tags.Add(tag);
                TagRepository.Save(tag);
            }

            return RedirectToAction("Page", new { page = model.Page });
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
                return View("Page", model)
                    .AndFlash("Your comment was not posted - please check the validation errors below.");
            }

            var comment = entry.Comment();
            comment.AuthorCompany = string.Empty;
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
            var settings = SettingsProvider.GetSettings();
            if (settings.DisableCommentsOlderThan > 0 && DateTime.UtcNow.AddDays(settings.DisableCommentsOlderThan) > entry.Published)
            {
                comment.IsSpam = true;
            }

            EntryRepository.Save(entry);

            EventPublisher.Publish(new CommentPostedEvent(entry, comment));

            return RedirectToAction("Page", new {page = page})
                .AndFlash("Thanks, your comment has been posted.");
        }

        public virtual ActionResult Revisions(PageName page)
        {
            var entry = EntryRepository.GetEntry(page);
            if (entry == null)
            {
                return RedirectToAction("Edit", new { page = page });
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
