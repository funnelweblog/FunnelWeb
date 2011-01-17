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

        public ActionResult Index()
        {
            var tags = _repo.GetTags();

            return View(tags);
        }

    }
}
