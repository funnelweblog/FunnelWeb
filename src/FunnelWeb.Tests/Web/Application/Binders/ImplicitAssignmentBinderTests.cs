using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Model.Strings;
using FunnelWeb.Tests.Helpers;
using FunnelWeb.Web.Application.Mvc.Binders;
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
                        ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(PageName)),
                        ValueProvider = new NameValueCollectionValueProvider(Subject.HttpContext.Request.Form, null)
                    });
            }

            protected PageName Result { get; set; }

            [Then]
            public void ShouldConvertFromStringToPageName()
            {
                Assert.AreEqual((PageName)"paul-blog", Result);
            }
        }
    }
}
