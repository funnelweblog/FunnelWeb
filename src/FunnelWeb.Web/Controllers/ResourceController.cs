using System;
using System.Web.Mvc;
using FunnelWeb.Settings;

namespace FunnelWeb.Web.Controllers
{
    public class ResourceController : Controller
    {
        public ISettingsProvider SettingsProvider { get; set; }

        public virtual ActionResult Render(string fileName, string fileName2, string contentType)
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(10));

            string fileToRender = fileName;

            return RenderUploadedFileIfExists(fileToRender, contentType)
                ?? RenderThemedFileIfExists(fileToRender, contentType)
                ?? File(fileName2 ?? fileToRender, contentType);
        }

        public FileResult RenderThemedFileIfExists(string fileToRender, string contentType)
        {
            return RenderWhileReplacingTokenWith(fileToRender, contentType, "{Theme}", "/Themes/" + SettingsProvider.GetSettings<FunnelWebSettings>().Theme);
        }

        private FileResult RenderUploadedFileIfExists(string fileToRender, string contentType)
        {
            return RenderWhileReplacingTokenWith(fileToRender, contentType, "{Theme}", SettingsProvider.GetSettings<FunnelWebSettings>().UploadPath);
        }

        private FileResult RenderWhileReplacingTokenWith(string fileToRender, string contentType, string token, string replacement)
        {
            if (fileToRender.Contains(token))
            {
                fileToRender = fileToRender.Replace(token, replacement);
                var localFile = Server.MapPath(fileToRender);

                if (System.IO.File.Exists(localFile))
                    return File(localFile, contentType);
            }
            return null;
        }
    }
}
