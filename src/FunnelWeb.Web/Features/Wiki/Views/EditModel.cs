using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Features.Wiki.Views
{
    public class EditModel
    {
        public EditModel()
        {
            
        }

        public EditModel(PageName page, bool isNew, IEnumerable<Feed> feeds)
        {
            Page = page;
            IsNew = isNew;
            Feeds = feeds.ToList();
        }

        public bool IsNew { get; set; }

        [Required]
        [DisplayName("Name")]
        [StringLength(50)]
        [Description("This will appear in the URL to the page.")]
        [RegularExpression("[a-z0-9\\-]+", ErrorMessage = "Page names can only include lowecase alpha characters, numbers and dashes")]
        [HintSize(HintSize.Medium)]
        public string Page { get; set; }

        [Required]
        [DisplayName("Title")]
        [StringLength(200)]
        [Description("This appears at the top of this page and on the home page.")]
        [HintSize(HintSize.Medium)]
        public string Title { get; set; }

        [Required]
        [DisplayName("Meta-Title")]
        [StringLength(65)]
        [Description("This appears at the top of the browser tab and is used by search engines.")]
        [HintSize(HintSize.Medium)]
        public string MetaTitle { get; set; }

        [Required]
        [DisplayName("Publish date")]
        [Description("This page will not appear in any feeds until after the date above.")]
        [HintSize(HintSize.Medium)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public DateTime? PublishDate { get; set; }

        [Required]
        [DisplayName("Description")]
        [StringLength(150)]
        [Description("A short description that will appear in the &lt;meta&gt; tags of the page, and on the home page as a summary.")]
        [HintSize(HintSize.Large)]
        public string MetaDescription { get; set; }

        [DisplayName("Sidebar")]
        [StringLength(1000)]
        [Description("This will appear at the right of the page. Use it to provide a quick description of the page to users. Use markdown or HTML.")]
        [HintSize(HintSize.Large)]
        public string Sidebar { get; set; }

        [Required]
        [DisplayName("Keywords")]
        [StringLength(100)]
        [Description("Comma-seperated keywords that will appear in the &lt;meta&gt; tags of the page.")]
        [HintSize(HintSize.Large)]
        public string Keywords { get; set; }

        [Required]
        [DisplayName("Content")]
        [DataType("Markdown")]
        [HintSize(HintSize.Large)]
        public string Content { get; set; }

        [Required]
        [DisplayName("Change summary")]
        [StringLength(300)]
        [Description("A brief overview of what was changed and why. This will appear on the page history.")]
        [HintSize(HintSize.Large)]
        public string ChangeSummary { get; set; }

        [DisplayName("Allow comments")]
        [Description("If checked, allows users to post comments on this page.")]
        public bool AllowComments { get; set; }

        public IEnumerable<Feed> Feeds { get; set; }

        public int[] FeedIds { get; set; }
    }
}