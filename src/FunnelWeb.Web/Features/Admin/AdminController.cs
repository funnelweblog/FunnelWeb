using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Application.Settings;
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
        public ISettingsProvider SettingsProvider { get; set; }

        [Authorize]
        public virtual ActionResult Index()
        {
            return View(new IndexModel());
        }

        [Authorize]
        public virtual ActionResult Settings()
        {
            var settings = SettingsProvider.GetSettings();
            return View(settings);
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult Settings(Settings settings)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Your settings could not be saved. Please fix the errors shown below.");
                return View(settings);
            }
            
            SettingsProvider.SaveSettings(settings);
            
            return RedirectToAction(FunnelWebMvc.Admin.Index())
                .AndFlash("Your changes have been saved");
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult CreateFeed(string name, string title)
        {
            var feed = new Feed { Name = name, Title = title };
            FeedRepository.Save(feed);
            return RedirectToAction(FunnelWebMvc.Admin.Index());
        }

        [Authorize]
        [HttpPost]
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
        [HttpPost]
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
        [HttpPost]
        public virtual ActionResult CreateRedirect(string from, string to)
        {
            var redirect = new Redirect();
            redirect.From = from;
            redirect.To = to;
            AdminRepository.Save(redirect);
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
