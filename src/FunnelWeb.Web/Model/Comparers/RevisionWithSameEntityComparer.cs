using System.Collections.Generic;

namespace FunnelWeb.Web.Model.Comparers
{
    public class RevisionWithSameEntityComparer : IEqualityComparer<Revision>
    {
        public bool Equals(Revision x, Revision y)
        {
            return x.Entry.Id == y.Entry.Id;
        }

        public int GetHashCode(Revision obj)
        {
            return obj.Entry.Id;
        }
    }
}