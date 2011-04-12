using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Views.Wiki
{
    public class RecentModel
    {
        public RecentModel(string title, IEnumerable<Entry> revisions, int pageNumber, int totalPages, string actionName)
        {
            Title = title;
            Entries = revisions;
            PageNumber = pageNumber;
            TotalPages = totalPages;
            ActionName = actionName;
        }

        public string Title { get; set; }
        [DataType("Entries")]
        public IEnumerable<EntrySummary> Entries { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string ActionName { get; set; }
    }
}