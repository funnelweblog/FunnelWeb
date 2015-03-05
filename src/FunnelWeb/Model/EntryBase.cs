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
using FunnelWeb.Settings;

namespace FunnelWeb.Model
{
    public class EntryBase
    {
        public EntryBase()
        {
            Title = string.Empty;
            Name = string.Empty;
            Summary = string.Empty;
            MetaDescription = string.Empty;
            Author = string.Empty;
            Published = DateTime.UtcNow;
        }

        public virtual int Id { get; set; }

        [Required]
        [DisplayName("SLUG")]
        [EnforcedStringLength(200)] //StringLength throws a cast exception for PageName
        [Description("This will form the URL to your page.")]
        [RegularExpression("[a-z0-9\\-\\/]+", ErrorMessage = "Page names can only include lowercase alpha characters, numbers, dashes and forward slashes (/)")]
        [HintSize(HintSize.Scaled)]
        public virtual PageName Name { get; set; }

        [Required]
        [DisplayName("Title")]
        [StringLength(200)]
        [Description("This appears at the top of this page and on the home page.")]
        [HintSize(HintSize.Scaled)]
        public virtual string Title { get; set; }

        [DisplayName("Introduction")]
        [StringLength(1000)]
        [Description("An introduction that will appear at the top or right of the page. Use markdown or HTML.")]
        [HintSize(HintSize.Scaled)]
        public virtual string Summary { get; set; }

        public virtual string Status { get; set; }
        
        public virtual int CommentCount { get; set; }

        [DisplayName("Summary")]
        [StringLength(150)]
        [Description("A short description that will appear on the home page, and in the meta-description shown to search engines.")]
        [HintSize(HintSize.Scaled)]
        public virtual string MetaDescription { get; set; }

        [DataType("PublishedDate")]
        public virtual DateTime Published { get; set; }

        [DataType("Tags")]
        public virtual IList<Tag> Tags { get; set; }
        
        public virtual string Author { get; set; }

        [DisplayName("Tags")]
        [StringLength(100)]
        [Description("Comma-separated tags that will appear in the &lt;meta&gt; tags of the page.")]
        [HintSize(HintSize.Scaled)]
        [DataType("TagsList")]
        public virtual string TagsCommaSeparated
        {
            get
            {
                return Tags == null ? "" : string.Join(",", Tags.Select(x => x.Name));
            }

            set
            {
                string[] tagStrings = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                
                Tags = new List<Tag>();
                foreach (string tagString in tagStrings)
                {
                    Tags.Add(new Tag() { Name = tagString });
                }
            }
        }

        public virtual string DisqusIdentifier 
        {
            get
            {
                //return (this.Title == null ? "" : (this.Title + " - ")) + SettingsProvider().GetSettings<FunnelWebSettings>().SiteTitle;
                return (this.Title == null ? "" : this.Title);
            }
        }
    }
}
