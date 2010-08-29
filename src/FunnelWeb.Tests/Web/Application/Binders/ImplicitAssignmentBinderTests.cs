using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Tests.Helpers;
using FunnelWeb.Web.Application.Binders;
using FunnelWeb.Web.Model.Strings;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Application.Binders
{
    public class ImplicitAssignmentBinderTests
    {
        public class Types_supporting_implicit_conversion_should_be_mapped : Specification<ControllerContext>
        {
            public override ControllerContext Given()
            {
                var controller = Substitute.For<ControllerContext>();
                var httpContext = Substitute.For<HttpContextBase>();
                var httpRequest = Substitute.For<HttpRequestBase>();
                httpContext.Request.Returns(httpRequest);
                controller.HttpContext.Returns(httpContext);
                controller.HttpContext.Request.Form.Returns(new NameValueCollection()
                {
                    { "foo", "Paul Blog" }
                });
                return controller;
            }

            public override void When()
            {
                Result = (PageName)new ImplicitAssignmentBinder().BindModel(Subject,
                    new ModelBindingContext
                    {
                        ModelName = "foo",
                        ModelType = typeof(PageName)
                    });
            }

            protected PageName Result { get; set; }

            [Then]
            public void ShouldConvertFromStringToPageName()
            {
                Assert.AreEqual("paul-blog", Result);
            }
        }
    }
}
