using System;
using System.Linq;

namespace FunnelWeb.Model
{
    public static class EntryExtensions
    {
        public static Revision Revision(this Entry entry, int? revision)
        {
            return revision.HasValue
                           ? entry.Revisions.Single(x => x.RevisionNumber == revision.Value)
                           : entry.LatestRevision;
        }
    }
}
