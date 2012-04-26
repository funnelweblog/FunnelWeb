using System.Linq;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration
{
    [TestFixture]
    public class DatabaseDeploymentTests : SqlCeIntegrationTest
    {
        public DatabaseDeploymentTests() : base(TheDatabase.MustBeFresh)
        {
        }

        [Test]
        public void TablesAreCreated()
        {
            var tables = Database.AdHoc.ExecuteReader("select * from INFORMATION_SCHEMA.Tables order by [TABLE_NAME] desc");

            Assert.IsNotNull(tables.FirstOrDefault(table => table["TABLE_NAME"] == "Entry"));
            Assert.IsNotNull(tables.FirstOrDefault(table => table["TABLE_NAME"] == "Revision"));
            Assert.IsNotNull(tables.FirstOrDefault(table => table["TABLE_NAME"] == "Comment"));
            Assert.IsNotNull(tables.FirstOrDefault(table => table["TABLE_NAME"] == "SchemaVersions"));
            Assert.IsNotNull(tables.FirstOrDefault(table => table["TABLE_NAME"] == "Tag"));
        }
    }
}