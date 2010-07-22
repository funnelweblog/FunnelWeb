using System;
using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Web.Model.Repositories;
using FunnelWeb.Web.Model;
namespace FunnelWeb.Web.Application.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly object _lock = new object();
        private readonly IAdminRepository _repository;
        private string _siteTitle;
        private string _introduction;
        private string _mainLinks;
        private string _author;
        private string _searchDescription;
        private string _searchKeywords;
        private bool _isInitialized;
        private string _spamWords;

        public SettingsProvider(IAdminRepository repository)
        {
            _repository = repository;
        }

        public string SiteTitle
        {
            get
            {
                EnsureInitialized();
                return _siteTitle;
            }
        }

        public string Author
        {
            get
            {
                EnsureInitialized();
                return _author;
            }
        }

        public string SearchDescription
        {
            get
            {
                EnsureInitialized();
                return _searchDescription;
            }
        }

        public string SearchKeywords
        {
            get
            {
                EnsureInitialized();
                return _searchKeywords;
            }
        }

        public string SpamWords
        {
            get 
            { 
                EnsureInitialized();
                return _spamWords;
            }
        }

        public string Introduction
        {
            get
            {
                EnsureInitialized(); 
                return _introduction;
            }
        }

        public string MainLinks
        {
            get
            {
                EnsureInitialized(); 
                return _mainLinks;
            }
        }

        private void EnsureInitialized()
        {
            if (_isInitialized) 
                return;

            lock (_lock)
            {
                if (_isInitialized)
                    return;
                
                _isInitialized = true;

                var settings = _repository.GetSettings().ToList();
                _siteTitle = ReadSetting(settings, "ui-title");
                _introduction = ReadSetting(settings, "ui-introduction");
                _mainLinks = ReadSetting(settings, "ui-links");
                _author = ReadSetting(settings, "search-author");
                _searchDescription = ReadSetting(settings, "search-description");
                _searchKeywords = ReadSetting(settings, "search-keywords");
                _spamWords = ReadSetting(settings, "spam-blacklist");
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