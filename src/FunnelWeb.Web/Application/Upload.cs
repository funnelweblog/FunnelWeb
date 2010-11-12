using System.IO;
using System.Web;

namespace FunnelWeb.Web.Application
{
    public class Upload
    {
        private readonly HttpPostedFileBase postedFile;

        public Upload(HttpPostedFileBase postedFile)
        {
            this.postedFile = postedFile;
        }

        public string FileName
        {
            get { return Path.GetFileName(postedFile.FileName); }
        }

        public void SaveTo(string fullPath)
        {
            postedFile.SaveAs(fullPath);
        }
    }
}
