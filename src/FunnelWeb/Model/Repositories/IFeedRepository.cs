using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Model.Strings;

namespace FunnelWeb.Model.Repositories
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
