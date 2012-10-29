using System;

namespace FunnelWeb.Web.Models
{
    public class Paginator
    {
        public string ActionName { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}