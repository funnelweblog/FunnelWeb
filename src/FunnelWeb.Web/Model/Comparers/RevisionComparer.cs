using System.Collections.Generic;

namespace FunnelWeb.Web.Model.Comparers
{
    public class RevisionComparer : IComparer<Revision>
    {
        public int Compare(Revision x, Revision y)
        {
            return -x.Revised.CompareTo(y.Revised);
        }
    }
}