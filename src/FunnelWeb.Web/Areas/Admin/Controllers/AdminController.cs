using System;
using System.IdentityModel.Services;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web.Mvc;
using FunnelWeb.Authentication.Internal;
using FunnelWeb.Filters;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Providers;
using FunnelWeb.Providers.File;
using FunnelWeb.Repositories;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Settings;
using FunnelWeb.Tasks;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Application.Themes;
using FunnelWeb.Web.Areas.Admin.Views.Admin;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
	[Authorize]
	[FunnelWebRequest]
	[ValidateInput(false)]
	// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
	public class AdminController : Controller
	{
		// ReSharper disable UnusedAutoPropertyAccessor.Global
		public IFederatedAuthenticationConfigurator FederatedAuthenticationConfigurator { get; set; }
		public IAdminRepository AdminRepository { get; set; }
		public ISettingsProvider SettingsProvider { get; set; }
		public IThemeProvider ThemeProvider { get; set; }
		public IRepository Repository { get; set; }
		public ITaskStateRepository TaskRepository { get; set; }
		public ITaskExecutor<BlogMLImportTask> ImportTask { get; set; }
		public Func<IProviderInfo<IFileRepository>> FileRepositoriesInfo { get; set; }
		// ReSharper restore UnusedAutoPropertyAccessor.Global

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.View, Resource = Authorization.Resources.Admin.Index)]
		public virtual ActionResult Index()
		{
			return View(new IndexModel());
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.View, Resource = Authorization.Resources.Admin.Settings)]
		public virtual ActionResult Settings()
		{
			var settings = SettingsProvider.GetSettings<FunnelWebSettings>();
			ViewBag.Themes = ThemeProvider.GetThemes();
			ViewBag.FileRepositories = FileRepositoriesInfo().Keys;
			return View(settings);
		}

		[HttpPost]
		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.Update, Resource = Authorization.Resources.Admin.Settings)]
		public virtual ActionResult Settings(FunnelWebSettings settings)
		{
			ViewBag.Themes = ThemeProvider.GetThemes();
			ViewBag.FileRepositories = FileRepositoriesInfo().Keys;
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Your settings could not be saved. Please fix the errors shown below.");
				return View(settings);
			}

			SettingsProvider.SaveSettings(settings);

			return RedirectToAction("Settings", "Admin").AndFlash("Your changes have been saved");
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.View, Resource = Authorization.Resources.Admin.AcsSettings)]
		public virtual ActionResult AcsSettings()
		{
			var settings = SettingsProvider.GetSettings<AccessControlServiceSettings>();
			ViewBag.Themes = ThemeProvider.GetThemes();
			ViewBag.FileRepositories = FileRepositoriesInfo().Keys;
			return View(settings);
		}

		[HttpPost]
		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.Update, Resource = Authorization.Resources.Admin.AcsSettings)]
		public virtual ActionResult AcsSettings(AccessControlServiceSettings acsSettings)
		{
			ViewBag.Themes = ThemeProvider.GetThemes();
			ViewBag.FileRepositories = FileRepositoriesInfo().Keys;
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Your settings could not be saved. Please fix the errors shown below.");
				return View(acsSettings);
			}

			SettingsProvider.SaveSettings(acsSettings);

			FederatedAuthenticationConfigurator.InitiateFederatedAuthentication(acsSettings);

			return RedirectToAction("AcsSettings").AndFlash("Your changes have been saved");
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.View, Resource = Authorization.Resources.Admin.Comments)]
		public virtual ActionResult Comments(int? pageNumber)
		{
			var page = pageNumber ?? 0;

			var comments = Repository.Find(new GetAllCommentsQuery(), page, 20);
			return View(new CommentsModel(page, comments));
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.Delete, Resource = Authorization.Resources.Admin.Comment)]
		public virtual ActionResult DeleteComment(int id)
		{
			var item = Repository.Get<Comment>(id);
			Repository.Remove(item);
			AdminRepository.UpdateCommentCountFor(item.Entry.Id);
			return RedirectToAction("Comments", "Admin");
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.Delete, Resource = Authorization.Resources.Admin.AllSpam)]
		public virtual ActionResult DeleteAllSpam()
		{
			var comments = Repository.Find(new GetSpamQuery()).ToList();
			foreach (var comment in comments)
				Repository.Remove(comment);
			foreach (var entryToUpdate in comments.Select(c => c.Entry.Id).GroupBy(id => id))
			{
				AdminRepository.UpdateCommentCountFor(entryToUpdate.Key);
			}
			return RedirectToAction("Comments", "Admin");
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.Update, Resource = Authorization.Resources.Admin.Spam)]
		public virtual ActionResult ToggleSpam(int id)
		{
			var item = Repository.Get<Comment>(id);
			if (item != null)
			{
				item.IsSpam = !item.IsSpam;
				AdminRepository.UpdateCommentCountFor(item.Entry.Id);
			}
			return RedirectToAction("Comments", "Admin");
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.View, Resource = Authorization.Resources.Admin.Pingbacks)]
		public virtual ActionResult Pingbacks()
		{
			var pingbacks = Repository.FindAll<Pingback>();
			return View(new PingbacksModel(pingbacks));
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.Delete, Resource = Authorization.Resources.Admin.Pingback)]
		public virtual ActionResult DeletePingback(int id)
		{
			var item = Repository.Get<Pingback>(id);
			Repository.Remove(item);
			return RedirectToAction("Pingbacks", "Admin");
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.Update, Resource = Authorization.Resources.Admin.Pingback)]
		public virtual ActionResult TogglePingbackSpam(int id)
		{
			var item = Repository.Get<Pingback>(id);
			if (item != null)
			{
				item.IsSpam = !item.IsSpam;
			}
			return RedirectToAction("Pingbacks", "Admin");
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.View, Resource = Authorization.Resources.Admin.Tasks)]
		public virtual ActionResult Tasks()
		{
			var tasks = TaskRepository.GetAll().OrderByDescending(x => x.Started);
			return View("Tasks", new TasksModel(tasks.ToList()));
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.View, Resource = Authorization.Resources.Admin.Task)]
		public virtual ActionResult Task(int id)
		{
			var task = TaskRepository.Get(id);
			return View("Task", new TaskModel(task));
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.View, Resource = Authorization.Resources.Admin.BlogMl)]
		public virtual ActionResult BlogMlImport()
		{
			return View(new BlogMLImportModel());
		}

		[HttpPost]
		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.Update, Resource = Authorization.Resources.Admin.BlogMl)]
		public virtual ActionResult BlogMlImport(FileUpload upload)
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

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.View, Resource = Authorization.Resources.Admin.Pages)]
		public virtual ActionResult PageList(EntriesSortColumn? sort, bool? asc)
		{
			if (sort == null)
				sort = EntriesSortColumn.Slug;

			var entries = Repository.Find(new GetEntriesQuery(EntryStatus.All, sort.Value, asc ?? true), 0, 500);

			return View(new PageListModel(entries) { SortAscending = asc.GetValueOrDefault() });
		}
	}
}