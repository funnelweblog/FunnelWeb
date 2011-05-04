using System;
using System.ComponentModel.DataAnnotations;

namespace FunnelWeb.Model.Strings
{
    /// <summary>
    /// StringLength does a explicit cast to string, so it will not work with FunnelWeb's EnforcedString class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EnforcedStringLengthAttribute : StringLengthAttribute
    {
        public EnforcedStringLengthAttribute(int maximumLength) : base(maximumLength)
        {
        }

        public override bool IsValid(object value)
        {
            var enforcedString = (EnforcedString)value;

            return base.IsValid(enforcedString.ToString());
        }
    }
}