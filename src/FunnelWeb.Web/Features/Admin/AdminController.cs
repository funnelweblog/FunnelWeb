using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Features.Admin.Views;
using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Repositories;

namespace FunnelWeb.Web.Features.Admin
{
    [Transactional]
    public partial class AdminController : Controller
    {
        public IAdminRepository AdminRepository { get; set; }
        public IFeedRepository FeedRepository { get; set; }

        [Authorize]
        public virtual ActionResult Index()
        {
            var settings = AdminRepository.GetSettings();
            var feeds = FeedRepository.GetFeeds();
            var comments = AdminRepository.GetComments(0, 30);
            var redirects = AdminRepository.GetRedirects();
            var pingbacks = AdminRepository.GetPingbacks();

            var themeFolder = new DirectoryInfo(Server.MapPath("~/Content/Styles/Themes"));
            var themes = themeFolder.GetDirectories().Select(x => x.Name).OrderBy(x => x);
            ViewData.Model = new IndexModel(settings, feeds, comments, pingbacks, redirects, themes);
            return View();
        }

        [Authorize]
        public virtual ActionResult CreateFeed(string name, string title)
        {
            var feed = new Feed { Name = name, Title = title };
            FeedRepository.Save(feed);
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult DeleteFeed(int feedId)
        {
            var feed = FeedRepository.GetFeeds().FirstOrDefault(x => x.Id == feedId);
            if (feed != null)
            {
                FeedRepository.Delete(feed);
            }
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult DeleteRedirect(int redirectId)
        {
            var redirect = AdminRepository.GetRedirects().FirstOrDefault(x => x.Id == redirectId);
            if (redirect != null)
            {
                AdminRepository.Delete(redirect);
            }
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult CreateRedirect(string from, string to)
        {
            var redirect = new Redirect();
            redirect.From = from;
            redirect.To = to;
            AdminRepository.Save(redirect);
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult UpdateSettings(Dictionary<string, string> settings)
        {
            var previousSettings = AdminRepository.GetSettings();
            foreach (var setting in previousSettings)
            {
                if (settings.ContainsKey(setting.Name))
                    setting.Value = settings[setting.Name];
            }
            AdminRepository.Save(previousSettings);

            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult DeleteComment(int comment)
        {
            var item = AdminRepository.GetComment(comment);
            AdminRepository.Delete(item);
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult DeleteAllSpam()
        {
            var comments = AdminRepository.GetSpam().ToList();
            foreach (var comment in comments) 
                AdminRepository.Delete(comment);
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult DeletePingback(int pingback)
        {
            var item = AdminRepository.GetPingback(pingback);
            AdminRepository.Delete(item);
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }


        [Authorize]
        public virtual ActionResult ToggleSpam(int comment)
        {
            var item = AdminRepository.GetComment(comment);
            if (item != null)
            {
                item.IsSpam = !item.IsSpam;
                AdminRepository.Save(item); 
            }
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        public virtual ActionResult TogglePingbackSpam(int pingback)
        {
            var item = AdminRepository.GetPingback(pingback);
            if (item != null)
            {
                item.IsSpam = !item.IsSpam;
                AdminRepository.Save(item);
            }
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }
    }
}
