using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Areas.Admin.Views.Admin
{
    public class PingbacksModel
    {
        public PingbacksModel(IEnumerable<Pingback> pingbacks)
        {
            Pingbacks = pingbacks;
        }

        public IEnumerable<Pingback> Pingbacks { get; set; }
    }
}