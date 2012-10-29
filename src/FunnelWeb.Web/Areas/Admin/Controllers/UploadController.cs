using System.IO;
using System.Web.Mvc;
using FunnelWeb.Providers.File;
using FunnelWeb.Settings;
using FunnelWeb.Utilities;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Areas.Admin.Views.Upload;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    public class UploadController : Controller
    {
        public IFileRepository FileRepository { get; set; }
        public IMimeTypeLookup MimeHelper { get; set; }
        public ISettingsProvider SettingsProvider { get; set; }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult Index(string path)
        {
            path = path ?? string.Empty;
            if (FileRepository.IsFile(path))
            {
                return RedirectToAction("Index", "Upload", new {path = Path.GetDirectoryName(path)});
            }

            ViewData.Model = new IndexModel(path, FileRepository.GetItems(path))
                                 {
                                     StorageProvider = SettingsProvider.GetSettings<FunnelWebSettings>().StorageProvider
                                 };
            return View();
        }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult Upload(string path, bool? unzip, FileUpload upload)
        {
            var filePath = Path.Combine(path, upload.FileName);
            FileRepository.Save(upload.Stream, filePath, unzip ?? false);
            return RedirectToAction("Index", "Upload", new {path});
        }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult CreateDirectory(string path, string name)
        {
            FileRepository.CreateDirectory(path, name);
            return RedirectToAction("Index", "Upload", new {path});
        }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult Move(string path, string oldPath, string newPath)
        {
            FileRepository.Move(oldPath, newPath);
            return RedirectToAction("Index", "Upload", new {path});
        }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult Delete(string path, string filePath)
        {
            FileRepository.Delete(filePath);
            return RedirectToAction("Index", "Upload", new {path});
        }

        public virtual ActionResult Render(string path)
        {
            return FileRepository.Render(path);
        }
    }
}