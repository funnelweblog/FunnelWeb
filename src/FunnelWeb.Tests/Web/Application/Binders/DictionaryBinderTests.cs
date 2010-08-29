using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Tests.Helpers;
using FunnelWeb.Web.Application.Binders;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Application.Binders
{
    public class DictionaryBinderTests
    {
        public class Settings_posted_via_form_must_be_turned_into_dictionaries_by_prefix : Specification<ControllerContext>
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
                    { "setting_Title", "Paul's Blog" }, 
                    { "setting_Description", "The worst place on the internet"},
                    { "foo_Bar", "123"},
                });
                return controller;
            }

            public override void When()
            {
                Result = (Dictionary<string, string>)new DictionaryBinder().BindModel(Subject,
                    new ModelBindingContext
                    {
                        ModelName = "setting"
                    });
            }

            protected Dictionary<string, string> Result { get; set; }

            [Then]
            public void ShouldRecogniseAllPostcodes()
            {
                Assert.AreEqual(2, Result.Keys.Count);
                Assert.AreEqual("Paul's Blog", Result["Title"]);
                Assert.AreEqual("The worst place on the internet", Result["Description"]);
            }
        }
    }
}
