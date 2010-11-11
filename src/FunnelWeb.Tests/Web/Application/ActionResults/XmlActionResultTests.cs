using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FunnelWeb.Web.Application.Mvc.ActionResults;
using NUnit.Framework;
using System.Xml.Linq;

namespace FunnelWeb.Tests.Web.Application.ActionResults
{
    [TestFixture] 
    public class XmlActionResultTests
    {
        [Test]
        public void XmlActionResultTests_Result_Content_Type_Is_Xml()
        {
            //Arrange
            var actionResult = new XmlActionResult(new XDocument());

            //Act

            //Assert
            Assert.AreEqual("text/xml", actionResult.ContentType);
        }

        [Test]
        public void XmlActionResultTests_Cant_Pass_Null()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws(typeof(ArgumentNullException), () => new XmlActionResult(null));
        }

        [Test]
        public void XmlActionResultTests_Default_Encoding_UTF8()
        {
            //Arrange
            var actionResult = new XmlActionResult(new XDocument());
            //Act

            //Assert
            Assert.AreEqual(System.Text.Encoding.UTF8, actionResult.Encoding);
        }
    }
}
