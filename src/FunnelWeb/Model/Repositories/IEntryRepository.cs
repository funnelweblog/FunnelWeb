using System;
using System.Collections.Generic;
using FunnelWeb.Model.Strings;

namespace FunnelWeb.Model.Repositories
{
    public interface IEntryRepository
    {
        Tuple<IEnumerable<EntryRevision>, int> GetEntries(int skip, int? take);
        Entry GetEntry(int id);
        EntryRevision GetEntry(PageName name);
        EntryRevision GetEntry(PageName name, int revision);
        void Save(Entry entry);
        IEnumerable<EntryRevision> Search(string searchText);
        void Delete(int id);
    }
}
