using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Routing;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Helpers
{
    public static class RouteExtensions
    {
        public static void WillRoute(this RouteCollection routes, string requestUrl, object expectations)
        {
            var httpContextMock = Substitute.For<HttpContextBase>();
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpContextMock.Request.Returns(httpRequest);
            httpContextMock.Request.AppRelativeCurrentExecutionFilePath.Returns(requestUrl);

            var routeData = routes.GetRouteData(httpContextMock);
            Assert.IsNotNull(routeData, "Should have found the route");

            var properties = GetProperties(expectations);
            foreach (var property in properties)
            {
                Assert.AreEqual(
                    routeData.Values[property.Name], 
                    property.Value,
                    string.Format(
                        "Expected '{0}', not '{1}' for '{2}'.",
                        property.Value, routeData.Values[property.Name], property.Name)
                    );
            }
        }

        private static IEnumerable<PropertyValue> GetProperties(object o)
        {
            if (o == null) return new PropertyValue[0];
            var results = new List<PropertyValue>();
            var props = TypeDescriptor.GetProperties(o);
            foreach (PropertyDescriptor prop in props)
            {
                var val = prop.GetValue(o);
                if (val == null) continue;
                results.Add(new PropertyValue { Name = prop.Name, Value = val });
            }
            return results;
        }

        private sealed class PropertyValue
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }
    }
}
