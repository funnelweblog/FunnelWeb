using System.IO;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Model;

namespace FunnelWeb.Providers.File
{
    public abstract class FileRepositoryBase : IFileRepository
    {
        protected static string GetImage(string file, string extension)
        {
            if (string.IsNullOrEmpty(extension) || extension == ".") return "default.png";
            if (extension.StartsWith(".")) extension = extension.Substring(1);
            extension = extension.ToLowerInvariant();
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/Content/Images/FileTypes/" + extension + ".png")))
            {
                return extension + ".png";
            }
            return "default.png";
        }

        protected static bool IsZipFile(string fullPath)
        {
            var extension = Path.GetExtension(fullPath).ToLowerInvariant();
            return extension == "zip" || extension == "gz" || extension == "tar" || extension == "rar";
        }

        public abstract bool IsFile(string path);
        public abstract FileItem[] GetItems(string path);
        public abstract void Move(string oldPath, string newPath);
        public abstract void Delete(string filePath);
        public abstract void CreateDirectory(string path, string name);
        public abstract void Save(Stream inputStream, string path, bool unzip);
        public abstract ActionResult Render(string path);
    }
}