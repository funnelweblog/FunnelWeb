using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.Model.Strings;
using FunnelWeb.Web.Application.Mvc;
using System.Web.Mvc;

namespace FunnelWeb.Web.Areas.Admin.Views.WikiAdmin
{
    public class EditModel
    {
        public EditModel()
        {
            
        }

        public EditModel(PageName page, int originalEntryId, IEnumerable<Tag> tags)
        {
            Page = page;
            IsNew = originalEntryId == 0;
            OriginalEntryId = originalEntryId;
            AllTags = tags.ToList();
        }

        public List<Tag> AllTags { get; set; }

        public bool IsNew { get; set; }

        public int OriginalEntryId { get; set; }

        [Required]
        [DisplayName("SLUG")]
        [StringLength(200)]
        [Description("This will form the URL to your page.")]
        [RegularExpression("[a-z0-9\\-\\/]+", ErrorMessage = "Page names can only include lowercase alpha characters, numbers, dashes and forward slashes (/)")]
        [HintSize(HintSize.Medium)]
        public string Page { get; set; }

        [Required]
        [DisplayName("Title")]
        [StringLength(200)]
        [Description("This appears at the top of this page and on the home page.")]
        [HintSize(HintSize.Medium)]
        public string Title { get; set; }

        [DisplayName("Meta-Title")]
        [StringLength(255)]
        [Description("This appears at the top of the browser tab and is used by search engines.")]
        [HintSize(HintSize.Medium)]
        public string MetaTitle { get; set; }

		[DisplayName("Page Template")]
		[StringLength(20)]
		[Description("This will change how your page looks.")]
		[RegularExpression("[a-zA-Z0-9\\-]+", ErrorMessage = "Page templates can only include lowercase alpha characters, numbers and dashes")]
		[HintSize(HintSize.Medium)]
		public string PageTemplate { get; set; }

        [Required]
        [DisplayName("Publish date")]
        [Description("This page will not appear in any feeds until after the date above.")]
        [HintSize(HintSize.Medium)]
        [DataType(DataType.Date)]
        [RegularExpression("[0-9]{4}-[0-9]{1,2}-[0-9]{1,2}", ErrorMessage = "Please enter a date in YYYY-MM-DD format.")]
        public string PublishDate { get; set; }

        [DisplayName("Summary")]
        [StringLength(150)]
        [Description("A short description that will appear on the home page, and in the meta-description shown to search engines.")]
        [HintSize(HintSize.Large)]
        public string MetaDescription { get; set; }

        [DisplayName("Introduction")]
        [StringLength(1000)]
        [Description("An introduction that will appear at the top or right of the page. Use markdown or HTML.")]
        [HintSize(HintSize.Large)]
        public string Sidebar { get; set; }

        [DisplayName("Tags")]
        [StringLength(100)]
        [Description("Comma-separated tags that will appear in the &lt;meta&gt; tags of the page.")]
        [HintSize(HintSize.Large)]
        [DataType("Tags")]
        public string TagsString { get; set; }

        [Required]
        [DisplayName("Format")]
        [Description("Choose the markup format you'd like to use for this page")]
        public string Format { get; set; }

        [Required]
        [DisplayName("Content")]
        [DataType("Markdown")]
        [HintSize(HintSize.Large)]
        [AllowHtml]
        public string Content { get; set; }

        [DisplayName("Change summary")]
        [StringLength(300)]
        [Description("A brief overview of what was changed and why. This will appear on the page history.")]
        [HintSize(HintSize.Large)]
        public string ChangeSummary { get; set; }

        [DisplayName("Disable Comments")]
        [Description("If checked, users will not be able to post comments on this page")]
        public bool DisableComments { get; set; }

        [DisplayName("Hide chrome")]
        [Description("If checked, the page title, date, history and so on will be hidden.")]
        public bool HideChrome { get; set; }

        [Required]
        [DisplayName("Status")]
        public string Status { get; set; }

        [Description("Comma-separated tags that will appear in the &lt;meta&gt; tags of the page.")]
        [HintSize(HintSize.Large)]
        [DataType("Tags")]
        public IEnumerable<Tag> SelectedTags
        {
            get
            {
                var tagStrings = (TagsString ?? string.Empty).Split(',', ';', ' ')
                    .Select(x => x.Trim().ToLowerInvariant())
                    .Where(x => x.Length > 0);
                return tagStrings.Select(s => AllTags.FirstOrDefault(t => t.Name == s) ?? new Tag {Name = s});
            }
            set
            {
                TagsString = string.Join(", ", value.Select(x => x.Name));
            }
        }
    }
}