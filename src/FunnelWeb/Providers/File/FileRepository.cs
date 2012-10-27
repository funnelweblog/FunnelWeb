using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Settings;
using FunnelWeb.Utilities;

namespace FunnelWeb.Providers.File
{
    public class FileRepository : FileRepositoryBase
    {
        private readonly IMimeTypeLookup mimeHelper;
        private readonly string root;

        public FileRepository(ISettingsProvider settingsProvider, HttpServerUtilityBase server, IMimeTypeLookup mimeHelper)
        {
            this.mimeHelper = mimeHelper;
            root = settingsProvider.GetSettings<FunnelWebSettings>().UploadPath;
            // If it's a virtual path then we can map it, otherwise we'll expect that it's a windows path
            if (root.StartsWith("~"))
            {
                root = server.MapPath(root);
            }
        }

        public static string ProviderName
        {
            get { return "Filesystem"; }
        }

        public string MapPath(string path)
        {
            path = (path ?? string.Empty).Trim();
            while (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            if (path.Contains("..")) throw new SecurityException("The path contained '..', which indicates an attempt to access another directory.");
            if (Regex.IsMatch(path, "^[A-z]:")) throw new SecurityException("An attempt was made to access a different drive");
            var fullPath = Path.GetFullPath(Path.Combine(root, path));
            if (!fullPath.StartsWith(root)) throw new SecurityException("An attempt was made to access an alternative file path");
            return fullPath;
        }

        public string UnmapPath(string fullPath)
        {
            var path = fullPath.Substring(root.Length);
            path = path.Replace("\\", "/");
            return path;
        }

        public override bool IsFile(string path)
        {
            var fullPath = MapPath(path);
            return System.IO.File.Exists(fullPath);
        }

        public override FileItem[] GetItems(string path)
        {
            var directories = GetDirectories(path);
            var files = GetFiles(path);
            return
                directories.Select(dir => new FileItem
                {
                    Extension = "",
                    Name = dir.Name,
                    Path = UnmapPath(dir.FullName),
                    FileSize = "",
                    Image = "dir.png",
                    IsDirectory = true,
                    Modified = dir.LastWriteTime.ToString("dd-MMM-yyyy")
                }).Union(files.Select(file => new FileItem
                {
                    Extension = file.Extension,
                    Name = file.Name,
                    Path = UnmapPath(file.FullName),
                    FileSize = file.Length.ToFileSizeString(),
                    Image = GetImage(file.Name, file.Extension),
                    Modified = file.LastWriteTime.ToString("dd-MMM-yyyy")
                })).ToArray();
        }

        public DirectoryInfo[] GetDirectories(string path)
        {
            var fullPath = MapPath(path);
            return Directory.Exists(fullPath)
                       ? Directory.GetDirectories(fullPath).Select(x => new DirectoryInfo(x)).ToArray()
                       : new DirectoryInfo[0];
        }

        public FileInfo[] GetFiles(string path)
        {
            var fullPath = MapPath(path);
            return Directory.Exists(fullPath)
                       ? Directory.GetFiles(fullPath).Select(x => new FileInfo(x)).ToArray()
                       : new FileInfo[0];
        }

        public override void Move(string oldPath, string newPath)
        {
            oldPath = MapPath(oldPath);
            newPath = MapPath(newPath);
            if (!System.IO.File.Exists(oldPath)) return;
            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
            System.IO.File.Move(oldPath, newPath);
        }

        public override void Delete(string filePath)
        {
            var fullPath = MapPath(filePath);
            if (IsFile(filePath))
            {
                System.IO.File.Delete(fullPath);
            }
            else
            {
                Directory.Delete(fullPath, true);
            }
        }

        public override void CreateDirectory(string path, string name)
        {
            var fullPath = MapPath(Path.Combine(path, name));
            Directory.CreateDirectory(fullPath);
        }

        public override void Save(Stream inputStream, string path, bool unzip)
        {
            var fullPath = MapPath(path);

            if (unzip && IsZipFile(fullPath))
            {
                inputStream.Extract(fullPath);
            }
            else
            {
                inputStream.Save(fullPath);
            }
        }

        public override ActionResult Render(string path)
        {
            if (IsFile(path))
                return new FilePathResult(MapPath(path), mimeHelper.GetMimeType(path));

            return new HttpNotFoundResult();
        }
    }
}