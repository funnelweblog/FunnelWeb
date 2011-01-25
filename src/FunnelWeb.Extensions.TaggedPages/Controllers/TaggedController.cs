using System.Web.Mvc;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Web.Application.Filters;

namespace FunnelWeb.Extensions.TaggedPages.Controllers
{
    [FunnelWebRequest]
    public class TaggedController : Controller
    {
        public ITagRepository TagRepository { get; set; }

        public ActionResult Index(string tag)
        {
            var entries = TagRepository.GetTaggedItems(tag, 0, 30);

            return View(entries);
        }
    }
}
