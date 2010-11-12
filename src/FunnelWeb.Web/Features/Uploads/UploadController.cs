using System.IO;
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
        public virtual ActionResult Index(string path)
        {
            path = path ?? string.Empty;
            if (_fileRepository.IsFile(path))
            {
                return RedirectToAction(FunnelWebMvc.Upload.Index(Path.GetDirectoryName(path)));
            }

            ViewData.Model = new IndexModel(path, _fileRepository.GetItems(path));
            return View();
        }

        [Authorize]
        public virtual ActionResult Upload(string path, Upload upload)
        {
            var fullPath = _fileRepository.MapPath(Path.Combine(path, upload.FileName));
            upload.SaveTo(fullPath);
            return RedirectToAction(FunnelWebMvc.Upload.Index(path));
        }

        [Authorize]
        public virtual ActionResult CreateDirectory(string path, string name)
        {
            _fileRepository.CreateDirectory(path, name);
            return RedirectToAction(FunnelWebMvc.Upload.Index(path));
        }

        [Authorize]
        public virtual ActionResult Move(string path, string oldPath, string newPath)
        {
            _fileRepository.Move(oldPath, newPath);
            return RedirectToAction(FunnelWebMvc.Upload.Index(path));
        }

        [Authorize]
        public virtual ActionResult Delete(string path, string filePath)
        {
            _fileRepository.Delete(filePath);
            return RedirectToAction(FunnelWebMvc.Upload.Index(path));
        }

        public virtual ActionResult Render(string path)
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
