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
using FunnelWeb.Repositories;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Application.Spam;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
    [FunnelWebRequest]
    [HandleError]
    [ValidateInput(false)]
    public class WikiAdminController : Controller
    {
        public IAuthenticator Authenticator { get; set; }
        public IRepository Repository { get; set; }
        public ITagRepository TagRepository { get; set; }
        public IFeedRepository FeedRepository { get; set; }
        public ISpamChecker SpamChecker { get; set; }
        public IEventPublisher EventPublisher { get; set; }
        public ISettingsProvider SettingsProvider { get; set; }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult Edit(PageName page, int? revertToRevision)
        {
            //Keep this call before the call to entry repository, they will be issued in a single query
            var allTags = TagRepository.GetTags();

            var entry =
                revertToRevision == null
                    ? Repository.FindFirstOrDefault(new EntryByNameQuery(page))
                    : Repository.FindFirstOrDefault(new EntryByNameAndRevisionQuery(page, revertToRevision.Value));

            if (entry == null)
            {
                entry = new EntryRevision
                            {
                                Title = "New post",
                                MetaTitle = "New post",
                                Name = page
                            };
            }

            entry.ChangeSummary = entry.Id == 0
                                      ? "Initial create"
                                      : (revertToRevision == null ? "" : "Reverted to version " + revertToRevision);
            entry.AllTags = allTags;

            if (revertToRevision != null)
            {
                return View("Edit", entry).AndFlash("You are editing an old version of this page. This will become the current version when you save.");
            }
            return View("Edit", entry);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public virtual ActionResult Edit(EntryRevision model)
		{
			model.AllTags = TagRepository.GetTags().ToList();

			if (!ModelState.IsValid)
			{
			    model.SelectedTags = GetEditTags(model);
				return View(model);
			}

			// Does an entry with that name already exist?
            var existing = Repository.FindFirstOrDefault(new EntryByNameQuery(model.Name));
			if (existing != null && existing.Id != model.Id)
			{
                model.SelectedTags = GetEditTags(model);
                ModelState.AddModelError("PageExists", string.Format("A page with SLUG '{0}' already exists. You should edit that page instead.", model.Title));
				return View(model);
			}

		    var author = Authenticator.GetName();

		    var entry = Repository.Get<Entry>(model.Id) ?? new Entry { Author = author };

            entry.Name = model.Title;
			entry.PageTemplate = string.IsNullOrEmpty(model.PageTemplate) ? null : model.PageTemplate;
			entry.Title = model.Title ?? string.Empty;
			entry.Summary = model.Summary ?? string.Empty;
			entry.MetaTitle = string.IsNullOrWhiteSpace(model.MetaTitle) ? model.Title : model.MetaTitle;
			entry.IsDiscussionEnabled = !model.DisableComments;
			entry.MetaDescription = model.MetaDescription ?? string.Empty;
			entry.HideChrome = model.HideChrome;
			entry.Published = DateTime.Parse(model.PublishDate, CultureInfo.InvariantCulture).ToUniversalTime();
			entry.Status = model.Status;

			var revision = entry.Revise();
		    revision.Author = author;
			revision.Body = model.Body;
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

            if (model.IsNew)
                Repository.Add(entry);

		    return RedirectToAction("Page", "Wiki", new { Area = "", page = model.Name});
		}

        private List<Tag> GetEditTags(EntryRevision model)
        {
            var tagList = new List<Tag>();
            foreach (var tagName in model.TagsCommaSeparated.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Where(s => s != "0"))
            {
                int id;
                var tag = int.TryParse(tagName, out id) ? TagRepository.GetTag(id) : new Tag {Name = tagName};
                tagList.Add(tag);
            }

            return tagList;
        }
    }
}
