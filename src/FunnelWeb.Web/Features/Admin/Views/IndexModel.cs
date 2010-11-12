using System.Collections.Generic;
using FunnelWeb.Web.Model;

namespace FunnelWeb.Web.Features.Admin.Views
{
    public class IndexModel
    {
        public IndexModel(IEnumerable<Setting> settings, IEnumerable<Feed> feeds, IEnumerable<Comment> comments, IEnumerable<Pingback> pingbacks, IEnumerable<Redirect> redirects, IEnumerable<string> themes)
        {
            Settings = settings;
            Feeds = feeds;
            Comments = comments;
            Pingbacks = pingbacks;
            Redirects = redirects;
            Themes = themes;
        }

        public IEnumerable<Feed> Feeds { get; set; }
        public IEnumerable<Setting> Settings { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Pingback> Pingbacks { get; set; }
        public IEnumerable<Redirect> Redirects { get; set; }
        public IEnumerable<string> Themes { get; set; }
    }

}