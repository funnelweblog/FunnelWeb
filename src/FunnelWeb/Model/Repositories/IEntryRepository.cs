using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Model.Strings;

namespace FunnelWeb.Model.Repositories
{
    public interface IEntryRepository
    {
        IQueryable<Entry> GetEntries();
        IEnumerable<Entry> GetUnpublished();
        Entry GetEntry(int id);
        Entry GetEntry(PageName name);
        Entry GetEntry(PageName name, int revision);
        Redirect GetClosestRedirect(PageName name);
        void Save(Entry entry);
        IEnumerable<Entry> Search(string searchText);
    }
}
