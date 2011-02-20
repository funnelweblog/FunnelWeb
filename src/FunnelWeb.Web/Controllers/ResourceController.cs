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
            if (fileToRender.Contains("{Theme}"))
            {
                fileToRender = fileToRender.Replace("{Theme}", "/Themes/" + SettingsProvider.GetSettings<FunnelWebSettings>().Theme);
                var localFile = Server.MapPath(fileToRender);
                if (System.IO.File.Exists(localFile))
                    return File(fileToRender, contentType);
            }

            return File(fileName2, contentType);
        }
    }
}
