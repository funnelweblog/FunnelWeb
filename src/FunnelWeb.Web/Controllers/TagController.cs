using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FunnelWeb.Web.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private Model.Repositories.ITagRepository repo;

        public TagController(Model.Repositories.ITagRepository repo)
        {
            // TODO: Complete member initialization
            this.repo = repo;
        }
        //
        // GET: /Tag/

        public ActionResult Index()
        {
            return View();
        }

    }
}
