using System;
using FunnelWeb.Web.Application.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Application.Extensions
{
    public class MarkupExtensionsTests
    {
        [Test]
        public void QualifyLinksProcessesVirtualDirectoryCorrectly()
        {
            const string link = "<li><a href=\"/projects\" /></li>";
            var htmlHelper = HtmlHelperBuilder.GetHtmlHelper();
            htmlHelper.ViewContext.HttpContext.Request.Url.Returns(new Uri("http://localhost/virtualdir"));
            htmlHelper.ViewContext.HttpContext.Request.Headers.Add("Host","localhost");
            htmlHelper.ViewContext.HttpContext.Request.ApplicationPath.Returns("VirtualDir");

            // act
            var mvcString = htmlHelper.QualifyLinks(link);

            string actual = mvcString.ToString();
            const string expected = "<li><a href=\"http://localhost/virtualdir/projects\" /></li>";
            Assert.AreEqual(expected, actual);
        }
    }
}