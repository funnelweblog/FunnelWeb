using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using FunnelWeb.Web.Application.Views;

namespace FunnelWeb.Web.Application.Extensions
{
    public static class MarkupExtensions
    {
        #region URL's

        public static string Qualify(this HtmlHelper html, MvcHtmlString url)
        {
            return Qualify(html, url.ToHtmlString());
        }

        public static string Qualify(this HtmlHelper html, string url)
        {
            var requestUrl = html.ViewContext.HttpContext.Request.Url;
            var prefix = requestUrl.GetLeftPart(UriPartial.Authority);
            if (url.StartsWith("<a"))
            {
                url = url.Replace("href=\"/", "href=\"" + prefix + "/").Replace("href='/", "href='" + prefix + "/");
            }
            else 
            {
                url = !url.StartsWith("/") ? prefix + "/" + url : prefix + url;
                url = url.ToLower(CultureInfo.InvariantCulture);
            }
            return url;
        }

        public static string QualifyVersion(this HtmlHelper html, string url)
        {
            var result = html.Qualify(url);
            return result + "?r=" + Assembly.GetExecutingAssembly().GetName().Version.Build;
        }

        public static string ActionUrl(this HtmlHelper html, string actionName, object values)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext, html.RouteCollection);
            var result = url.Action(actionName, values);
            return result.ToLower(CultureInfo.InvariantCulture);
        }

        public static string ActionUrl(this HtmlHelper html, string actionName, string controllerName, object values)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext, html.RouteCollection);
            var result = url.Action(actionName, controllerName, values);
            return result.ToLower(CultureInfo.InvariantCulture);
        }

        public static string Gravatar(this HtmlHelper html, string emailAddress)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(emailAddress));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            var id = sBuilder.ToString();
            return string.Format("http://www.gravatar.com/avatar.php?gravatar_id={0}&amp;rating=PG&amp;size=50&amp;default=identicon", 
                id
                );
        }

        public static string UrlLink(this HtmlHelper html, string url, string text)
        {
            if (string.IsNullOrEmpty(url) || (url.StartsWith("http") && url.Length < 8))
            {
                return html.Encode(text ?? string.Empty).Trim();
            }
            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }
            return string.Format("<a href=\"{0}\">{1}</a>",
                html.AttributeEncode(url),
                html.Encode(text ?? string.Empty).Trim()
                );
        }

        #endregion

        #region Text

        public static string Date(this HtmlHelper html, object value)
        {
            var date = (DateTime)value;
            return string.Format("<span class=\"date\" title=\"{0}\">{1}</span>",
                date.ToString("dd MMM, yyyy HH:mm"),
                date.ToString("dd MMM, yyyy hh:mm tt"));
        }

        //public static string DateRssFormat(this HtmlHelper html, object value)
        //{
        //    var date = (DateTime)value;
        //    return date.ToString("yyyy-MM-dd") + "T" + date.ToString("HH:mm:ss") + "Z";
        //}

        public static string Markdown(this HtmlHelper html, object content, bool sanitize)
        {
            var text = (content ?? string.Empty).ToString();
            var markdown = new MarkdownRenderer(sanitize, html.ViewContext.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));
            text = markdown.Render(text);
            return text;
        }

        public static string TextilizeList(this HtmlHelper html, object content)
        {
            var text = (content ?? string.Empty).ToString();
            text = string.Join("\n",
                text.Split('\n')
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.StartsWith("- ") ? x : "- " + x)
                    .ToArray()
                );
            text = text.Replace("\n", "<br />\n");
            return text;
        }

        #endregion
    }
}
