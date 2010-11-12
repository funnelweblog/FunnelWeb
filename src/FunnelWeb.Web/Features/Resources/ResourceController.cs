using System;
using System.Web.Mvc;

namespace FunnelWeb.Web.Controllers
{
    public partial class ResourceController : Controller
    {
        public virtual ActionResult Render(string fileName, string contentType)
        {
            Response.Cache.SetExpires(DateTime.Now.AddDays(10));
            return File(fileName, contentType);
        }
    }
}
