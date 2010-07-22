using FunnelWeb.Web.Application.Markup;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Application.Markup
{
    public class MarkdownTests
    {
        protected void Expect(string markdown, string html)
        {
            var renderer = new MarkdownRenderer(true, "http://www.foo.com");
            var result = renderer.Render(markdown);
            Assert.AreEqual(html.Trim(), result.Trim());
        }

        [TestFixture]
        public class WhenRenderingMarkdown : MarkdownTests
        {
            [Test]
            public void ShouldSupportParagraphs()
            {
                Expect("Hello", "<p>Hello</p>");
                Expect("Hello\n\nWorld", "<p>Hello</p>\n\n<p>World</p>");
                Expect("Hello  \nWorld", "<p>Hello<br />\nWorld</p>");
            }

            [Test]
            public void ShouldSupportHeaders()
            {
                Expect("Hello\n==", "<h2>Hello</h2>");
                Expect("Hello\n--", "<h3>Hello</h3>");
                Expect("# Hello", "<h2>Hello</h2>");
                Expect("## Hello", "<h3>Hello</h3>");
                Expect("### Hello", "<h4>Hello</h4>");
                Expect("#### Hello", "<h5>Hello</h5>");
                Expect("# Hello #", "<h2>Hello</h2>");
                Expect("## Hello ##", "<h3>Hello</h3>");
                Expect("### Hello ###", "<h4>Hello</h4>");
                Expect("#### Hello ####", "<h5>Hello</h5>");
            }

            [Test]
            public void ShouldSupportQuotes()
            {
                Expect("> Hello", "<blockquote>\n  <p>Hello</p>\n</blockquote>");
                Expect("> Hello\n>> World\nNow", "<blockquote>\n  <p>Hello</p>\n  \n  <blockquote>\n    <p>World\n    Now</p>\n  </blockquote>\n</blockquote>");
            }

            [Test]
            public void ShouldSupportLists()
            {
                Expect("+ ABC\n+ BCD", "<ul>\n<li>ABC</li>\n<li>BCD</li>\n</ul>");
            }

            [Test]
            public void ShouldFullyQualifyUrls()
            {
                Expect("[ABC][1]\n\n  [1]: /foo", "<p><a href=\"http://www.foo.com/foo\">ABC</a></p>");
            }

            [Test]
            public void ShouldSupportFormatting()
            {
                Expect("**Hello**", "<p><strong>Hello</strong></p>");
                Expect("*Hello*", "<p><em>Hello</em></p>");
                Expect("_Hello_", "<p><em>Hello</em></p>");
            }

            [Test]
            public void ShouldSupportInlineHtml()
            {
                Expect("<strong>hello</strong>", "<p><strong>hello</strong></p>");
            }

            [Test]
            public void ShouldProtectAgainstCrossSiteScripting()
            {
                Expect("<script language='javascript'>alert('wow');</script>", "alert('wow');");
            }
        }
    }
}
