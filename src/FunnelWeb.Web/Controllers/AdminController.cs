using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Web.Application;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Model.Repositories;
using FunnelWeb.Web.Model;

namespace FunnelWeb.Web.Controllers
{
    [ValidateInput(false), Transactional]
    public partial class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IFeedRepository _feedRepository;

        public AdminController(IAdminRepository adminRepository, IFeedRepository feedRepository)
        {
            _adminRepository = adminRepository;
            _feedRepository = feedRepository;
        }

        [Authorize]
        public ActionResult Index()
        {
            var settings = _adminRepository.GetSettings();
            var feeds = _feedRepository.GetFeeds();
            var comments = _adminRepository.GetComments(0, 30);
            var redirects = _adminRepository.GetRedirects();
            var pingbacks = _adminRepository.GetPingbacks();
            ViewData.Model = new IndexModel(settings, feeds, comments, pingbacks, redirects);
            return View("Index");
        }

        [Authorize]
        public ActionResult CreateFeed(string name, string title)
        {
            var feed = new Feed { Name = name, Title = title };
            _feedRepository.Save(feed);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult DeleteFeed(int feedId)
        {
            var feed = _feedRepository.GetFeeds().FirstOrDefault(x => x.Id == feedId);
            if (feed != null)
            {
                _feedRepository.Delete(feed);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult DeleteRedirect(int redirectId)
        {
            var redirect = _adminRepository.GetRedirects().FirstOrDefault(x => x.Id == redirectId);
            if (redirect != null)
            {
                _adminRepository.Delete(redirect);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult CreateRedirect(string from, string to)
        {
            var redirect = new Redirect();
            redirect.From = from;
            redirect.To = to;
            _adminRepository.Save(redirect);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult UpdateSettings(Dictionary<string, string> settings)
        {
            var previousSettings = _adminRepository.GetSettings();
            foreach (var setting in previousSettings)
            {
                if (settings.ContainsKey(setting.Name))
                {
                    setting.Value = settings[setting.Name];
                }
            }
            _adminRepository.Save(previousSettings);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult DeleteComment(int comment)
        {
            var item = _adminRepository.GetComment(comment);
            _adminRepository.Delete(item);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult DeleteAllSpam()
        {
            var comments = _adminRepository.GetComments(0, 30).ToList().Where(x => x.IsSpam);
            foreach (var comment in comments) _adminRepository.Delete(comment);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult DeletePingback(int pingback)
        {
            var item = _adminRepository.GetPingback(pingback);
            _adminRepository.Delete(item);
            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult ToggleSpam(int comment)
        {
            var item = _adminRepository.GetComment(comment);
            item.IsSpam = !item.IsSpam;
            _adminRepository.Save(item);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult TogglePingbackSpam(int pingback)
        {
            var item = _adminRepository.GetPingback(pingback);
            item.IsSpam = !item.IsSpam;
            _adminRepository.Save(item);
            return RedirectToAction("Index");
        }
    }
}
