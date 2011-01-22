using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Views.Admin;

namespace FunnelWeb.Web.Controllers
{
    [FunnelWebRequest]
    [ValidateInput(false)]
    public class AdminController : Controller
    {
        public IAdminRepository AdminRepository { get; set; }
        public ITagRepository FeedRepository { get; set; }
        public ISettingsProvider SettingsProvider { get; set; }
        public IEntryRepository EntryRepository { get; set; }
        public ITaskStateRepository TaskRepository { get; set; }

        [Authorize]
        public virtual ActionResult Index()
        {
            return View(new IndexModel());
        }

        #region Settings

        [Authorize]
        public virtual ActionResult Settings()
        {
            var settings = SettingsProvider.GetSettings();
            return View(settings);
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult Settings(Settings.Settings settings)
        {
            settings.Themes = SettingsProvider.GetSettings().Themes;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Your settings could not be saved. Please fix the errors shown below.");
                return View(settings);
            }
            
            SettingsProvider.SaveSettings(settings);
            
            return RedirectToAction("Settings", "Admin")
                .AndFlash("Your changes have been saved");
        }

        #endregion

        #region Comments

        [Authorize]
        public virtual ActionResult Comments()
        {
            var comments = AdminRepository.GetComments(0, 50);
            return View(new CommentsModel(comments));
        }

        [Authorize]
        public virtual ActionResult DeleteComment(int comment)
        {
            var item = AdminRepository.GetComment(comment);
            AdminRepository.Delete(item);
            return RedirectToAction("Comments", "Admin");
        }

        [Authorize]
        public virtual ActionResult DeleteAllSpam()
        {
            var comments = AdminRepository.GetSpam().ToList();
            foreach (var comment in comments) 
                AdminRepository.Delete(comment);
            return RedirectToAction("Comments", "Admin");
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
            return RedirectToAction("Comments", "Admin");
        }

        #endregion

        #region Pingbacks

        [Authorize]
        public virtual ActionResult Pingbacks()
        {
            var pingbacks = AdminRepository.GetPingbacks();
            return View(new PingbacksModel(pingbacks));
        }

        [Authorize]
        public virtual ActionResult DeletePingback(int pingback)
        {
            var item = AdminRepository.GetPingback(pingback);
            AdminRepository.Delete(item);
            return RedirectToAction("Pingbacks", "Admin");
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
            return RedirectToAction("Pingbacks", "Admin");
        }

        #endregion

        #region Tasks

        [Authorize]
        public virtual ActionResult Tasks()
        {
            var tasks = TaskRepository.GetAll().OrderByDescending(x => x.Started);
            return View("Tasks", new TasksModel(tasks.ToList()));
        }

        [Authorize]
        public virtual ActionResult Task(int id)
        {
            var task = TaskRepository.Get(id);
            return View("Task", new TaskModel(task));
        }

        #endregion

        [Authorize]
        public virtual ActionResult PageList()
        {
            var entries = EntryRepository.GetEntries().ToList();
            return View(new PageListModel(entries));
        }
    }
}
