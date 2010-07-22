using System.IO;
using System.Web.Mvc;
using FunnelWeb.Web.Application;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Model.Repositories;
using FunnelWeb.Web.Application.Mime;

namespace FunnelWeb.Web.Controllers
{
    public partial class UploadController : Controller
    {
        private readonly IFileRepository _fileRepository;
        private readonly IMimeTypeLookup _mimeHelper;

        public UploadController(IFileRepository fileRepository, IMimeTypeLookup mimeHelper)
        {
            _fileRepository = fileRepository;
            _mimeHelper = mimeHelper;
        }

        [Authorize]
        public ActionResult Index(string path)
        {
            path = path ?? string.Empty;
            if (_fileRepository.IsFile(path))
            {
                return RedirectToAction("Index", new {path = Path.GetDirectoryName(path)});
            }

            ViewData.Model = new IndexModel(path, _fileRepository.GetItems(path));
            return View();
        }

        [Authorize]
        public ActionResult Upload(string path, Upload upload)
        {
            var fullPath = _fileRepository.MapPath(Path.Combine(path, upload.FileName));
            upload.SaveTo(fullPath);
            return RedirectToAction("Index", new {path });
        }

        public ActionResult CreateDirectory(string path, string name)
        {
            _fileRepository.CreateDirectory(path, name);
            return RedirectToAction("Index", new { path });
        }

        [Authorize]
        public ActionResult Move(string path, string oldPath, string newPath)
        {
            _fileRepository.Move(oldPath, newPath);
            return RedirectToAction("Index", new {path});
        }

        [Authorize]
        public ActionResult Delete(string path, string filePath)
        {
            _fileRepository.Delete(filePath);
            return RedirectToAction("Index", new { path });
        }

        public ActionResult Render(string path)
        {
            if (_fileRepository.IsFile(path))
            {
                var fullPath = _fileRepository.MapPath(path);
                return File(fullPath, _mimeHelper.GetMimeType(fullPath));
            }
            return Redirect("/");
        }
    }
}
