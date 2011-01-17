using System.Collections.Generic;

namespace FunnelWeb.Model.Repositories
{
    public interface IFeedRepository
    {
        int GetEntryCount();
        IEnumerable<Entry> GetRecentEntries(int skip, int take);
        
        IEnumerable<Comment> GetRecentComments(int skip, int take);
    }
}