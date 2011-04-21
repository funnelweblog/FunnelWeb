using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Filters;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;

namespace FunnelWeb.Web.Controllers
{
    [FunnelWebRequest]
    public class TaggedController : Controller
    {
        private readonly ITagRepository tagRepository;

        public TaggedController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        public ActionResult Index(string tag)
        {
            var tagItems = tagRepository.GetTaggedItems(tag, 0, 30);
            ViewBag.Tag = tag;
            return View(tagItems.Select(x => (EntrySummary)x));
        }
    }
}
