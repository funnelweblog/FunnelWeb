using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Model.Strings;
using FunnelWeb.Mvc;
using NHibernate;
using NHibernate.Validator.Constraints;

namespace FunnelWeb.Model
{
    public class EntryRevision
    {
        private string tagsString;

        public EntryRevision()
        {
            Title = string.Empty;
            Name = string.Empty;
            Summary = string.Empty;
            MetaDescription = string.Empty;
            MetaTitle = string.Empty;
            Status = string.Empty;
            Author = string.Empty;
            HideChrome = false;
            Published = DateTime.UtcNow;
            PageTemplate = null;
            Format = Formats.Markdown;
            IsDiscussionEnabled = true;
            Status = EntryStatus.PublicBlog;

            Comments = new List<Comment>();
            Pingbacks = new List<Pingback>();
            Tags = new List<Tag>();
        }

        public virtual int Id { get; set; }

        public virtual bool IsNew { get { return Id == 0; } }

        [DataType("Markdown")]
        [NotNullNotEmpty(Message = "Please provide a body for this wiki entry.")]
        [Required]
        [DisplayName("Content")]
        [HintSize(HintSize.Large)]
        [AllowHtml]
        public virtual string Body { get; set; }

        [Required]
        [DisplayName("Format")]
        [Description("Choose the markup format you'd like to use for this page")]
        public virtual string Format { get; set; }
        public virtual DateTime Revised { get; set; }
        public virtual int RevisionNumber { get; set; }
        public virtual int LatestRevisionNumber { get; set; }

        [DisplayName("Change summary")]
        [StringLength(300)]
        [Description("A brief overview of what was changed and why. This will appear on the page history.")]
        [HintSize(HintSize.Large)]
        public string ChangeSummary { get; set; }

        public virtual bool IsPriorVersion 
        { 
            get
            {
                return RevisionNumber < LatestRevisionNumber;
            } 
        }

        [Required]
        [DisplayName("SLUG")]
        [EnforcedStringLength(200)] //StringLength throws a cast exception for PageName
        [Description("This will form the URL to your page.")]
        [RegularExpression("[a-z0-9\\-\\/]+", ErrorMessage = "Page names can only include lowercase alpha characters, numbers, dashes and forward slashes (/)")]
        [HintSize(HintSize.Medium)]
        public virtual PageName Name { get; set; }

        [Required]
        [DisplayName("Title")]
        [StringLength(200)]
        [Description("This appears at the top of this page and on the home page.")]
        [HintSize(HintSize.Medium)]
        public virtual string Title { get; set; }

        [DisplayName("Introduction")]
        [StringLength(1000)]
        [Description("An introduction that will appear at the top or right of the page. Use markdown or HTML.")]
        [HintSize(HintSize.Large)]
        public virtual string Summary { get; set; }
        
        public virtual int CommentCount { get; set; }

        [DisplayName("Summary")]
        [StringLength(150)]
        [Description("A short description that will appear on the home page, and in the meta-description shown to search engines.")]
        [HintSize(HintSize.Large)]
        public virtual string MetaDescription { get; set; }

        [Required]
        [DisplayName("Publish date")]
        [Description("This page will not appear in any feeds until after the date above.")]
        [HintSize(HintSize.Medium)]
        [DataType(DataType.Date)]
        [RegularExpression("[0-9]{4}-[0-9]{1,2}-[0-9]{1,2}", ErrorMessage = "Please enter a date in YYYY-MM-DD format.")]
        public virtual string PublishDate
        {
            get { return Published.ToLocalTime().ToString("yyyy-MM-dd"); }
            set { Published = DateTime.Parse(value, CultureInfo.InvariantCulture).ToUniversalTime(); }
        }

        [DataType("PublishedDate")]
        public virtual DateTime Published { get; set; }
        
        [DataType("Tags")]
        public virtual IList<Tag> Tags { get; set; }

        [DisplayName("Page template")]
        [StringLength(20)]
        [Description("This will change how your page looks.")]
        [RegularExpression("[a-zA-Z0-9\\-]+", ErrorMessage = "Page templates can only include lowercase alpha characters, numbers and dashes")]
        [HintSize(HintSize.Medium)]
        public virtual string PageTemplate { get; set; }


        [DisplayName("Disable comments")]
        [Description("If checked, users will not be able to post comments on this page")]
        public virtual bool DisableComments
        {
            get { return !IsDiscussionEnabled; }
            set { IsDiscussionEnabled = !value; }
        }

        public virtual bool IsDiscussionEnabled { get; set; }

        [DisplayName("Meta-title")]
        [StringLength(255)]
        [Description("This appears at the top of the browser tab and is used by search engines.")]
        [HintSize(HintSize.Medium)]
        public virtual string MetaTitle { get; set; }

        [DisplayName("Hide chrome")]
        [Description("If checked, the page title, date, history and so on will be hidden.")]
        public virtual bool HideChrome { get; set; }

        [Required]
        [DisplayName("Status")]
        public virtual string Status { get; set; }
        public virtual string Author { get; set; }
        public virtual string RevisionAuthor { get; set; }
        
        [DataType("Comments")]
        public virtual IList<Comment> Comments { get; set; }
        [DataType("Pingbacks")]
        public virtual IList<Pingback> Pingbacks { get; set; }
        public virtual int PingbackCount { get; set; }

        public virtual IFutureValue<Entry> Entry { get; internal set; }

        [DisplayName("Tags")]
        [StringLength(100)]
        [Description("Comma-separated tags that will appear in the &lt;meta&gt; tags of the page.")]
        [HintSize(HintSize.Large)]
        [DataType("TagsList")]
        public virtual string TagsCommaSeparated
        {
            get
            {
                return tagsString ?? string.Join(",", Tags.Select(x => x.Name));
            }
            set
            {
                tagsString = value;
            }
        }

        [DisplayName("Selected tags")]
        [Description("Comma-separated tags that will appear in the &lt;meta&gt; tags of the page.")]
        [HintSize(HintSize.Large)]
        [DataType("Tags")]
        public virtual IEnumerable<Tag> SelectedTags
        {
            get
            {
                var tagStrings = (TagsCommaSeparated ?? string.Empty).Split(',', ';', ' ')
                    .Select(x => x.Trim().ToLowerInvariant())
                    .Where(x => x.Length > 0);
                return tagStrings.Select(s => AllTags.FirstOrDefault(t => t.Name.Trim() == s.Trim()) ?? new Tag { Name = s });
            }
            set
            {
                TagsCommaSeparated = string.Join(",", value.Select(x => x.Name));
            }
        }
        [DataType("Tags")]
        public virtual IEnumerable<Tag> AllTags { get; set; }

    }
}
