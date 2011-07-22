using FunnelWeb.Model;
using NHibernate;

namespace FunnelWeb.Repositories
{
    public static class Filters
    {
        public static IQueryOver<Entry, Entry> ApplyEntryStatusFilter(this IQueryOver<Entry, Entry> query, string entryStatus)
        {
            if (entryStatus != EntryStatus.All)
            {
                if (entryStatus == null)
                    query.Where(e => e.Status != EntryStatus.Private);
                else
                    query.Where(e => e.Status == entryStatus);
            }

            return query;
        }
    }
}