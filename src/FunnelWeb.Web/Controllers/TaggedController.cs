using System.Web.Mvc;
using FunnelWeb.Filters;
using FunnelWeb.Repositories;
using FunnelWeb.Repositories.Queries;

namespace FunnelWeb.Web.Controllers
{
    [FunnelWebRequest]
    public class TaggedController : Controller
    {
        private readonly IRepository repository;

        public TaggedController(IRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index(string tag)
        {
            var tagItems = repository.Find(new GetEntriesByTagQuery(tag), 0, 30);
            ViewBag.Tag = tag;
            return View(tagItems);
        }
    }
}
