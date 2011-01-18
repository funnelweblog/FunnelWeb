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
            LogIn();

            // The Admin link should appear now we're logged in
            Assert.IsTrue(Browser.ContainsText("Admin"));
            
            LogOut();

            Assert.IsFalse(Browser.ContainsText("Admin"));
        }
    }
}