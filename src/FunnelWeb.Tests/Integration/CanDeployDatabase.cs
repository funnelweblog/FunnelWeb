using System.Linq;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration
{
    [TestFixture]
    public class CanDeployDatabase : IntegrationTest
    {
        public CanDeployDatabase() : base(TheDatabase.MustBeFresh)
        {
            
        }

        protected override void Execute()
        {
            var tables = Database.AdHoc.ExecuteReader("select * from sys.Tables order by [name] desc");

            Assert.IsNotNull(tables.FirstOrDefault(table => table["name"] == "Entry"));
            Assert.IsNotNull(tables.FirstOrDefault(table => table["name"] == "Revision"));
            Assert.IsNotNull(tables.FirstOrDefault(table => table["name"] == "Comment"));
            Assert.IsNotNull(tables.FirstOrDefault(table => table["name"] == "SchemaVersions"));
            Assert.IsNotNull(tables.FirstOrDefault(table => table["name"] == "Tag"));
        }
    }
}