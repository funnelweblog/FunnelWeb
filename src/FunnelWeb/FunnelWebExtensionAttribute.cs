using System;
using System.ComponentModel.Composition;

namespace FunnelWeb
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FunnelWebExtensionAttribute : ExportAttribute
    {
        public FunnelWebExtensionAttribute()
            : base(typeof(IFunnelWebExtension))
        {
            
        }

        public string FullName { get; set; }
        public string SupportLink { get; set; }
        public string Publisher { get; set; }
    }
}