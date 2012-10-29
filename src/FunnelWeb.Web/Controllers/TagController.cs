using System;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Repositories;
using FunnelWeb.Repositories.Queries;

namespace FunnelWeb.Web.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly IRepository _repo;

        public TagController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult Index(string tagName = null)
        {
			var tags = _repo.Find(new SearchTagsByNameQuery(tagName));

            return Json(tags.Select(x => new { Id = x.Id, Name = x.Name }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Tag(string tagName)
        {
            var tag = _repo.FindFirstOrDefault(new SearchTagsByNameQuery(tagName));

            return Json(tag);
        }
    }
}
