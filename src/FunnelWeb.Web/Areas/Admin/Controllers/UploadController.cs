using System.IdentityModel.Services;
using System.IO;
using System.Security.Permissions;
using System.Web.Mvc;
using FunnelWeb.Authentication.Internal;
using FunnelWeb.Providers.File;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Areas.Admin.Views.Upload;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
	[ValidateInput(false)]
	// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
	public class UploadController : Controller
	{
		public IFileRepository FileRepository { get; set; }
		public ISettingsProvider SettingsProvider { get; set; }

		[Authorize, ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operation.View, Resource = Authorization.Resource.Upload.Index)]
		public virtual ActionResult Index(string path)
		{
			path = path ?? string.Empty;
			if (FileRepository.IsFile(path))
			{
				return RedirectToAction("Index", "Upload", new { path = Path.GetDirectoryName(path) });
			}

			ViewData.Model = new IndexModel(path, FileRepository.GetItems(path))
													 {
														 StorageProvider = SettingsProvider.GetSettings<FunnelWebSettings>().StorageProvider
													 };
			return View();
		}

		[Authorize, ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operation.Update, Resource = Authorization.Resource.Upload.Index)]
		public virtual ActionResult Upload(string path, bool? unzip, FileUpload upload)
		{
			var filePath = Path.Combine(path, upload.FileName);
			FileRepository.Save(upload.Stream, filePath, unzip ?? false);
			return RedirectToAction("Index", "Upload", new { path });
		}

		[Authorize, ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operation.Update, Resource = Authorization.Resource.Upload.CreateDirectory)]
		public virtual ActionResult CreateDirectory(string path, string name)
		{
			FileRepository.CreateDirectory(path, name);
			return RedirectToAction("Index", "Upload", new { path });
		}

		[Authorize, ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operation.Update, Resource = Authorization.Resource.Upload.Move)]
		public virtual ActionResult Move(string path, string oldPath, string newPath)
		{
			FileRepository.Move(oldPath, newPath);
			return RedirectToAction("Index", "Upload", new { path });
		}

		[Authorize, ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operation.Delete, Resource = Authorization.Resource.Upload.Delete)]
		public virtual ActionResult Delete(string path, string filePath)
		{
			FileRepository.Delete(filePath);
			return RedirectToAction("Index", "Upload", new { path });
		}

		public virtual ActionResult Render(string path)
		{
			return FileRepository.Render(path);
		}
	}
}