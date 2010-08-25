using System.Web.Mvc;
using System;

namespace FunnelWeb.Web.Controllers
{
    public partial class ResourceController : Controller
    {
        public virtual ActionResult Render(string fileName, string contentType)
        {
            Response.Cache.SetExpires(DateTime.Now.AddDays(90));
            return File(fileName, contentType);
        }
    }
}
