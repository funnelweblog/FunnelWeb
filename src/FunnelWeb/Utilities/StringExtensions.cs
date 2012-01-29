using System.Text.RegularExpressions;

namespace FunnelWeb.Utilities
{
    public static class StringExtensions
    {
        public static string Slugify(this string value)
        {
            // http://stackoverflow.com/questions/2920744/url-slugify-alrogithm-in-c

            var bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = System.Text.Encoding.ASCII.GetString(bytes).ToLower(); 

            //// invalid chars           
            value = Regex.Replace(value, @"[^a-z0-9\s-]", "");
            //// convert multiple spaces into one space   
            value = Regex.Replace(value, @"\s+", " ").Trim();
            //// cut and trim 
            value = value.Substring(0, value.Length <= 45 ? value.Length : 45).Trim();
            value = Regex.Replace(value, @"\s", "-"); // hyphens   
            return value; 
        }
    }
}
