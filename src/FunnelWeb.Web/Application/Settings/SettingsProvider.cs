using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Repositories;

namespace FunnelWeb.Web.Application.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly object @lock = new object();
        private readonly IAdminRepository repository;
        private string siteTitle;
        private string introduction;
        private string mainLinks;
        private string author;
        private string searchDescription;
        private string searchKeywords;
        private bool isInitialized;
        private string spamWords;
        private string defaultPage;
        private string footer;
        private string theme;

        public SettingsProvider(IAdminRepository repository)
        {
            this.repository = repository;
        }

        public string SiteTitle
        {
            get
            {
                EnsureInitialized();
                return siteTitle;
            }
        }

        public string DefaultPage
        {
            get
            {
                EnsureInitialized();
                return defaultPage;
            }
        }

        public string Author
        {
            get
            {
                EnsureInitialized();
                return author;
            }
        }

        public string SearchDescription
        {
            get
            {
                EnsureInitialized();
                return searchDescription;
            }
        }

        public string SearchKeywords
        {
            get
            {
                EnsureInitialized();
                return searchKeywords;
            }
        }

        public string SpamWords
        {
            get 
            { 
                EnsureInitialized();
                return spamWords;
            }
        }

        public string Introduction
        {
            get
            {
                EnsureInitialized(); 
                return introduction;
            }
        }

        public string MainLinks
        {
            get
            {
                EnsureInitialized(); 
                return mainLinks;
            }
        }

        public string Footer
        {
            get
            {
                EnsureInitialized();
                return footer;
            }
        }

        public string Theme
        {
            get
            {
                EnsureInitialized();
                return theme;
            }
        }

        private void EnsureInitialized()
        {
            if (isInitialized) 
                return;

            lock (@lock)
            {
                if (isInitialized)
                    return;
                
                isInitialized = true;

                var settings = repository.GetSettings().ToList();
                siteTitle = ReadSetting(settings, "ui-title");
                introduction = ReadSetting(settings, "ui-introduction");
                mainLinks = ReadSetting(settings, "ui-links");
                author = ReadSetting(settings, "search-author");
                searchDescription = ReadSetting(settings, "search-description");
                searchKeywords = ReadSetting(settings, "search-keywords");
                spamWords = ReadSetting(settings, "spam-blacklist");
                footer = ReadSetting(settings, "ui.footer");
                defaultPage = ReadSetting(settings, "default-page");
                theme = ReadSetting(settings, "ui-theme");
            }
        }

        private static string ReadSetting(IEnumerable<Setting> settings, string name)
        {
            var setting = settings.FirstOrDefault(x => x.Name == name);
            if (setting == null) return string.Empty;
            return setting.Value;
        }
    }
}