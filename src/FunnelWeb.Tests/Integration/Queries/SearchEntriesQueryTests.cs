using System;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration.Queries
{
    [TestFixture]
    public class SearchEntriesQueryTests : IntegrationTest
    {
        public SearchEntriesQueryTests()
            : base(TheDatabase.CanBeDirty)
        {
        }

        [Test]
        public void ReturnsEntry()
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

            //todo: TableFullTextChangeTrackingOn doesn't check if full text is enabled for that table, need to find IsFullTextIndexEnabled property
            var executeScalar = Database.AdHoc.ExecuteScalar(
                "SELECT FullTextServiceProperty('IsFullTextInstalled') + OBJECTPROPERTY(OBJECT_ID('$schema$.Entry'), 'TableFullTextChangeTrackingOn')");
            var isFullTextEnabled = (int)executeScalar;

            //Idealy the test will run when full text is installed and enabled, if not, still test like based search
            if (isFullTextEnabled == 2)
            {
                //Database.AdHoc.ExecuteNonQuery("ALTER FULLTEXT INDEX ON $schema$.[Entry] START UPDATE POPULATION");

                //Database.WithRepository(
                //repo =>
                //{
                //    var result = repo.Find(new SearchEntriesQuery(name.ToString()), 0, 1);
                //    Assert.AreEqual(1, result.Count);
                //    Assert.GreaterOrEqual(2, result.TotalResults);
                //});

                Database.AdHoc.ExecuteNonQuery("ALTER FULLTEXT INDEX ON $schema$.[Entry] SET CHANGE_TRACKING = OFF");
                Database.AdHoc.ExecuteNonQuery("EXEC sys.sp_fulltext_table @tabname=N'$schema$.[Entry]', @action=N'deactivate'");
            }

            Database.WithRepository(
                repo =>
                {
                    var result = repo.Find(new SearchEntriesQuery(name.ToString()), 0, 1);
                    Assert.AreEqual(1, result.Count);
                    Assert.GreaterOrEqual(2, result.TotalResults);
                });

            if (isFullTextEnabled == 2)
            {
                //Database.AdHoc.ExecuteNonQuery("EXEC sys.sp_fulltext_table @tabname=N'$schema$.[Entry]', @action=N'activate'");
            }
        }
    }
}
