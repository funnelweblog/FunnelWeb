using System;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration.Queries
{
    [TestFixture]
    public class GetEntriesQueryByTagTests : IntegrationTest
    {
        public GetEntriesQueryByTagTests()
            : base(TheDatabase.MustBeFresh)
        {
        }

        [Test]
        public void ReturnsEntryWithWithRequestedTagOnly()
        {
            var name = "test-" + Guid.NewGuid();

            Database.WithRepository(
                repo =>
                {
                    var entry = new Entry { Name = name, Author = "A1" };
                    var revision2 = entry.Revise();
                    revision2.Body = "Goodbye";
                    var tag = new Tag{ Name = "Awesome"};
                    tag.Entries.Add(entry);
                    entry.Tags.Add(tag);
                    repo.Add(entry);
                    repo.Add(tag);

                    var entry1 = new Entry { Name = name, Author = "A1" };
                    var revision1 = entry1.Revise();
                    revision1.Body = "Hello";
                    var tag2 = new Tag { Name = "NotAwesome" };
                    tag2.Entries.Add(entry1);
                    entry1.Tags.Add(tag2);
                    repo.Add(entry1);
                    repo.Add(tag2);
                });

            Database.WithRepository(
                repo =>
                {
                    var result = repo.Find(new GetEntriesByTagQuery("Awesome"), 0, 2);
                    Assert.AreEqual(1, result.Count);
                    Assert.AreEqual(result.TotalResults, 1);
                    Assert.AreEqual(1, result[0].TagsCommaSeparated.Split(new[]{","}, StringSplitOptions.RemoveEmptyEntries).Length);
                });
        }
    }
}
