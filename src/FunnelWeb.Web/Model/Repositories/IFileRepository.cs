using System.IO;

namespace FunnelWeb.Web.Model.Repositories
{
    public interface IFileRepository
    {
        string MapPath(string path);
        string UnmapPath(string fullPath);
        bool IsFile(string path);
        FileItem[] GetItems(string path);
        DirectoryInfo[] GetDirectories(string path);
        FileInfo[] GetFiles(string path);

        void Move(string oldPath, string newPath);
        void Delete(string filePath);

        void CreateDirectory(string path, string name);
    }
}