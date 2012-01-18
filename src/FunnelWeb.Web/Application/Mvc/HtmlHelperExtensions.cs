using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            return html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }
    }
}