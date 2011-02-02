using System.Linq;
using System.Web;
using System.Web.Routing;

namespace FunnelWeb.Web.Application.Mvc
{
    public class NotFromValuesListConstraint : IRouteConstraint
    {
        private readonly string[] _values;

        public NotFromValuesListConstraint(params string[] values)
        {
            _values = values;
        }

        public bool Match(HttpContextBase httpContext,
                          Route route,
                          string parameterName,
                          RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            if (!values.ContainsKey(parameterName))
                return true;

            // Get the value called "parameterName" from the 
            // RouteValueDictionary called "value"
            string value = values[parameterName].ToString();

            // Return true is the list of allowed values contains 
            // this value.
            var match = !_values.Any(value.Contains);
            return match;
        }
    }
}