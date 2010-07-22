using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Web.Model.Strings;
using FunnelWeb.Web.Application.Validation;

namespace FunnelWeb.Web.Model.Repositories
{
    public interface IEntryRepository
    {
        IQueryable<Entry> GetEntries();
        Entry GetEntry(PageName name);
        Entry GetEntry(PageName name, int revision);
        Redirect GetClosestRedirect(PageName name);
        ValidationResult Save(Entry entry);
        IEnumerable<Entry> Search(string searchText);
    }
}
