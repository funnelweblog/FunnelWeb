using System;
using System.ComponentModel.DataAnnotations;
using FunnelWeb.Model;
using FunnelWeb.Repositories;
using FunnelWeb.Web.Models;

namespace FunnelWeb.Web.Views.Wiki
{
    public class RecentModel
    {
        public RecentModel(string title, PagedResult<EntrySummary> results, string actionName)
        {
            Title = title;
            Results = results;
            ActionName = actionName;

            Paginator = new Paginator
                            {
                                ActionName = actionName,
                                CurrentPage = Results.Page,
                                TotalPages = Results.TotalPages
                            };
        }

        public string Title { get; set; }
        [UIHint("EntrySummaries")]
        public PagedResult<EntrySummary> Results { get; set; }
        public string ActionName { get; set; }

        public Paginator Paginator { get; set; }
    }
}