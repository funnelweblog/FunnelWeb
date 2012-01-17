using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
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
            using (var writer = new StringWriter())
            {
                var root = (HttpContext.Request.IsSecureConnection ? "https://" : "http://") + HttpContext.Request.Url.Authority;
                var homepage = root + VirtualPathUtility.ToAbsolute("~/");
                var api = root + VirtualPathUtility.ToAbsolute("~/blogapi");
                new XDocument(
                    new XElement(XName.Get("rsd", "http://tales.phrasewise.com/rfc/rsd"), new XAttribute("version", "1.0"),
                                 new XElement("service",
                                              new XElement("engineName", "FunnelWeblog"),
                                              new XElement("engineLink", "http://www.funnelweblog.com/"),
                                              new XElement("homePageLink", homepage),
                                              new XElement("apis",
                                                           new XElement("api",
                                                                        new XAttribute("name", "MetaWeblog"),
                                                                        new XAttribute("preferred", "true"),
                                                                        new XAttribute("apiLink", api),
                                                                        new XAttribute("blogId", "something")))))).Save(writer);

                var rsdFile = writer.ToString();
                return Content(rsdFile, "application/rsd+xml");
            }
        }
    }
}