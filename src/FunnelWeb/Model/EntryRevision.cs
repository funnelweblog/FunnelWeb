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
    public class EntryRevision : EntryBase
    {
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
        [HintSize(HintSize.Scaled)]
        public string ChangeSummary { get; set; }

        public virtual bool IsPriorVersion 
        { 
            get
            {
                return RevisionNumber < LatestRevisionNumber;
            } 
        }

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
        [HintSize(HintSize.Scaled)]
        public virtual string MetaTitle { get; set; }

        [DisplayName("Hide chrome")]
        [Description("If checked, the page title, date, history and so on will be hidden.")]
        public virtual bool HideChrome { get; set; }

        public virtual string RevisionAuthor { get; set; }
        
        [DataType("Comments")]
        public virtual IList<Comment> Comments { get; set; }
        [DataType("Pingbacks")]
        public virtual IList<Pingback> Pingbacks { get; set; }
        public virtual int PingbackCount { get; set; }

        public virtual IFutureValue<Entry> Entry { get; internal set; }

        [DisplayName("Selected tags")]
        [Description("Comma-separated tags that will appear in the &lt;meta&gt; tags of the page.")]
        [HintSize(HintSize.Scaled)]
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
        }

        [DataType("Tags")]
        public virtual IEnumerable<Tag> AllTags { get; set; }

    }
}
