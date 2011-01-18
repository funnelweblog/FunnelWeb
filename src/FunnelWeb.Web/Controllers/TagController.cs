using System;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Model.Repositories;

namespace FunnelWeb.Web.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly ITagRepository _repo;

        public TagController(ITagRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult Index(string tagName = null)
        {
            var tags = _repo.GetTags(tagName);

            return Json(tags, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Tag(string tagName)
        {
            var tag = _repo.GetTag(tagName);

            return Json(tag);
        }
    }
}
