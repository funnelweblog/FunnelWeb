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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Model;
using FunnelWeb.Mvc;
using FunnelWeb.Settings;
using FunnelWeb.Utilities;
using FunnelWeb.Web.Application.Markup;

namespace FunnelWeb.Web.Application.Extensions
{
	public static class MarkupExtensions
	{
		public static MvcHtmlString Version(this HtmlHelper html)
		{
			return MvcHtmlString.Create(Assembly.GetExecutingAssembly().GetName().Version.ToString());
		}

		#region URL's

		/// <summary>
		/// Fixes up any relative links so they work in all scenarios
		/// </summary>
		/// <param name="html"></param>
		/// <param name="htmlToProcess"></param>
		/// <returns></returns>
		public static MvcHtmlString QualifyLinks(this HtmlHelper html, string htmlToProcess)
		{
			foreach (Match match in Regex.Matches(htmlToProcess, "href=['\"](?<url>.*?)['\"]"))
			{
				var urlGroup = match.Groups["url"];
				var url = urlGroup.Value;

				if (url[0] == '~')
					url = url.Substring(1);

				if (!url.StartsWith("/")) continue;

				var mvcHtmlString = html.Qualify(url);
				string newValue = mvcHtmlString.ToString();
				htmlToProcess = htmlToProcess.Replace(urlGroup.Value, newValue);
			}

			return MvcHtmlString.Create(htmlToProcess);
		}

		public static MvcHtmlString Qualify(this HtmlHelper html, MvcHtmlString url)
		{
			return Qualify(html, url.ToHtmlString());
		}

		public static MvcHtmlString Qualify(this HtmlHelper html, string url)
		{
			var prefix = html.ViewContext.HttpContext.Request.GetBaseUrl().ToString().TrimEnd('/');

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
			var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(emailAddress.ToLower()));
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

		public static IHtmlString CommentedAtRevision(this HtmlHelper html, Comment comment, EntryRevision revision)
		{
			if (Settings(html).EnablePublicHistory == false)
				return MvcHtmlString.Empty;

			if (comment.EntryRevisionNumber != revision.RevisionNumber)
			{
				if (comment.EntryRevisionNumber == comment.Entry.LatestRevision.RevisionNumber)
					return MvcHtmlString.Create("@ " +
							html.ActionLink("latest",
									"Page", "Wiki",
											new { page = comment.Entry.Name },
											new { }));
				return MvcHtmlString.Create("@ " +
						html.ActionLink(string.Format("version {0}", comment.EntryRevisionNumber),
								"Page", "Wiki",
										new { page = comment.Entry.Name, revision = comment.EntryRevisionNumber },
										new { }));
			}
			return MvcHtmlString.Empty;
		}

		#endregion

		#region Text

		public static IHtmlString Date(this HtmlHelper html, DateTime value)
		{
			return MvcHtmlString.Create(value.ToString("yyyy-MM-dd hh:mm"));
		}

		public static IHtmlString DateWithoutTime(this HtmlHelper html, DateTime value)
		{
			return MvcHtmlString.Create(value.ToString("yyyy-MM-dd"));
		}

		//public static IHtmlString Date(this HtmlHelper html, object value)
		//{
		//	var date = (DateTime)value;
		//	return MvcHtmlString.Create(string.Format("<span class=\"date\" title=\"{0}\">{1}</span>",
		//			date.ToString("dd MMM, yyyy HH:mm"),
		//			date.ToString("dd MMM, yyyy hh:mm tt")));
		//}

		//public static IHtmlString DateWithoutTime(this HtmlHelper html, object value)
		//{
		//	var date = (DateTime)value;
		//	return MvcHtmlString.Create(string.Format("<span class=\"date\" title=\"{0}\">{1}</span>",
		//			date.ToString("dd MMM, yyyy"),
		//			date.ToString("dd MMM, yyyy")));
		//}

		public static MvcHtmlString RenderTrusted(this HtmlHelper html, object content, string format)
		{
			var renderer = DependencyResolver.Current.GetService<IContentRenderer>();
			var rendered = renderer.RenderTrusted((content ?? string.Empty).ToString(), format, html);
			return MvcHtmlString.Create(rendered);
		}
		public static MvcHtmlString RenderUntrusted(this HtmlHelper html, object content, string format)
		{
			var renderer = DependencyResolver.Current.GetService<IContentRenderer>();
			var rendered = renderer.RenderUntrusted((content ?? string.Empty).ToString(), format, html);
			return MvcHtmlString.Create(rendered);
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

		static readonly Regex keyword = new Regex("^-?[_a-zA-Z]+[_a-zA-Z0-9-]*$", RegexOptions.Compiled);
		static readonly Regex keywordReplace = new Regex(@"[ &\.#]+", RegexOptions.Compiled);
		public static IEnumerable<MvcHtmlString> CssKeywordsFor(this HtmlHelper html, EntrySummary entry)
		{
			var tags = entry.TagsCommaSeparated.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			return from tagName in tags
						 select CssKeywordsForTag(html, tagName);
		}

		public static IEnumerable<MvcHtmlString> CssKeywordsFor(this HtmlHelper html, EntryRevision entry)
		{
			return html.CssKeywordsFor(entry.Tags);
		}

		private static IEnumerable<MvcHtmlString> CssKeywordsFor(this HtmlHelper html, IEnumerable<Tag> tags)
		{
			return from tagName in tags.Select(x => x.Name)
						 select CssKeywordsForTag(html, tagName);
		}

		public static MvcHtmlString CssKeywordsForTag(this HtmlHelper html, string tag)
		{
			var fixedTag = keywordReplace.Replace(tag, "-");
			return keyword.IsMatch(fixedTag)
					? MvcHtmlString.Create("keyword-" + fixedTag)
					: MvcHtmlString.Empty;
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
			var settingsProvider = DependencyResolver.Current.GetService<ISettingsProvider>();

			// A database upgrade is required, lets just use the default settings
			if (DependencyResolver.Current.GetService<IDatabaseUpgradeDetector>().UpdateNeeded())
				return settingsProvider.GetDefaultSettings<FunnelWebSettings>();

			return settingsProvider.GetSettings<FunnelWebSettings>();
		}

		public static AccessControlServiceSettings AcsSettings(this HtmlHelper helper)
		{
			var settingsProvider = DependencyResolver.Current.GetService<ISettingsProvider>();

			return
				DependencyResolver.Current.GetService<IDatabaseUpgradeDetector>().UpdateNeeded() ?
				// A database upgrade is required, lets just use the default settings
				settingsProvider.GetDefaultSettings<AccessControlServiceSettings>() :
				settingsProvider.GetSettings<AccessControlServiceSettings>();
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
			var attributes = new RouteValueDictionary { { "class", "" } };

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
		{
			// http://stackoverflow.com/questions/2036305/how-to-specify-an-area-name-in-an-action-link
			return htmlHelper.ActionLink(linkText, actionName, adminControllerName, new { Area = "Admin" }, new { });
		}

		public static string ThemePath(this HtmlHelper helper)
		{
			return "~/Themes/" + helper.Settings().Theme;
		}
	}
}