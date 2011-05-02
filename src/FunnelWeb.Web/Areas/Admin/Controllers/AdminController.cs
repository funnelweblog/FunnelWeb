using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Filters;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Settings;
using FunnelWeb.Tasks;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Application.Themes;
using FunnelWeb.Web.Areas.Admin.Views.Admin;
using NHibernate;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
    [FunnelWebRequest]
    [ValidateInput(false)]
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        public IAdminRepository AdminRepository { get; set; }
        public ITagRepository FeedRepository { get; set; }
        public ISettingsProvider SettingsProvider { get; set; }
        public IThemeProvider ThemeProvider { get; set; }
        public IEntryRepository EntryRepository { get; set; }
        public ITaskStateRepository TaskRepository { get; set; }
        public ISession DatabaseSession { get; set; }
        public ITaskExecutor<BlogMLImportTask> ImportTask { get; set; }

        public virtual ActionResult Index()
        {
            return View(new IndexModel());
        }

        #region Settings

        public virtual ActionResult Settings()
        {
            var settings = SettingsProvider.GetSettings<FunnelWebSettings>();
            ViewBag.Themes = ThemeProvider.GetThemes();
            return View(settings);
        }

        [HttpPost]
        public virtual ActionResult Settings(FunnelWebSettings settings)
        {
            ViewBag.Themes = ThemeProvider.GetThemes();
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

        public virtual ActionResult Comments(int? pageNumber)
        {
            var commentCount = (int)Math.Ceiling(AdminRepository.GetComments(0, int.MaxValue).Count()/20.00);
            var comments = AdminRepository.GetComments((pageNumber??0) * 20, 20);
            return View(new CommentsModel((pageNumber ?? 0), commentCount, comments));
        }

        public virtual ActionResult DeleteComment(int id)
        {
            var item = AdminRepository.GetComment(id);
            AdminRepository.Delete(item);
            return RedirectToAction("Comments", "Admin");
        }

        public virtual ActionResult DeleteAllSpam()
        {
            var comments = AdminRepository.GetSpam().ToList();
            foreach (var comment in comments)
                AdminRepository.Delete(comment);
            return RedirectToAction("Comments", "Admin");
        }

        public virtual ActionResult ToggleSpam(int id)
        {
            var item = AdminRepository.GetComment(id);
            if (item != null)
            {
                item.IsSpam = !item.IsSpam;
                AdminRepository.Save(item);
            }
            return RedirectToAction("Comments", "Admin");
        }

        #endregion

        #region Pingbacks

        public virtual ActionResult Pingbacks()
        {
            var pingbacks = AdminRepository.GetPingbacks();
            return View(new PingbacksModel(pingbacks));
        }

        public virtual ActionResult DeletePingback(int id)
        {
            var item = AdminRepository.GetPingback(id);
            AdminRepository.Delete(item);
            return RedirectToAction("Pingbacks", "Admin");
        }

        public virtual ActionResult TogglePingbackSpam(int id)
        {
            var item = AdminRepository.GetPingback(id);
            if (item != null)
            {
                item.IsSpam = !item.IsSpam;
                AdminRepository.Save(item);
            }
            return RedirectToAction("Pingbacks", "Admin");
        }

        #endregion

        #region Tasks

        public virtual ActionResult Tasks()
        {
            var tasks = TaskRepository.GetAll().OrderByDescending(x => x.Started);
            return View("Tasks", new TasksModel(tasks.ToList()));
        }

        public virtual ActionResult Task(int id)
        {
            var task = TaskRepository.Get(id);
            return View("Task", new TaskModel(task));
        }

        #endregion

        #region Import

        public virtual ActionResult BlogMLImport()
        {
            return View(new BlogMLImportModel());
        }

        [HttpPost]
        public virtual ActionResult BlogMLImport(FileUpload upload)
        {
            if (upload == null || string.IsNullOrWhiteSpace(upload.FileName))
            {
                ModelState.AddModelError("File", "Please select a file to upload.");
                return View();
            }

            var fullPath = Server.MapPath(SettingsProvider.GetSettings<FunnelWebSettings>().UploadPath);
            fullPath = Path.Combine(fullPath, upload.FileName);
            upload.SaveTo(fullPath);

            var id = ImportTask.Execute(new { inputFile = fullPath });
            return RedirectToAction("Task", new { id });
        }

        #endregion

        public virtual ActionResult PageList(EntriesSortColumn? sort, bool? asc)
        {
            if (sort == null)
                sort = EntriesSortColumn.Slug;

            var entries = new GetEntriesQuery(sort.Value, asc ?? true).Execute(DatabaseSession);

            return View(new PageListModel(entries) { SortAscending = asc.GetValueOrDefault() });
        }

        public virtual ActionResult DeletePage(int id)
		{
			EntryRepository.Delete(id);
			return RedirectToAction("PageList");
		}
    }
}
