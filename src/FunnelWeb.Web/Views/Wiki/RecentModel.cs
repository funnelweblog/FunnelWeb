using System;
using System.ComponentModel.DataAnnotations;
using FunnelWeb.Model;
using FunnelWeb.Repositories;

namespace FunnelWeb.Web.Views.Wiki
{
    public class RecentModel
    {
        public RecentModel(string title, PagedResult<EntrySummary> results, string actionName)
        {
            Title = title;
            Results = results;
            ActionName = actionName;
        }

        public string Title { get; set; }
        [UIHint("EntrySummaries")]
        public PagedResult<EntrySummary> Results { get; set; }
        public string ActionName { get; set; }
    }
}