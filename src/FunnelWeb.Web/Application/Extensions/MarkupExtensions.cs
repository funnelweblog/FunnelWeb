using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using FunnelWeb.Model;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Application.Views;

namespace FunnelWeb.Web.Application.Extensions
{
    public static class MarkupExtensions
    {
        public static MvcHtmlString Version(this HtmlHelper html)
        {
            return MvcHtmlString.Create(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        #region URL's

        public static MvcHtmlString Qualify(this HtmlHelper html, MvcHtmlString url)
        {
            return Qualify(html, url.ToHtmlString());
        }

        public static MvcHtmlString Qualify(this HtmlHelper html, string url)
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
            return MvcHtmlString.Create(url);
        }

        public static MvcHtmlString QualifyVersion(this HtmlHelper html, string url)
        {
            var result = html.Qualify(url);
            return MvcHtmlString.Create(result + "?r=" + Assembly.GetExecutingAssembly().GetName().Version.Build);
        }

        public static MvcHtmlString ActionUrl(this HtmlHelper html, string actionName, object values)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext, html.RouteCollection);
            var result = url.Action(actionName, "Wiki", values);
            return MvcHtmlString.Create(result.ToLower(CultureInfo.InvariantCulture));
        }

        public static MvcHtmlString ActionUrl(this HtmlHelper html, string actionName, string controllerName, object values)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext, html.RouteCollection);
            var result = url.Action(actionName, controllerName, values);
            return MvcHtmlString.Create(result.ToLower(CultureInfo.InvariantCulture));
        }

        public static IHtmlString Gravatar(this HtmlHelper html, string emailAddress)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(emailAddress));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            var id = sBuilder.ToString();
            return MvcHtmlString.Create(string.Format("http://www.gravatar.com/avatar.php?gravatar_id={0}&amp;rating=PG&amp;size=50&amp;default=identicon", 
                id
                ));
        }

        public static IHtmlString UrlLink(this HtmlHelper html, string url, string text, string @class = "")
        {
            if (string.IsNullOrEmpty(url) || (url.StartsWith("http") && url.Length < 8))
            {
                return MvcHtmlString.Create(string.Format("<span class\"{0}\">{1}</span>",
                    @class,
                    html.Encode(text ?? string.Empty).Trim())
                );
            }
            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }
            return MvcHtmlString.Create(string.Format("<a href=\"{0}\" class=\"{1}\">{2}</a>",
                html.AttributeEncode(url),
                @class,
                html.Encode(text ?? string.Empty).Trim()
                ));
        }

        #endregion

        #region Text

        public static IHtmlString Date(this HtmlHelper html, object value)
        {
            var date = (DateTime)value;
            return MvcHtmlString.Create(string.Format("<span class=\"date\" title=\"{0}\">{1}</span>",
                date.ToString("dd MMM, yyyy HH:mm"),
                date.ToString("dd MMM, yyyy hh:mm tt")));
        }

        public static IHtmlString DateWithoutTime(this HtmlHelper html, object value)
        {
            var date = (DateTime)value;
            return MvcHtmlString.Create(string.Format("<span class=\"date\" title=\"{0}\">{1}</span>",
                date.ToString("dd MMM, yyyy"),
                date.ToString("dd MMM, yyyy")));
        }

        //public static string DateRssFormat(this HtmlHelper html, object value)
        //{
        //    var date = (DateTime)value;
        //    return date.ToString("yyyy-MM-dd") + "T" + date.ToString("HH:mm:ss") + "Z";
        //}

        public static MvcHtmlString Markdown(this HtmlHelper html, object content, bool sanitize)
        {
            var text = (content ?? string.Empty).ToString();
            var markdown = new MarkdownRenderer(sanitize, html.ViewContext.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));
            text = markdown.Render(text);
            return MvcHtmlString.Create(text);
        }

        public static MvcHtmlString TextilizeList(this HtmlHelper html, object content)
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
            return MvcHtmlString.Create(text);
        }

        static Regex keyword = new Regex("^-?[_a-zA-Z]+[_a-zA-Z0-9-]*$", RegexOptions.Compiled);
        public static IEnumerable<MvcHtmlString> CssKeywordsFor(this HtmlHelper html, Entry entry)
        {
            return from k in entry.Tags.Select(x => x.Name)
                   let w = k.Trim()
                   where keyword.IsMatch(w)
                   select MvcHtmlString.Create("keyword-" + w);
        }

        #endregion

        #region Hint

        public static IHtmlString HintFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> property)
        {
            var message = "";
            WhenEncountering<DescriptionAttribute>(property, d => message = d.Description);

            message = (message ?? string.Empty).Trim();
            if (message.Length == 0)
            {
                return new HtmlString(string.Empty);                
            }

            return new HtmlString("<span class='hint'>" + message + "</span>");
        }

        #endregion

        #region Settings

        public static FunnelWebSettings Settings(this HtmlHelper helper)
        {
            return DependencyResolver.Current.GetService<ISettingsProvider>().GetSettings<FunnelWebSettings>();
        }

        #endregion

        private static void WhenEncountering<TAttribute>(LambdaExpression expression, Action<TAttribute> callback)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
            {
                var unary = expression.Body as UnaryExpression;
                if (unary == null)
                    return;

                member = unary.Operand as MemberExpression;
            }
            foreach (var instance in member.Member.GetCustomAttributes(true).OfType<TAttribute>())
            {
                callback(instance);
            }
        }

        public static IDictionary<string, object> AttributesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var attributes = new RouteValueDictionary();
            attributes.Add("class", "");

            WhenEncountering<StringLengthAttribute>(expression, att => attributes["maxlength"] = att.MaximumLength);
            WhenEncountering<HintSizeAttribute>(expression, att =>
                {
                    attributes["class"] += att.Size.ToString().ToLowerInvariant() + " ";
                });

            attributes["class"] = attributes["class"].ToString().Trim();
            return attributes;
        }

		/// <summary>
		/// Create an action link to an action in the Admin area.
		/// </summary>
		public static MvcHtmlString AdminActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string adminControllerName)
		{	// http://stackoverflow.com/questions/2036305/how-to-specify-an-area-name-in-an-action-link
			return htmlHelper.ActionLink(linkText, actionName, adminControllerName, new {Area = "Admin"}, new {});
		}

		public static string ThemePath(this HtmlHelper helper)
		{
			return "~/Themes/" + helper.Settings().Theme;
		}
    }
}
