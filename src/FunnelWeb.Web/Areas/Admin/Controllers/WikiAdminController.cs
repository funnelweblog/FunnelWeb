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
using FunnelWeb.Web.Areas.Admin.Views.WikiAdmin;
using RecentModel = FunnelWeb.Web.Views.Wiki.RecentModel;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
    [FunnelWebRequest]
    [HandleError]
    [ValidateInput(false)]
    public class WikiAdminController : Controller
    {
        private const int ItemsPerPage = 30;
        public IEntryRepository EntryRepository { get; set; }
        public ITagRepository TagRepository { get; set; }
        public IFeedRepository FeedRepository { get; set; }
        public ISpamChecker SpamChecker { get; set; }
        public IEventPublisher EventPublisher { get; set; }
        public ISettingsProvider SettingsProvider { get; set; }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult Unpublished()
        {
            var allPosts = EntryRepository.GetUnpublished();
            return View("Recent", new RecentModel("Unpublished Posts", allPosts, 1, 1, "Unpublished"));
        }

        [Authorize(Roles = "Moderator")]
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
            model.ChangeSummary = entry.Id == 0 ? "Initial create" : (revertToRevision == null ? "" : "Reverted to version " + revertToRevision);

            if (revertToRevision != null)
            {
                return View("Edit", model).AndFlash("You are editing an old version of this page. This will become the current version when you save.");
            }
            return View("Edit", model);
        }

		[HttpPost]
        [Authorize(Roles = "Moderator")]
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
			entry.PageTemplate = string.IsNullOrEmpty(model.PageTemplate) ? null : model.PageTemplate;
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
			foreach (var tagName in model.TagsString.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
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

			return RedirectToAction("Page", "Wiki", new { Area = "", page = model.Page});
		}
    }
}
