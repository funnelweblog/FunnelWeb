using System.IO;
using System.Web.Mvc;

namespace FunnelWeb.Extensions.MetaWeblog.Controllers
{
    public class MetaWeblogController : Controller
    {
        public ActionResult WlwManifest()
        {
            var resourceStream = typeof (MetaWeblogController).Assembly
                .GetManifestResourceStream("FunnelWeb.Extensions.MetaWeblog.wlwmanifest.xml");
            using (var resourceStreamReader = new StreamReader(resourceStream))
            {
                var contents = resourceStreamReader.ReadToEnd();
                return Content(contents, "application/xml");
            }
        }
    }
}
