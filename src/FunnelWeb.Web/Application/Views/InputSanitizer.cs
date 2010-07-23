using System.Text.RegularExpressions;

namespace FunnelWeb.Web.Application.Views
{
    public static class InputSanitizer
    {
        private static readonly Regex AnyTag = new Regex("<[^>]*(>|$)", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        private static readonly Regex Whitelist = new Regex(@"
            ^</?(b(lockquote)?|code|d(d|t|l|el)|table|tr|td|thead|tbody|em|h(1|2|3|4|5)|i|kbd|li|ol|p(re)?|s(ub|up|trong|trike)?|ul)>$|
            ^<(b|h)r\s?/?>$", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        private static readonly Regex WhitelistHyperlinks = new Regex(@"
            ^<a\s
            href=""(\#\d+|(https?|ftp)://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+)""
            (\stitle=""[^""<>]+"")?\s?>$|
            ^</a>$", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        private static readonly Regex WhitelistImage = new Regex(@"
            ^<img\s
            src=""https?://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+""
            (\swidth=""\d{1,3}"")?
            (\sheight=""\d{1,3}"")?
            (\salt=""[^""<>]*"")?
            (\stitle=""[^""<>]*"")?
            \s?/?>$", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Sanitize any potentially dangerous tags from the provided raw HTML input using a whitelist based approach, leaving the "safe" HTML tags.
        /// </summary>
        public static string Sanitize(string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            // Match every HTML tag in the input
            var tags = AnyTag.Matches(html);
            for (var i = tags.Count - 1; i > -1; i--)
            {
                var tag = tags[i];
                var tagname = tag.Value.ToLowerInvariant();

                if ((Whitelist.IsMatch(tagname) || WhitelistHyperlinks.IsMatch(tagname) || WhitelistImage.IsMatch(tagname)))
                    continue;

                html = html.Remove(tag.Index, tag.Length);
            }

            return html;
        }
    }
}
