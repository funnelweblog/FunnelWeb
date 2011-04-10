using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Authentication;
using FunnelWeb.Eventing;
using FunnelWeb.Filters;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Model.Strings;
using FunnelWeb.Settings;
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
        public IAuthenticator Authenticator { get; set; }
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
            var revision = entry.Revision(revertToRevision);
            var allTags = TagRepository.GetTags();
            var model = new EditModel(page, entry.Id, allTags)
                            {
                                DisableComments = !entry.IsDiscussionEnabled,
                                Content = revision.Body,
                                Format = revision.Format,
                                HideChrome = entry.HideChrome,
                                Status = entry.Status,
                                MetaDescription = entry.MetaDescription,
                                MetaTitle = entry.MetaTitle,
                                PublishDate = entry.Published.ToLocalTime().ToString("yyyy-MM-dd"),
                                Sidebar = entry.Summary,
                                Title = entry.Title,
                                SelectedTags = entry.Tags,
                                ChangeSummary =
                                    entry.Id == 0
                                        ? "Initial create"
                                        : (revertToRevision == null ? "" : "Reverted to version " + revertToRevision)
                            };

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
			    model.SelectedTags = GetEditTags(model);
				return View(model);
			}

			// Does an entry with that name already exist?
			var existing = EntryRepository.GetEntry(model.Page);
			if (existing != null && existing.Id != model.OriginalEntryId)
			{
                model.SelectedTags = GetEditTags(model);
                ModelState.AddModelError("PageExists", string.Format("A page with SLUG '{0}' already exists. You should edit that page instead.", model.Page));
				return View(model);
			}

		    var author = Authenticator.GetName();

		    var entry = EntryRepository.GetEntry(model.OriginalEntryId) ?? new Entry { Author = author };
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
		    revision.Author = author;
			revision.Body = model.Content;
			revision.Reason = model.ChangeSummary ?? string.Empty;
			revision.Format = model.Format;

            var editTags = GetEditTags(model);
            var toDelete = entry.Tags.Where(t => !editTags.Contains(t)).ToList();
            var toAdd = editTags.Where(t => !entry.Tags.Contains(t)).ToList();

		    foreach (var tag in toDelete)
		        tag.Remove(entry);
            foreach (var tag in toAdd)
            {
                if (tag.Id == 0)
                    TagRepository.Save(tag);
                tag.Add(entry);
            }

			EntryRepository.Save(entry);

		    return RedirectToAction("Page", "Wiki", new { Area = "", page = model.Page});
		}

        private List<Tag> GetEditTags(EditModel model)
        {
            var tagList = new List<Tag>();
            foreach (var tagName in model.TagsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Where(s => s != "0"))
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
                }
                tagList.Add(tag);
            }

            return tagList;
        }
    }
}
