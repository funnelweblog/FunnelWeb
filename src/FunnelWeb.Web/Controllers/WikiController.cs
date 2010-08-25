using System;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Web.Application;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Application.HealthMonitoring.Events;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Repositories;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Controllers
{
    [ValidateInput(false), Transactional]
    [HandleError]
    public partial class WikiController : Controller
    {
        private const int ItemsPerPage = 30;
        private readonly IEntryRepository _entryRepository;
        private readonly IFeedRepository _feedRepository;
        private readonly ISpamChecker _spamChecker;

        public WikiController(IEntryRepository entryRepository, IFeedRepository feedRepository, ISpamChecker spamChecker)
        {
            _entryRepository = entryRepository;
            _feedRepository = feedRepository;
            _spamChecker = spamChecker;
        }

        public virtual ActionResult Recent(int pageNumber)
        {
            var feed = _feedRepository.GetFeeds().OrderBy(f => f.Id).First().Name;

            var entries = _feedRepository.GetFeed(feed, pageNumber * ItemsPerPage, ItemsPerPage);
            var totalItems = _feedRepository.GetFeedCount(feed);
            ViewData.Model = new RecentModel(entries, pageNumber, (int)((decimal)totalItems / ItemsPerPage + 1));
            return View();
        }

        public virtual ActionResult Search([Bind(Prefix = "q")] string searchText)
        {
            var results = _entryRepository.Search(searchText);
            ViewData.Model = new NotFoundModel(searchText, false, results);
            return View("NotFound");
        }

        public virtual ActionResult NotFound(string searchText)
        {
            var redirect = _entryRepository.GetClosestRedirect(HttpContext.Request.Url.AbsolutePath);
            if (redirect != null)
            {
                return redirect.To.StartsWith("http") 
                    ? Redirect(redirect.To) 
                    : Redirect("~/" + redirect.To);
            }

            new EntryNotFoundEvent(HttpContext.Request.Url.OriginalString, this).Raise();
            var results = _entryRepository.Search(searchText);
            ViewData.Model = new NotFoundModel(searchText, true, results);
            return View("NotFound");
        }

        public virtual ActionResult Page(PageName page, int revision)
        {
            var entry = _entryRepository.GetEntry(page, revision);
            if (entry == null)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Edit", new { page = page });
                }
                return NotFound(page);
            }

            ViewData.Model = new PageModel(page, entry, revision > 0);
            return View();
        }
        
        [Authorize]
        public virtual ActionResult New()
        {
            var feeds = _feedRepository.GetFeeds();
            var entry = new Entry() { Title = "Enter a Title", MetaTitle = "Enter a meta title", Name = "" };
            ViewData.Model = new EditModel("", entry, true, feeds);
            return View("Edit");
        }

        [Authorize]
        public virtual ActionResult Edit(PageName page)
        {
            var entry = _entryRepository.GetEntry(page) ?? new Entry() { Title = page, MetaTitle = page, Name = page};
            var feeds = _feedRepository.GetFeeds();
            ViewData.Model = new EditModel(page, entry, entry.Id == 0, feeds);
            return View();
        }

        [Authorize]
        public virtual ActionResult Unpublished()
        {
            var allPosts = _entryRepository.GetUnpublished();
            ViewData.Model = new RecentModel(allPosts, 1, 1);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize]
        public virtual ActionResult Save(PageName page, string title, string metaTitle, string summary, string body, string comment, string metaDescription, string metaKeywords, bool enableDiscussion, int[] feeds)
        {
            var entry = _entryRepository.GetEntry(page) ?? new Entry();
            entry.Name = page;
            entry.Title = title;
            entry.Summary = summary;
            entry.MetaTitle = metaTitle;
            entry.IsDiscussionEnabled = enableDiscussion;
            entry.MetaDescription = metaDescription;
            entry.MetaKeywords = metaKeywords;

            var revision = entry.Revise();
            revision.Body = body;
            revision.Reason = comment;

            _entryRepository.Save(entry);

            foreach (var feed in _feedRepository.GetFeeds())
            {
                if (!feeds.Contains(feed.Id)) continue;

                feed.Publish(entry);
                _feedRepository.Save(feed);
            }

            return RedirectToAction("Page", new { page = page });
        }

        public virtual ActionResult Comment(PageName page, string name, string url, string email, string comments)
        {
            var entry = _entryRepository.GetEntry(page);
            if (entry == null) return RedirectToAction("Recent");

            var comment = entry.Comment();
            comment.AuthorCompany = string.Empty;
            comment.AuthorEmail = email;
            comment.AuthorName = name;
            comment.AuthorUrl = url;
            comment.Body = comments;
            _spamChecker.Verify(comment);
            _entryRepository.Save(entry);
            new CommentPostedEvent(page, comment.IsSpam, comment.Body, this).Raise();

            return RedirectToAction("Page", new { page = page });
        }

        public virtual ActionResult Revisions(PageName page)
        {
            var entry = _entryRepository.GetEntry(page);
            if (entry == null)
            {
                return RedirectToAction("Edit", new { page = page });
            }

            ViewData.Model = new RevisionsModel(page, entry);
            return View();
        }

        public virtual ActionResult SiteMap()
        {
            var allPosts = _entryRepository.GetEntries().OrderBy(x => x.Published).ToList();
            ViewData.Model = new SiteMapModel(allPosts);
            return View();
        }
    }
}
