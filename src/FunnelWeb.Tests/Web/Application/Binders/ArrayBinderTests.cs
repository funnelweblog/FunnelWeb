using System.Collections.Specialized;
using System.Web.Mvc;
using FunnelWeb.Tests.Helpers;
using FunnelWeb.Web.Application.Binders;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Application.Binders
{
    public class ArrayBinderTests
    {
        public class Form_values_split_by_commas_will_be_arrayed : Specification<ControllerContext>
        {
            public override ControllerContext Given()
            {
                var controller = Substitute.For<ControllerContext>();
                controller.HttpContext.Request.Form.Returns(new NameValueCollection()
                {
                    { "postcodes", ",4005,,,2009,," }, 
                    { "postcodes", "5022"},
                    { "luckyNumbers", "14,5"}
                });
                return controller;
            }

            public override void When()
            {
                Result = (int[])new ArrayBinder().BindModel(Subject,
                    new ModelBindingContext
                    {
                        ModelName = "postcodes"
                    });
            }

            protected int[] Result { get; set; }

            [Then]
            public void ShouldRecogniseAllPostcodes()
            {
                Assert.AreEqual(3, Result.Length);
                Assert.AreEqual(4005, Result[0]);
                Assert.AreEqual(2009, Result[1]);
                Assert.AreEqual(5022, Result[2]);
            }
        }
    }
}
