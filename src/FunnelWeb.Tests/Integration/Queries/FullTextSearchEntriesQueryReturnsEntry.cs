using System;
using System.Threading;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration.Queries
{
    [TestFixture]
    public class FullTextSearchEntriesQueryReturnsEntry : IntegrationTest
    {
        public FullTextSearchEntriesQueryReturnsEntry()
            : base(TheDatabase.CanBeDirty)
        { }

        [Test]
        public void TestQuery()
        {
            var name = Guid.NewGuid();

            Database.WithRepository(
                repo =>
                    {
                        var entry1 = new Entry { Name = "bar-" + name, Title = name.ToString(), Author = "A1" };
                        var revision1 = entry1.Revise();
                        revision1.Body = "Hello";
                        repo.Add(entry1);

                        var entry2 = new Entry { Name = "foo-" + name, Title = name.ToString(), Author = "A1" };
                        var revision2 = entry2.Revise();
                        revision2.Body = "Goodbye";
                        repo.Add(entry2);
                    });

            var executeScalar = Database.AdHoc.ExecuteScalar(
                "SELECT FullTextServiceProperty('IsFullTextInstalled') + OBJECTPROPERTY(OBJECT_ID('$schema$.Entry'), 'TableFullTextChangeTrackingOn')");
            var isFullTextEnabled = (int)executeScalar;

            if (isFullTextEnabled == 2)
            {
                Database.AdHoc.ExecuteNonQuery("ALTER FULLTEXT INDEX ON $schema$.[Entry] SET CHANGE_TRACKING MANUAL");
                Database.AdHoc.ExecuteNonQuery("ALTER FULLTEXT INDEX ON $schema$.[Entry] START UPDATE POPULATION");

                var count = 0;
                while (true)
                {
                    var status = (int)Database.AdHoc.ExecuteScalar("SELECT OBJECTPROPERTY(OBJECT_ID('$schema$.Entry'), 'TableFulltextPopulateStatus')");
                    if (status == 0 || count++ > 10)
                        break;
                    Thread.Sleep(200);
                }

                Database.WithRepository(
                repo =>
                {
                    var result = repo.Find(new FullTextSearchEntriesQuery(name.ToString()), 0, 1);
                    Assert.AreEqual(1, result.Count);
                    Assert.GreaterOrEqual(2, result.TotalResults);
                });
            }
        }
    }
}