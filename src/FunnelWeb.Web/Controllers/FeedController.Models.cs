using System.Collections.Generic;
using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Controllers
{
    public partial class FeedController
    {
        public class FeedModel
        {
            public FeedModel(PageName feedName, IEnumerable<Entry> items)
            {
                FeedName = feedName;
                Items = items;
            }

            public PageName FeedName { get; set; }
            public IEnumerable<Entry> Items { get; set; }
        }

        public class CommentFeedModel
        {
            public CommentFeedModel(IEnumerable<Comment> comments)
            {
                Comments = comments;
            }

            public IEnumerable<Comment> Comments { get; set; }
        }
    }
}
