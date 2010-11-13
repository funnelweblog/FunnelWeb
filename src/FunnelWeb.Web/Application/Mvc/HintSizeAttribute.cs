using System;

namespace FunnelWeb.Web.Application.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HintSizeAttribute : Attribute
    {
        public HintSizeAttribute(HintSize size)
        {
            Size = size;
        }

        public HintSize Size { get; private set; }
    }
}