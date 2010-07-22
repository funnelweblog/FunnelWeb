using System;
using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Controllers
{
    partial class WikiController
    {
        public class RecentModel
        {
            public RecentModel(IEnumerable<Entry> revisions, int pageNumber, int totalPages)
            {
                Entries = revisions;
                PageNumber = pageNumber;
                TotalPages = totalPages;
            }

            public IEnumerable<Entry> Entries { get; set; }
            public int PageNumber { get; set; }
            public int TotalPages { get; set; }
        }

        public class NotFoundModel
        {
            public NotFoundModel(string searchText, bool is404, IEnumerable<Entry> results)
            {
                SearchText = searchText;
                Is404 = is404;
                Results = results;
            }

            public string SearchText { get; set; }
            public bool Is404 { get; set; }
            public IEnumerable<Entry> Results { get; set; }
        }

        public class PageModel
        {
            public PageModel(PageName page, Entry entry, bool isPriorVersion)
            {
                Page = page;
                Entry = entry;
                IsPriorVersion = isPriorVersion;
            }

            public bool IsPriorVersion { get; set; }
            public PageName Page { get; set; }
            public Entry Entry { get; set; }
        }

        public class EditModel
        {
            public EditModel(PageName page, Entry entry, bool isNew, IEnumerable<Feed> feeds)
            {
                Page = page;
                Entry = entry;
                IsNew = isNew;
                Feeds = feeds;
            }

            public PageName Page { get; set; }
            public Entry Entry { get; set; }
            public IEnumerable<Feed> Feeds { get; set; }
            public bool IsNew { get; set; }
        }

        public class RevisionsModel
        {
            public RevisionsModel(PageName page, Entry entry)
            {
                Page = page;
                Entry = entry;
            }

            public PageName Page { get; set; }
            public Entry Entry { get; set; }
        }

        public class SiteMapModel
        {
            public SiteMapModel(IEnumerable<Entry> entries)
            {
                Entries = entries.OrderByDescending(x => Prioritize(x)).ToList();
            }

            public IEnumerable<Entry> Entries { get; set; }

            public double Prioritize(Entry entry)
            {
                // I use the number of comments as an indicator of the popularity. For example:
                //  15+ comments => 0.5 + (1*0.5) = 1.0
                //   10 comments => 0.5 + (2/3*0.5) = 8.333
                //    2 comments => 0.5 + (2/3*0.5) = 5.667
                // New posts get a high initial ranking for the first 7 days
                var priority = 0.5 + ((double)Math.Min(entry.Comments.Count, 15) / 15) * 0.5;
                if (entry.Published.Date.AddDays(7) > DateTime.Now)
                {
                    priority = 1.0;
                }

                return priority;
            }

            public string GetChangeFrequency(Entry entry)
            {
                return entry.Published.Date.AddDays(7) > DateTime.Now 
                    ? "daily" 
                    : "weekly";
            }
        }
    }
}
