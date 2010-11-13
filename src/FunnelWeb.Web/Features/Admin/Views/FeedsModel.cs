using System.Collections.Generic;
using FunnelWeb.Web.Model;

namespace FunnelWeb.Web.Features.Admin.Views
{
    public class FeedsModel
    {
        public FeedsModel(IEnumerable<Feed> feeds)
        {
            Feeds = feeds;
        }

        public IEnumerable<Feed> Feeds { get; set; }
    }
}