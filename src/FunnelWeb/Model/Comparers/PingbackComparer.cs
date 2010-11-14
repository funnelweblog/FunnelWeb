using System.Collections.Generic;

namespace FunnelWeb.Model.Comparers
{
    public class PingbackComparer : IComparer<Pingback>
    {
        public int Compare(Pingback x, Pingback y)
        {
            return x.TargetTitle.CompareTo(y.TargetTitle);
        }
    }
}