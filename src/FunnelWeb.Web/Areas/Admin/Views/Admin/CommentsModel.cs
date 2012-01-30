using System.Collections.Generic;
using FunnelWeb.Model;
using FunnelWeb.Repositories;
using FunnelWeb.Web.Models;

namespace FunnelWeb.Web.Areas.Admin.Views.Admin
{
    public class CommentsModel
    {
        public CommentsModel(int pageNumber, PagedResult<Comment> comments)
        {
            PageNumber = pageNumber;
            Comments = comments;

            Paginator = new Paginator
            {
                ActionName = "Comments",
                CurrentPage = pageNumber,
                TotalPages = comments.TotalPages
            };
        }

        public IEnumerable<Comment> Comments { get; set; }
        public int PageNumber { get; set; }
        public Paginator Paginator { get; set; }
    }
}