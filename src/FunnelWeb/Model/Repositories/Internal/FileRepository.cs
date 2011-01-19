using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using FunnelWeb.Settings;
using FunnelWeb.Utilities;
using ICSharpCode.SharpZipLib.Zip;

namespace FunnelWeb.Model.Repositories.Internal
{
    public class FileRepository : IFileRepository
    {
        private readonly string root;

        public FileRepository(ISettingsProvider settingsProvider, HttpServerUtilityBase server)
        {
            root = settingsProvider.GetSettings().UploadPath;
            // If it's a virtual path then we can map it, otherwise we'll expect that it's a windows path
            if (root.StartsWith("~"))
            {
                root = server.MapPath(root);
            }
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

        public bool IsFile(string path)
        {
            var fullPath = MapPath(path);
            return File.Exists(fullPath);
        }

        public FileItem[] GetItems(string path)
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
                    Image = GetImage(file),
                    Modified = file.LastWriteTime.ToString("dd-MMM-yyyy")
                })).ToArray();
        }

        private static string GetImage(FileSystemInfo file)
        {
            var extension = (file.Extension ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(extension) || extension == ".") return "default.png";
            if (extension.StartsWith(".")) extension = extension.Substring(1);
            extension = extension.ToLowerInvariant();
            if (File.Exists(HttpContext.Current.Server.MapPath("/Content/Images/FileTypes/" + extension + ".png")))
            {
                return extension + ".png";
            }
            return "default.png";
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

        public void Move(string oldPath, string newPath)
        {
            oldPath = MapPath(oldPath);
            newPath = MapPath(newPath);
            if (!File.Exists(oldPath)) return;
            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
            File.Move(oldPath, newPath);
        }

        public void Delete(string filePath)
        {
            var fullPath = MapPath(filePath);
            if (IsFile(filePath))
            {
                File.Delete(fullPath);
            }
            else
            {
                Directory.Delete(fullPath, true);
            }
        }

        public void CreateDirectory(string path, string name)
        {
            var fullPath = MapPath(Path.Combine(path, name));
            Directory.CreateDirectory(fullPath);
        }

        public void Save(Stream inputStream, string fullPath, bool unzip)
        {
            if (unzip && IsZipFile(fullPath))
            {
                inputStream.Extract(fullPath);
            }
            else
            {
                inputStream.Save(fullPath);
            }
        }

        private bool IsZipFile(string fullPath)
        {
            var extension = Path.GetExtension(fullPath).ToLowerInvariant();
            return extension == "zip" || extension == "gz" || extension == "tar" || extension == "rar";
        }
    }
}