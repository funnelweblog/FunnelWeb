using System;
using System.Collections.Specialized;
using System.Web;
using FunnelWeb.Utilities;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Core.Utilities
{
    public class HttpRequestExtensionsTest
    {
        [Test]
        public void LocalTestingUrl()
        {
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri("http://localhost:5102/File.txt"));
            httpRequest.Headers.Returns(new NameValueCollection());
            httpRequest.Headers.Add("Host", "localhost:5102");

            Assert.AreEqual("http://localhost:5102/File.txt", httpRequest.GetOriginalUrl().ToString());
        }

        [Test]
        public void LocalMeTestingUrl()
        {
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri("http://localtest.me/File.txt"));
            httpRequest.Headers.Returns(new NameValueCollection());
            httpRequest.Headers.Add("Host", "localtest.me");

            Assert.AreEqual("http://localtest.me/File.txt", httpRequest.GetOriginalUrl().ToString());
        }

        [Test]
        public void VirtualDirectoryTestingUrl()
        {
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri("http://localtest.me/VirtualDir/File.txt"));
            httpRequest.Headers.Returns(new NameValueCollection());
            httpRequest.Headers.Add("Host", "localtest.me");

            Assert.AreEqual("http://localtest.me/VirtualDir/File.txt", httpRequest.GetOriginalUrl().ToString());
        }

        [Test]
        public void LocalAzureTestingUrl()
        {
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri("http://127.0.0.1:81/File.txt"));
            httpRequest.Headers.Returns(new NameValueCollection());
            httpRequest.Headers.Add("Host", "127.0.0.1");

            Assert.AreEqual("http://127.0.0.1/File.txt", httpRequest.GetOriginalUrl().ToString());
        }

        [Test]
        public void LocalTestingBaseUrl()
        {
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri("http://localhost:5102/File.txt"));
            httpRequest.Headers.Returns(new NameValueCollection());
            httpRequest.Headers.Add("Host", "localhost:5102");

            Assert.AreEqual("http://localhost:5102/", httpRequest.GetBaseUrl().ToString());
        }

        [Test]
        public void LocalMeTestingBaseUrl()
        {
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri("http://localtest.me/File.txt"));
            httpRequest.Headers.Returns(new NameValueCollection());
            httpRequest.Headers.Add("Host", "localtest.me");

            Assert.AreEqual("http://localtest.me/", httpRequest.GetBaseUrl().ToString());
        }

        [Test]
        public void VirtualDirectoryTestingBaseUrl()
        {
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri("http://localtest.me/VirtualDir/File.txt"));
            httpRequest.ApplicationPath.Returns("VirtualDir");
            httpRequest.Headers.Returns(new NameValueCollection());
            httpRequest.Headers.Add("Host", "localtest.me");

            Assert.AreEqual("http://localtest.me/VirtualDir", httpRequest.GetBaseUrl().ToString());
        }

        [Test]
        public void LocalAzureTestingBaseUrl()
        {
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri("http://127.0.0.1:81/File.txt"));
            httpRequest.Headers.Returns(new NameValueCollection());
            httpRequest.Headers.Add("Host", "127.0.0.1");

            Assert.AreEqual("http://127.0.0.1/", httpRequest.GetBaseUrl().ToString());
        }
    }
}