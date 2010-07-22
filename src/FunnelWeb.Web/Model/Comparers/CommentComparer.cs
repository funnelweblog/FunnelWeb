using System.Collections.Generic;

namespace FunnelWeb.Web.Model.Comparers
{
    public class CommentComparer : IComparer<Comment>
    {
        public int Compare(Comment x, Comment y)
        {
            return x.Posted.CompareTo(y.Posted);
        }
    }
}