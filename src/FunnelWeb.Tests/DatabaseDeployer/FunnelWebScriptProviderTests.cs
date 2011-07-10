using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using FunnelWeb.DatabaseDeployer;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.DatabaseDeployer
{
    [TestFixture]
    public class FunnelWebScriptProviderTests
    {
        [Test]
        public void WhenDatabaseProviderSpecificScriptIsPresentIgnoreGeneric()
        {
            var assembly = Substitute.For<_Assembly>();
            assembly.GetManifestResourceStream(Arg.Any<string>()).Returns(c=>new MemoryStream(Encoding.UTF8.GetBytes("132")));
            assembly.GetManifestResourceNames().Returns(new[]
                                                            {
                                                                "Script0001.sql",
                                                                "Script0001_sqlce.sql",
                                                                "Script0001_sql.sql", //to test when multiple providers specified
                                                                "Script0002.sql",
                                                                "Script0002_sql.sql", //to test when another provider is specified only
                                                                "Script0003.sql"
                                                            });
            var scriptProvider = new FunnelWebScriptProvider(assembly, s=>s.StartsWith("Script"), "sqlce");

            var scripts = scriptProvider.GetScripts().ToList();

            Assert.AreEqual("Script0001_sqlce.sql", scripts.Single(s => s.Name == "Script0001_sqlce.sql").Name);
            Assert.AreEqual("Script0002.sql", scripts.Single(s => s.Name == "Script0002.sql").Name);
            Assert.AreEqual("Script0003.sql", scripts.Single(s => s.Name == "Script0003.sql").Name);
            Assert.AreEqual(3, scripts.Count);
        }
    }
}