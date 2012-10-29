using System;
using System.ComponentModel.DataAnnotations;

namespace FunnelWeb.Web.Application.Mvc
{
    public class ValidUrlAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var text = (value ?? string.Empty).ToString();
            if (text.Length == 0)
                return true;

            Uri uri;
            return Uri.TryCreate(text, UriKind.Absolute, out uri);
        }
    }
}