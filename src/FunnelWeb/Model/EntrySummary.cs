using System;
using System.ComponentModel.DataAnnotations;
using FunnelWeb.Model.Strings;

namespace FunnelWeb.Model
{
	public class EntrySummary : EntryBase
	{
		public DateTime LastRevised { get; set; }
	}
}