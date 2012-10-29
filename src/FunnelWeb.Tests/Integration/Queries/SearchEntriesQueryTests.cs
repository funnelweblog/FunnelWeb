using System;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration.Queries
{
    [TestFixture]
    public class SimpleSearchEntriesQueryReturnsEntry : QueryIntegrationTest
    {
        public SimpleSearchEntriesQueryReturnsEntry()
            : base(TheDatabase.CanBeDirty)
        {
        }

        public override void TestQuery()
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

            Database.WithRepository(
                repo =>
                {
                    var result = repo.Find(new SimpleSearchEntriesQuery(name.ToString()), 0, 1);
                    Assert.AreEqual(1, result.Count);
                    Assert.GreaterOrEqual(2, result.TotalResults);
                });
        }
    }
}
