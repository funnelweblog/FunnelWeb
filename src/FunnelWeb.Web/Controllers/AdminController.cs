using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Repositories;

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
        public virtual ActionResult Index()
        {
            var settings = _adminRepository.GetSettings();
            var feeds = _feedRepository.GetFeeds();
            var comments = _adminRepository.GetComments(0, 30);
            var redirects = _adminRepository.GetRedirects();
            var pingbacks = _adminRepository.GetPingbacks();

            var themeFolder = new DirectoryInfo(Server.MapPath("~/Content/Styles/Themes"));
            var themes = themeFolder.GetDirectories().Select(x => x.Name).OrderBy(x => x);
            ViewData.Model = new IndexModel(settings, feeds, comments, pingbacks, redirects, themes);
            return View();
        }

        [Authorize]
        public virtual ActionResult CreateFeed(string name, string title)
        {
            var feed = new Feed { Name = name, Title = title };
            _feedRepository.Save(feed);
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult DeleteFeed(int feedId)
        {
            var feed = _feedRepository.GetFeeds().FirstOrDefault(x => x.Id == feedId);
            if (feed != null)
            {
                _feedRepository.Delete(feed);
            }
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult DeleteRedirect(int redirectId)
        {
            var redirect = _adminRepository.GetRedirects().FirstOrDefault(x => x.Id == redirectId);
            if (redirect != null)
            {
                _adminRepository.Delete(redirect);
            }
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult CreateRedirect(string from, string to)
        {
            var redirect = new Redirect();
            redirect.From = from;
            redirect.To = to;
            _adminRepository.Save(redirect);
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult UpdateSettings(Dictionary<string, string> settings)
        {
            var previousSettings = _adminRepository.GetSettings();
            foreach (var setting in previousSettings)
            {
                if (settings.ContainsKey(setting.Name))
                    setting.Value = settings[setting.Name];
            }
            _adminRepository.Save(previousSettings);

            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult DeleteComment(int comment)
        {
            var item = _adminRepository.GetComment(comment);
            _adminRepository.Delete(item);
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult DeleteAllSpam()
        {
            var comments = _adminRepository.GetSpam().ToList();
            foreach (var comment in comments) _adminRepository.Delete(comment);
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult DeletePingback(int pingback)
        {
            var item = _adminRepository.GetPingback(pingback);
            _adminRepository.Delete(item);
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }


        [Authorize]
        public virtual ActionResult ToggleSpam(int comment)
        {
            var item = _adminRepository.GetComment(comment);
            if (item != null)
            {
                item.IsSpam = !item.IsSpam;
                _adminRepository.Save(item); 
            }
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult TogglePingbackSpam(int pingback)
        {
            var item = _adminRepository.GetPingback(pingback);
            if (item != null)
            {
                item.IsSpam = !item.IsSpam;
                _adminRepository.Save(item);
            }
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }
    }
}
