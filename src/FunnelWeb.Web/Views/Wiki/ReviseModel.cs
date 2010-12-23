using System;
using System.ComponentModel.DataAnnotations;

namespace FunnelWeb.Web.Views.Wiki
{
    public class ReviseModel
    {
        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [StringLength(65)]
        [DataType(DataType.Text)]
        public string MetaTitle { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }

        [Required]
        [StringLength(150)]
        [DataType(DataType.Text)]
        public string Brief { get; set; }

        [DataType(DataType.MultilineText)]
        public string Sidebar { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string Keywords { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public string[] FeedIds { get; set; }

        [Required]
        [StringLength(300)]
        [DataType(DataType.MultilineText)]
        public string RevisionComments { get; set; }
    }
}