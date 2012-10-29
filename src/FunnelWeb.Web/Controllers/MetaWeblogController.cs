using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace FunnelWeb.Web.Controllers
{
    public class MetaWeblogController : Controller
    {
        public ActionResult WlwManifest()
        {
            Stream resourceStream = typeof(MetaWeblogController).Assembly
                .GetManifestResourceStream("FunnelWeb.Web.Application.MetaWeblog.wlwmanifest.xml");
            using (var resourceStreamReader = new StreamReader(resourceStream))
            {
                string contents = resourceStreamReader.ReadToEnd();
                return Content(contents, "application/wlwmanifest+xml");
            }
        }

        public ActionResult Rsd()
        {
            const string xmlns = "http://archipelago.phrasewise.com/rsd";
            var root = (HttpContext.Request.IsSecureConnection ? "https://" : "http://") + HttpContext.Request.Url.Authority;
            var homepage = root + VirtualPathUtility.ToAbsolute("~/");
            var api = root + VirtualPathUtility.ToAbsolute("~/blogapi");
            var rsdFile = new XDocument(
                new XElement(XName.Get("rsd", xmlns), new XAttribute("version", "1.0"),
                             new XElement(XName.Get("service", xmlns),
                                          new XElement(XName.Get("engineName", xmlns), "FunnelWeblog"),
                                          new XElement(XName.Get("engineLink", xmlns), "http://www.funnelweblog.com/"),
                                          new XElement(XName.Get("homePageLink", xmlns), homepage),
                                          new XElement(XName.Get("apis", xmlns),
                                                       new XElement(XName.Get("api", xmlns),
                                                                    new XAttribute("name", "MetaWeblog"),
                                                                    new XAttribute("preferred", "true"),
                                                                    new XAttribute("apiLink", api),
                                                                    new XAttribute("blogId", "something"))))))
                .ToString();

            return Content(rsdFile, "application/rsd+xml", Encoding.Default);
        }
    }
}