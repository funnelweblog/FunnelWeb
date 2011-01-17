using System;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;
using WatiN.Core;

namespace FunnelWeb.Tests.Integration
{
    [TestFixture]
    public class CanLogIn : IntegrationTest
    {
        public CanLogIn() : base(TheDatabase.CanBeDirty)
        {
        }

        protected override void Execute()
        {
            using (var browser = new IE())
            {
                browser.GoTo(RootUrl + "login");
                
                Assert.IsTrue(browser.ContainsText("log in"));

                browser.TextField(Find.ByName("Username")).AppendText("test");
                browser.TextField(Find.ByName("Password")).AppendText("test");

                browser.Button(Find.ByValue("Submit")).Click();

                // The Admin link should appear now we're logged in
                browser.WaitUntilContainsText("Admin");

                if (browser.ContainsText("Logout"))
                {
                    browser.Link(Find.ByText("Logout")).Click();
                }
            }
        }
    }

    public class LoggedInIntegrationTest : IntegrationTest
    {
        public LoggedInIntegrationTest(TheDatabase requirements) : base(requirements)
        {
        }

        protected override void Execute()
        {
            LogIn();
            LogOut();
        }
    }
}