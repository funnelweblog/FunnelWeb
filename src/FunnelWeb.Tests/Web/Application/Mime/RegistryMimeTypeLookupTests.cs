using FunnelWeb.Web.Application.Mime;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Application.Mime
{
    public class RegistryMimeTypeLookupTests
    {
        [TestFixture]
        public class WhenMimeTypeRequested
        {
            [Test]
            public void ShouldReturnMimeTypeFromRegistry()
            {
                var lookup = new RegistryMimeTypeLookup();
                Assert.AreEqual("image/jpeg", lookup.GetMimeType("Hello.jpg"));
                Assert.AreEqual("image/png", lookup.GetMimeType("Hello.png"));
                Assert.AreEqual("text/plain", lookup.GetMimeType("C:\\Hello.txt"));
                Assert.AreEqual("image/jpeg", lookup.GetMimeType("../Green.jpg"));
            }
        }
    }
}
