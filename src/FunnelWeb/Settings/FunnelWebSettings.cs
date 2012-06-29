using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FunnelWeb.Settings
{
    public class FunnelWebSettings : ISettings
    {
        [DisplayName("File Upload Path")]
        [StringLength(300)]
        [DefaultValue("~/files")]
        [Description("Files you upload for blog posts will be stored here. Use ~/XYZ to indicate a file path under the website root.")]
        [SettingStorage(StorageLocation.Database, "upload-path")]
        public string UploadPath { get; set; }

        [DisplayName("File Storage Provider")]
        [StringLength(20)]
        [DefaultValue("Filesystem")]
        [Description("The storage provider")]
        [SettingStorage(StorageLocation.Database, "storage-provider")]
        public string StorageProvider { get; set; }

        [DisplayName("Akismet API Key")]
        [StringLength(30)]
        [DefaultValue("37726b9324fe")]
        [Description("If you have your own API key for Akismet, place it here.")]
        [SettingStorage(StorageLocation.Database, "akismet-api-key")]
        public string AkismetApiKey { get; set; }

        [DisplayName("Title")]
        [StringLength(200)]
        [Description("The title shown at the top in the browser")]
        [DefaultValue("My FunnelWeb Site")]
        [SettingStorage(StorageLocation.Database, "ui-title")]
        public string SiteTitle { get; set; }

        [DisplayName("Introduction")]
        [StringLength(5000)]
        [DataType("Markdown")]
        [Description("The welcome text that is shown on the home page. You can use markdown.")]
        [DefaultValue("Welcome to your FunnelWeb blog. You can <a href=\"/admin/login\">login</a> and edit this message in the administration section. The default username and password is <code>test/test</code>.")]
        [SettingStorage(StorageLocation.Database, "ui-introduction")]
        public string Introduction { get; set; }

        [DisplayName("Main Links")]
        [StringLength(5000)]
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
        [StringLength(150)]
        [Description("The description shown to search engines in the meta description tag.")]
        [DefaultValue("My website.")]
        [SettingStorage(StorageLocation.Database, "search-description")]
        public string SearchDescription { get; set; }

        [DisplayName("Meta-Keywords")]
        [StringLength(100)]
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

        [DisplayName("Disable comments after")]
        [DefaultValue(0)]
        [Description("If a post is older than this many days, comments will be disabled. Use 0 to allow comments indefinitely.")]
        [SettingStorage(StorageLocation.Database, "spam-comment-disable")]
        public int DisableCommentsOlderThan { get; set; }

        [DisplayName("HTML Head")]
        [StringLength(2000)]
        [DefaultValue("")]
        [Description("Custom HTML that will appear just before the &lt;/head&gt; tag")]
        [SettingStorage(StorageLocation.Database, "ui-html-head")]
        public string HtmlHead { get; set; }

        [DisplayName("HTML Footer")]
        [StringLength(2000)]
        [DefaultValue("")]
        [Description("Custom HTML that will appear just before the &lt;/body&gt; tag")]
        [SettingStorage(StorageLocation.Database, "ui-html-foot")]
        public string HtmlFooter { get; set; }

        [DisplayName("Theme")]
        [StringLength(100)]
        [DefaultValue("Official")]
        [Description("The theme which will be used for this website.")]
        [SettingStorage(StorageLocation.Database, "ui-theme")]
        public string Theme { get; set; }

        [DisplayName("Server")]
        [StringLength(100)]
        [DefaultValue("")]
        [Description("The server name for your SMTP server.")]
        [SettingStorage(StorageLocation.Database, "smtp-server")]
        public string SmtpServer { get; set; }

        [DisplayName("Port")]
        [DefaultValue("21")]
        [Description("The port that your SMTP server listens on.")]
        [SettingStorage(StorageLocation.Database, "smtp-port")]
        public int SmtpPort { get; set; }

        [DisplayName("From")]
        [StringLength(200)]
        [DefaultValue("funnelweb@your-site.com")]
        [RegularExpression("^[A-Za-z0-9._%+-]+@([A-Za-z0-9-]+\\.)+([A-Za-z0-9]{2,4}|museum)$", ErrorMessage = "Please enter a valid email address")]
        [Description("The email address from which emails will be sent.")]
        [SettingStorage(StorageLocation.Database, "smtp-from")]
        public string SmtpFromEmailAddress { get; set; }

        [DisplayName("Username")]
        [StringLength(100)]
        [DefaultValue("")]
        [Description("If your SMTP server requires authentication, enter your username here, or leave it empty.")]
        [SettingStorage(StorageLocation.Database, "smtp-auth-username")]
        public string SmtpUsername { get; set; }

        [DisplayName("Password")]
        [StringLength(100)]
        [DefaultValue("")]
        [Description("If your SMTP server requires authentication, enter your password here, or leave it empty.")]
        [SettingStorage(StorageLocation.Database, "smtp-auth-password")]
        public string SmtpPassword { get; set; }

        [DisplayName("To")]
        [StringLength(200)]
        [DefaultValue("you@your-site.com")]
        [RegularExpression("^[A-Za-z0-9._%+-]+@([A-Za-z0-9-]+\\.)+([A-Za-z0-9]{2,4}|museum)$", ErrorMessage = "Please enter a valid email address")]
        [Description("The email address you want comment notification emails to be sent to.")]
        [SettingStorage(StorageLocation.Database, "smtp-to")]
        public string SmtpToEmailAddress { get; set; }

        [DisplayName("Use SSL")]
        [DefaultValue(false)]
        [Description("Whether SSL should be used when sending emails")]
        [SettingStorage(StorageLocation.Database, "smtp-ssl")]
        public bool SmtpUseSsl { get; set; }

        [DisplayName("Notify me")]
        [DefaultValue(true)]
        [Description("Notify me when comments are posted")]
        [SettingStorage(StorageLocation.Database, "smtp-comments-on")]
        public bool CommentNotification { get; set; }

        [DisplayName("Facebook Like")]
        [DefaultValue(true)]
        [Description("Show a Facebook 'Like' button under each page")]
        [SettingStorage(StorageLocation.Database, "facebook-like")]
        [DataType("FacebookLike")]
        public bool FacebookLike { get; set; }

        [DisplayName("Public History")]
        [DefaultValue(true)]
        [Description("Allow public users to see the 'history' links and view past revisions of your posts.")]
        [SettingStorage(StorageLocation.Database, "show-public-history")]
        public bool EnablePublicHistory { get; set; }

        [DisplayName("Home page")]
        [DefaultValue("blog")]
        [Description("Enter the name of a page to use as your custom home page. Use 'blog' to show a list of recent posts.")]
        [SettingStorage(StorageLocation.Database, "home-page")]
        public string CustomHomePage { get; set; }

        [DisplayName("Enable Disqus commenting")]
        [DefaultValue(false)]
        [Description("Enable the Disqus commenting system. Note - this will still require the theme to also use Disqus.")]
        [SettingStorage(StorageLocation.Database, "enable-disqus-comments")]
        public bool EnableDisqusCommenting { get; set; }

        [DisplayName("Shortname for Disqus")]
        [DefaultValue("")]
        [Description("The shortname of your Disqus comments, configured on the Disqus website.")]
        [SettingStorage(StorageLocation.Database, "disqus-shortname")]
        public string DisqusShortname { get; set; }
    }
}