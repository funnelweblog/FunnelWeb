using System.IO;
using System.Web.Mvc;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Web.Application;
using FunnelWeb.Web.Application.Mime;
using FunnelWeb.Web.Features.Upload.Views;

namespace FunnelWeb.Web.Features.Upload
{
    [ValidateInput(false)]
    public partial class UploadController : Controller
    {
        public IFileRepository FileRepository { get; set; }
        public IMimeTypeLookup MimeHelper { get; set; }

        [Authorize]
        public virtual ActionResult Index(string path)
        {
            path = path ?? string.Empty;
            if (FileRepository.IsFile(path))
            {
                return RedirectToAction(FunnelWebMvc.Upload.Index(Path.GetDirectoryName(path)));
            }

            ViewData.Model = new IndexModel(path, FileRepository.GetItems(path));
            return View();
        }

        [Authorize]
        public virtual ActionResult Upload(string path, FileUpload upload)
        {
            var fullPath = FileRepository.MapPath(Path.Combine(path, upload.FileName));
            upload.SaveTo(fullPath);
            return RedirectToAction(FunnelWebMvc.Upload.Index(path));
        }

        [Authorize]
        public virtual ActionResult CreateDirectory(string path, string name)
        {
            FileRepository.CreateDirectory(path, name);
            return RedirectToAction(FunnelWebMvc.Upload.Index(path));
        }

        [Authorize]
        public virtual ActionResult Move(string path, string oldPath, string newPath)
        {
            FileRepository.Move(oldPath, newPath);
            return RedirectToAction(FunnelWebMvc.Upload.Index(path));
        }

        [Authorize]
        public virtual ActionResult Delete(string path, string filePath)
        {
            FileRepository.Delete(filePath);
            return RedirectToAction(FunnelWebMvc.Upload.Index(path));
        }

        public virtual ActionResult Render(string path)
        {
            if (FileRepository.IsFile(path))
            {
                var fullPath = FileRepository.MapPath(path);
                return File(fullPath, MimeHelper.GetMimeType(fullPath));
            }
            return Redirect("/");
        }
    }
}
