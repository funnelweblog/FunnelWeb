using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Routing;

namespace FunnelWeb.Web.Application.Mvc
{
    /// <summary>
    /// Implements a regex route
    /// </summary>
    /// <remarks>
    /// <example>
    /// <![CDATA[
    ///     routes.Add(new RegexRoute("^(?<controller>.*?)//(?<action>.*?)(.aspx)?$", new MvcRouteHandler())
    ///     {
    ///       GetVirtualPath = ((route, context, dictionary) => 
    ///                            new VirtualPathData(route, String.Format("{0}/{1}/", dictionary["controller"],dictionary["action"])))
    ///     });
    ///  ]]>
    /// </example>
    /// </remarks>
    public class RegexRoute : Route
    {
        protected readonly Regex UrlRegex;
        public Func<RouteBase, RequestContext, RouteValueDictionary, string> GenerateUrls = null;
        public RegexRoute(string urlPattern, IRouteHandler routeHandler)
            : this(urlPattern, null, routeHandler)
        { }

        public RegexRoute(string urlPattern, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(null, defaults, routeHandler)
        {
            UrlRegex = new Regex(urlPattern, RegexOptions.Compiled);
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var requestUrl = GetRequestUrl(httpContext);
            var match = UrlRegex.Match(requestUrl);
            RouteData data = null;
            if (match.Success)
            {
                data = new RouteData(this, RouteHandler);
                if (null != Defaults)
                {
                    foreach (var def in Defaults)
                    {
                        data.Values[def.Key] = def.Value;
                    }
                }
                for (var i = 1; i < match.Groups.Count; i++)
                {
                    var group = match.Groups[i];
                    if (group.Success)
                    {
                        var key = UrlRegex.GroupNameFromNumber(i);
                        if (!string.IsNullOrEmpty(key) && !Char.IsNumber(key, 0)) // only consider named groups
                        {
                            data.Values[key] = group.Value;
                        }
                    }
                }
            }
            return data;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            string url;
            if (GenerateUrls != null)
            {
                url = GenerateUrls(this, requestContext, values);
                if (url.StartsWith("/"))
                    url = url.Substring(1);
            }
            else
                return base.GetVirtualPath(requestContext, values);

            //was this url valid?
            if (!UrlRegex.IsMatch(url))
                return null;

            var data = new VirtualPathData(this, url);
            if (DataTokens != null)
            {
                foreach (KeyValuePair<string, object> pair in DataTokens)
                {
                    data.DataTokens[pair.Key] = pair.Value;
                }
            }

            Url = url;

            return data;
        }

        protected static string GetRequestUrl(HttpContextBase httpContext)
        {
            return httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;
        }
    }
}