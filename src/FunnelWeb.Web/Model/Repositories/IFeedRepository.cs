using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Model.Repositories
{
    public interface IFeedRepository
    {
        IQueryable<Feed> GetFeeds();
        IEnumerable<Entry> GetFeed(PageName feed, int skip, int take);
        int GetFeedCount(PageName feed);
        void Save(Feed feed);
        void Delete(Feed feed);
        IEnumerable<Comment> GetCommentFeed(int skip, int take);
    }
}
