using System.Collections.Specialized;
using System.Web.Mvc;
using FunnelWeb.Web.Application.Binders;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Application.Binders
{
    public class ArrayBinderTests
    {
        [TestFixture]
        public class WhenActionRequiresIntegerArray
        {
            [Test]
            public void ShouldSplitByCommas()
            {
                var controller = Substitute.For<ControllerContext>();
                controller.HttpContext.Request.Form.Returns(new NameValueCollection()
                {
                    { "postcodes", "4005,2009" }, 
                    { "luckyNumbers", "14,5"}
                });
                var result = (int[])new ArrayBinder().BindModel(controller,
                    new ModelBindingContext
                        {
                        ModelName = "postcodes"
                    });

                Assert.AreEqual(2, result.Length);
                Assert.AreEqual(4005, result[0]);
                Assert.AreEqual(2009, result[1]);
            }

            [Test]
            public void ShouldIgnoreEmptyValues()
            {
                var controller = Substitute.For<ControllerContext>();
                controller.HttpContext.Request.Form.Returns(new NameValueCollection()
                {
                    { "postcodes", ",4005,,,2009," }
                });
                var result = (int[])new ArrayBinder().BindModel(controller,
                    new ModelBindingContext
                    {
                        ModelName = "postcodes"
                    });

                Assert.AreEqual(2, result.Length);
                Assert.AreEqual(4005, result[0]);
                Assert.AreEqual(2009, result[1]);
            }
        }
    }
}
