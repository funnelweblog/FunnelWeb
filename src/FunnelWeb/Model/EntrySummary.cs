using System;
using System.ComponentModel.DataAnnotations;
using FunnelWeb.Model.Strings;

namespace FunnelWeb.Model
{
	public class EntrySummary
	{
		public int Id { get; set; }

		public PageName Name { get; set; }
	
		public string Title { get; set; }

		[DataType("Markdown")]
		public string Summary { get; set; }

		public string Status { get; set; }

		public int CommentCount { get; set; }

		public bool HideComments { get; set; }

		public string MetaDescription { get; set; }

		[DataType("PublishedDate")]
		public DateTime Published { get; set; }

        public string Author { get; set; }

		public DateTime LastRevised { get; set; }

		[DataType("TagsList")]
		public string TagsCommaSeparated { get; set; }
	}
}