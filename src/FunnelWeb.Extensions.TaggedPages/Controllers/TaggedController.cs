using System.Web.Mvc;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Web.Application.Filters;

namespace FunnelWeb.Extensions.TaggedPages.Controllers
{
    [FunnelWebRequest]
    public class TaggedController : Controller
    {
        private readonly ITagRepository _tagRepository;
        //public ITagRepository TagRepository { get; set; }

        public TaggedController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public ActionResult Index(string tag)
        {
            var tagItems = _tagRepository.GetTaggedItems(tag, 0, 30);

            return View(tagItems);
        }
    }
}
