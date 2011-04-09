using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Areas.Admin.Views.Admin
{
    public class CommentsModel
    {
        public CommentsModel(int pageNumber, int totalPages, IEnumerable<Comment> comments)
        {
            PageNumber = pageNumber;
            TotalPages = totalPages;
            Comments = comments;
        }

        public IEnumerable<Comment> Comments { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}