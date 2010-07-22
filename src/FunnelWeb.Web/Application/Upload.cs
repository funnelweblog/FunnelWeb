using System;
using System.Web;
using System.IO;

namespace FunnelWeb.Web.Application
{
    public class Upload
    {
        private readonly HttpPostedFileBase _postedFile;

        public Upload(HttpPostedFileBase postedFile)
        {
            _postedFile = postedFile;
        }

        public string FileName
        {
            get { return Path.GetFileName(_postedFile.FileName); }
        }

        public void SaveTo(string fullPath)
        {
            _postedFile.SaveAs(fullPath);
        }
    }
}
