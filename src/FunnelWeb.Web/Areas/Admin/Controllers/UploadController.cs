using System;
using System.IO;
using System.Web.Mvc;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Web.Application;
using FunnelWeb.Web.Application.Mime;
using FunnelWeb.Web.Areas.Admin.Views.Upload;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    public class UploadController : Controller
    {
        public IFileRepository FileRepository { get; set; }
        public IMimeTypeLookup MimeHelper { get; set; }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult Index(string path)
        {
            path = path ?? string.Empty;
            if (FileRepository.IsFile(path))
            {
                return RedirectToAction("Index", "Upload", new { path = Path.GetDirectoryName(path) });
            }

            ViewData.Model = new IndexModel(path, FileRepository.GetItems(path));
            return View();
        }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult Upload(string path, bool? unzip, FileUpload upload)
        {
            var fullPath = FileRepository.MapPath(Path.Combine(path, upload.FileName));
            FileRepository.Save(upload.Stream, fullPath, unzip ?? false);
            return RedirectToAction("Index", "Upload", new { path });
        }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult CreateDirectory(string path, string name)
        {
            FileRepository.CreateDirectory(path, name);
            return RedirectToAction("Index", "Upload", new { path });
        }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult Move(string path, string oldPath, string newPath)
        {
            FileRepository.Move(oldPath, newPath);
            return RedirectToAction("Index", "Upload", new { path });
        }

        [Authorize(Roles = "Moderator")]
        public virtual ActionResult Delete(string path, string filePath)
        {
            FileRepository.Delete(filePath);
            return RedirectToAction("Index", "Upload", new { path });
        }

        public virtual ActionResult Render(string path)
        {
            if (FileRepository.IsFile(path))
            {
                var fullPath = FileRepository.MapPath(path);

                return new CustomFileResult(fullPath, MimeHelper.GetMimeType(fullPath));
            }
            return Redirect("/");
        }

        // We can't use MVC's inbuilt FileResult, because ClientDependency seems to conflict with it
        // and compresses the files
        private class CustomFileResult : ActionResult
        {
            private readonly string path;
            private readonly string mimeType;

            public CustomFileResult(string path, string mimeType)
            {
                this.path = path;
                this.mimeType = mimeType;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.ContentType = mimeType;
                context.HttpContext.Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(path));
                context.HttpContext.Response.WriteFile(path);
                context.HttpContext.Response.Flush();
            }
        }
    }
}
