using FunnelWeb.Tests.Helpers;
using NUnit.Framework;
using WatiN.Core;

namespace FunnelWeb.Tests.Integration
{
    [TestFixture]
    public class BlogPostsWillAppearOnHomePage : IntegrationTest
    {
        public BlogPostsWillAppearOnHomePage()
            : base(TheDatabase.MustBeFresh)
        {
        }

        protected override void Execute()
        {
            LogIn();

            Browser.Link(Find.ByText("New Post")).Click();

            Browser.TextField("Page").TypeText("my-page");
            Browser.TextField("Title").TypeText("My Page");
            Browser.TextField("MetaTitle").TypeText("My Page Top");
            Browser.TextField("MetaDescription").TypeText("This is a summary of my blog post");
            Browser.TextField("Sidebar").TypeText("Some intro");
            Browser.TextField("TagsString").TypeText("tag1, tag2, tag3");
            Browser.TextField("wmd-input").TypeText("This is my entry...");
            Browser.RadioButton(Find.ByValue("Public-Blog")).Click();
            
            Browser.Button(Find.ByValue("Save!")).Click();

            Browser.WaitUntilContainsText("History");
            
            Browser.GoTo(RootUrl);

            Assert.IsTrue(Browser.ContainsText("My Page"));
            Assert.IsTrue(Browser.ContainsText("This is a summary of my blog post"));
            Assert.IsTrue(Browser.ContainsText("tag1"));
            Assert.IsTrue(Browser.ContainsText("tag2"));
            Assert.IsTrue(Browser.ContainsText("tag3"));
            
            Browser.WaitForComplete();
        }
    }
}