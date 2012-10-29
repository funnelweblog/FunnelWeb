using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Areas.Admin.Views.Upload
{
    public class IndexModel
    {
        public IndexModel(string path, FileItem[] items)
        {
            Items = items;
            PathString = path;

            var parts = new List<PathPart>();
            parts.Add(new PathPart("Home", "/"));
            var pathBuilder = "";
            foreach (var part in path.Split('/'))
            {
                if (part.Length == 0) continue;
                pathBuilder += "/" + part;
                parts.Add(new PathPart(part, pathBuilder));
            }
            Path = parts.ToArray();
        }

        public string StorageProvider { get; set; }
        public string PathString { get; set; }
        public PathPart[] Path { get; set; }
        public FileItem[] Items { get; set; }
        
        public class PathPart
        {
            public PathPart(string name, string path)
            {
                Name = name;
                Path = path;
            }

            public string Name { get; set; }
            public string Path { get; set; }
        }
    }
}