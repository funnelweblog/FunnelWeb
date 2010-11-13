using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FunnelWeb.Web.Application.Settings
{
    public class Settings
    {
        [StringLength(400)]
        [DefaultValue("database=FunnelWeb;server=(local)\\SQLEXPRESS;trusted_connection=true;")]
        [Description("The connection string used for the FunnelWeb SQL Server database")]
        [SettingStorage(StorageLocation.WebConfig, "funnelweb.configuration.database.connection")]
        public string ConnectionString { get; set; }

        [DisplayName("File Upload Path")]
        [StringLength(300)]
        [DefaultValue("~/files")]
        [Description("Files you upload for blog posts will be stored here. Use ~/XYZ to indicate a file path under the website root.")]
        [SettingStorage(StorageLocation.Database, "upload-path")]
        public string UploadPath { get; set; }

        [DisplayName("Akismet API Key")]
        [StringLength(30)]
        [DefaultValue("37726b9324fe")]
        [Description("If you have your own API key for Akismet, place it here.")]
        [SettingStorage(StorageLocation.Database, "akismet-api-key")]
        public string AkismetApiKey { get; set; }

        [DisplayName("Title")]
        [StringLength(100)]
        [Description("The title shown at the top in the browser")]
        [DefaultValue("My FunnelWeb Site")]
        [SettingStorage(StorageLocation.Database, "ui-title")]
        public string SiteTitle { get; set; }

        [DisplayName("Introduction")]
        [StringLength(100)]
        [DataType("Markdown")]
        [Description("The welcome text that is shown on the home page. You can use markdown.")]
        [DefaultValue("Welcome to your FunnelWeb blog. You can <a href=\"/login\">login</a> and edit this message in the administration section. The default username and password is <code>test/test</code>.")]
        [SettingStorage(StorageLocation.Database, "ui-introduction")]
        public string Introduction { get; set; }

        [DisplayName("Main Links")]
        [StringLength(100)]
        [DataType("HTML")]
        [Description("A list of links shown at the top of each page. Use HTML for this.")]
        [DefaultValue("<li><a href=\"/about\">About</a></li>")]
        [SettingStorage(StorageLocation.Database, "ui-links")]
        public string MainLinks { get; set; }

        [DisplayName("Footer")]
        [StringLength(3000)]
        [DataType("HTML")]
        [DefaultValue("<p>Powered by <a href=\"http://www.funnelweblog.com\">FunnelWeb</a>, the blog engine of real developers.</p>")]
        [Description("This will appear at the bottom of the page - use it to add copyright information, links to any web hosts, people or technologies that helped you to build the site, and so on.")]
        [SettingStorage(StorageLocation.Database, "ui-footer")]
        public string Footer { get; set; }

        [DisplayName("Default Page")]
        [Description("When users visit the root (/) of your site, it will be equivalent to visiting the page you specify here.")]
        [DefaultValue("blog")]
        [StringLength(100)]
        [SettingStorage(StorageLocation.Database, "default-page")]
        public string DefaultPage { get; set; }
        
        [DisplayName("Author")]
        [StringLength(100)]
        [Description("Your name. Rendered as a meta tag.")]
        [DefaultValue("Daffy Duck")]
        [SettingStorage(StorageLocation.Database, "search-author")]
        public string Author { get; set; }

        [DisplayName("Meta-Description")]
        [StringLength(100)]
        [Description("The description shown to search engines in the meta description tag.")]
        [DefaultValue("My website.")]
        [SettingStorage(StorageLocation.Database, "search-description")]
        public string SearchDescription { get; set; }

        [DisplayName("Meta-Keywords")]
        [StringLength(200)]
        [Description("Keywords shown to search engines in the meta-keywords tag (comma-separated text).")]
        [DefaultValue(".net, c#, test")]
        [SettingStorage(StorageLocation.Database, "search-keywords")]
        public string SearchKeywords { get; set; }

        [DisplayName("Spam blacklist")]
        [StringLength(500)]
        [DefaultValue("casino")]
        [Description("Comments with these words (case-insensitive) will automatically be marked as spam, in addition to Akismet. Seperate using spaces or newlines.")]
        [SettingStorage(StorageLocation.Database, "spam-blacklist")]
        public string SpamWords { get; set; }

        [DisplayName("Theme")]
        [StringLength(100)]
        [DefaultValue("Default")]
        [Description("The theme which will be used for this website.")]
        [SettingStorage(StorageLocation.Database, "ui-theme")]
        public string Theme { get; set; }

        [SettingStorage(StorageLocation.Custom)]
        public string[] Themes { get; set; }
    }
}