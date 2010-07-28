using System.Linq;
using FunnelWeb.Web.Application.Validation;
using FunnelWeb.Web.Model.Strings;
using System.Collections.Generic;

namespace FunnelWeb.Web.Model.Repositories
{
    public interface IFeedRepository
    {
        IQueryable<Feed> GetFeeds();
        IEnumerable<Entry> GetFeed(PageName feed, int skip, int take);
        int GetFeedCount(PageName feed);
        ValidationResult Save(Feed feed);
        void Delete(Feed feed);
        IEnumerable<Comment> GetCommentFeed(int skip, int take);
    }
}
