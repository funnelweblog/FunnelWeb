using System;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration.Queries
{
    [TestFixture]
    public class GetEntriesQueryTests : IntegrationTest
    {
        public GetEntriesQueryTests()
            : base(TheDatabase.MustBeFresh)
        {
        }

        [Test]
        public void ReturnsEntry()
        {
            var name = "test-" + Guid.NewGuid();

            Database.WithRepository(
                repo =>
                {
                    var entry1 = new Entry { Name = name, Author = "A1" };
                    var revision1 = entry1.Revise();
                    revision1.Body = "Hello";
                    repo.Add(entry1);

                    var entry2 = new Entry { Name = name, Author = "A1" };
                    var revision2 = entry2.Revise();
                    revision2.Body = "Goodbye";
                    repo.Add(entry2);
                });

            Database.WithRepository(
                repo =>
                {
                    int total;
                    var entries = repo.Find(new GetEntriesQuery(), 0, 1, out total);
                    Assert.AreEqual(1, entries.Count());
                    Assert.AreEqual(2, total);
                });
        }
    }
}
