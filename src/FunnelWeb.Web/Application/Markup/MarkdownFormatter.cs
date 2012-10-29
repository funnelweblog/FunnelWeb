using System;
using System.Web;
using FunnelWeb.Utilities;

namespace FunnelWeb.Web.Application.Markup
{
    public class MarkdownFormatter : IContentFormatter
    {
        private readonly HttpRequestBase request;

        public MarkdownFormatter(HttpRequestBase request)
        {
            this.request = request;
        }

        public string Format(string content)
        {
            var relativePathUrlPrefix = request.GetOriginalUrl().GetLeftPart(UriPartial.Authority) + request.ApplicationPath;
            var relativePathUrlPrefixLength = relativePathUrlPrefix.Length;

            // This fixes the issue where links such as <a href="/some/thing"></a> are rendered as http://domain//some/thing
            // Notice the double slash after the domain (or application name)
            relativePathUrlPrefix =
                relativePathUrlPrefix[relativePathUrlPrefixLength - 1] == '/' ? relativePathUrlPrefix.Substring(0, relativePathUrlPrefixLength - 1) : relativePathUrlPrefix;

            var renderer = new MarkdownNetByBrianJeremy(relativePathUrlPrefix);
            return renderer.Render(content);
        }
    }
}
