using System.Web.Mvc;
using FunnelWeb.Web.Application;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Model.Repositories;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Controllers
{
    [Transactional]
    public partial class FeedController : Controller
    {
        private readonly IFeedRepository _feedRepository;

        public FeedController(IFeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }

        public ActionResult Feed(PageName feedName)
        {
            var entries = _feedRepository.GetFeed(feedName, 0, 20);
            ViewData.Model = new FeedModel(feedName, entries);
            return View();
        }

        public ActionResult CommentFeed()
        {
            var comments = _feedRepository.GetCommentFeed(0, 20);
            ViewData.Model = new CommentFeedModel(comments);
            return View();
        }
    }
}