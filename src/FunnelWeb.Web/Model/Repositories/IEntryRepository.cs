using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Model.Repositories
{
    public interface IEntryRepository
    {
        IQueryable<Entry> GetEntries();
        IEnumerable<Entry> GetUnpublished();
        Entry GetEntry(PageName name);
        Entry GetEntry(PageName name, int revision);
        Redirect GetClosestRedirect(PageName name);
        void Save(Entry entry);
        IEnumerable<Entry> Search(string searchText);
    }
}
