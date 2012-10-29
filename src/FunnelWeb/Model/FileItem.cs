namespace FunnelWeb.Model
{
    public class FileItem
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsDirectory { get; set; }
        public string Extension { get; set; }
        public string FileSize { get; set; }
        public string Modified { get; set; }
        public bool IsPathAbsolute { get; set; }
    }
}