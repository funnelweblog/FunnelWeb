using System;
using System.Web.Mvc;

namespace FunnelWeb.Web.Controllers
{
    public class ResourceController : Controller
    {
        public virtual ActionResult Render(string fileName, string contentType)
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(10));
            return File(fileName, contentType);
        }
    }
}
