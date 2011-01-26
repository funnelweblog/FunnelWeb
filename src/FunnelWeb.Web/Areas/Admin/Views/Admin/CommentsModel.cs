using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Areas.Admin.Views.Admin
{
    public class CommentsModel
    {
        public CommentsModel(IEnumerable<Comment> comments)
        {
            Comments = comments;
        }

        public IEnumerable<Comment> Comments { get; set; }
    }
}