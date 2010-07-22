using System.Collections.Generic;
using FunnelWeb.Web.Model;

namespace FunnelWeb.Web.Controllers
{
    public partial class AdminController
    {
        public class IndexModel
        {
            public IndexModel(IEnumerable<Setting> settings, IEnumerable<Feed> feeds, IEnumerable<Comment> comments, IEnumerable<Pingback> pingbacks, IEnumerable<Redirect> redirects)
            {
                Settings = settings;
                Feeds = feeds;
                Comments = comments;
                Pingbacks = pingbacks;
                Redirects = redirects;
            }

            public IEnumerable<Feed> Feeds { get; set; }
            public IEnumerable<Setting> Settings { get; set; }
            public IEnumerable<Comment> Comments { get; set; }
            public IEnumerable<Pingback> Pingbacks { get; set; }
            public IEnumerable<Redirect> Redirects { get; set; }
        }
    }
}
