using FunnelWeb.Model.Strings;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Model.Strings
{
    public class PageNameTests
    {
        [TestFixture]
        public class WhenNamingPages : PageNameTests
        {
            [Test]
            public void PageNamesShouldBeCorrected()
            {
                var expected = (PageName)"hello-world";
                
                var matches = new[]
                {
                    "hello-world",
                    "hello-worLD",
                    " hello -world ",
                    " hello$-world ",
                    " hello $-world ",
                    " hello-$-world ",
                    " hello world ",
                    " -   hello   world   - ",
                    " -   HeLLo  WoRLD  - ",
                    " -   HeLLo  %^@#@#*()[]WoRLD  - ",
                    " -   HeLLo  %^@#@#*()[]WoRLD  - $%",
                    "@# -   HeLLo  %^@#@#*()[]WoRLD  - $%"
                };
                
                var nonMatches = new[]
                {
                    "helloworld",
                    "bob",
                    "",
                    null
                };

                foreach (var match in matches)
                {
                    var castMatch = (PageName) match;
                    Assert.AreEqual(expected, castMatch);
                    Assert.IsTrue(expected.GetHashCode() == castMatch.GetHashCode());
                    Assert.IsTrue(expected == castMatch);
                    Assert.IsTrue(expected == match);
                    Assert.IsTrue(expected.Equals(match));
                    Assert.IsTrue(expected.Equals(castMatch));
                    Assert.IsTrue(expected.Equals((object)castMatch));
                    Assert.IsTrue(expected.Equals(match));
                    Assert.IsTrue(Equals(expected, castMatch));
                }

                foreach (var nonMatch in nonMatches)
                {
                    Assert.AreNotEqual(expected, (PageName)nonMatch);
                    Assert.IsTrue(expected != nonMatch);
                }
            }

            [Test]
            public void NullsShouldBeConvertedToEmptyWherePossible()
            {
                var name = (string)null;
                Assert.IsNull(name);
                var pageName = (PageName)(string)null;
                Assert.IsNotNull(pageName);
            }
        }
    }
}
