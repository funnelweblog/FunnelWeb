using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Features.Admin.Views
{
    public class FeedsModel
    {
        public FeedsModel()
        {
        }

        public FeedsModel(IEnumerable<Feed> feeds)
        {
            Feeds = feeds;
        }

        public IEnumerable<Feed> Feeds { get; set; }

        [Required]
        [DisplayName("URL name")]
        [StringLength(30)]
        public string FeedName { get; set; }

        [Required]
        [DisplayName("Title")]
        [StringLength(40)]
        public string FeedTitle { get; set; }
    }
}