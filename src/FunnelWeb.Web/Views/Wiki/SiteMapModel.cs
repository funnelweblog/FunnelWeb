using System;
using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Views.Wiki
{
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