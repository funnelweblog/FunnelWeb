using System.Threading;
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

            Browser.TextField("Page").AppendText("my-page");
            Browser.TextField("Title").AppendText("My Page");
            Browser.TextField("MetaTitle").AppendText("My Page Top");
            Browser.TextField("MetaDescription").AppendText("This is a summary of my blog post");
            Browser.TextField("Sidebar").AppendText("Some intro");
            Browser.TextField("TagsString").AppendText("tag1, tag2, tag3");
            Browser.TextField("wmd-input").AppendText("This is my entry...");
            Browser.RadioButton(Find.ByValue("Public-Blog")).Click();
            
            Browser.Button(Find.ByValue("Save!")).Click();

            Browser.WaitUntilContainsText("History");
            Browser.WaitUntilContainsText("My Page");

            Browser.GoTo(RootUrl);

            Assert.IsTrue(Browser.ContainsText("My Page"));
            Assert.IsTrue(Browser.ContainsText("This is a summary of my blog post"));
            Assert.IsTrue(Browser.ContainsText("tag1"));
            Assert.IsTrue(Browser.ContainsText("tag2"));
            Assert.IsTrue(Browser.ContainsText("tag3"));
            
            Browser.WaitForComplete();
            Thread.Sleep(10000);
        }
    }
}