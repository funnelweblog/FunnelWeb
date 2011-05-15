using System;
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
                    var entry1 = new Entry { Name = name, Author = "A1", Status = EntryStatus.PublicBlog };
                    var revision1 = entry1.Revise();
                    revision1.Body = "Hello";
                    repo.Add(entry1);

                    var entry2 = new Entry { Name = name, Author = "A1", Status = EntryStatus.PublicBlog };
                    var revision2 = entry2.Revise();
                    revision2.Body = "Goodbye";
                    repo.Add(entry2);
                });

            Database.WithRepository(
                repo =>
                {
                    var result = repo.Find(new GetEntriesQuery(EntryStatus.PublicBlog), 0, 1);
                    Assert.AreEqual(1, result.Count);
                    Assert.GreaterOrEqual(result.TotalResults, 2);
                });
        }

        [Test]
        public void ReturnsEntryWithTags()
        {
            var name = "test-" + Guid.NewGuid();

            Database.WithRepository(
                repo =>
                {
                    var entry = new Entry { Name = name, Author = "A1", Status = EntryStatus.PublicBlog };
                    var revision2 = entry.Revise();
                    revision2.Body = "Goodbye";
                    var tag = new Tag{ Name = "Awesome"};
                    tag.Entries.Add(entry);
                    entry.Tags.Add(tag);
                    repo.Add(entry);
                    repo.Add(tag);

                    var entry1 = new Entry { Name = name, Author = "A1", Status = EntryStatus.PublicBlog };
                    var revision1 = entry1.Revise();
                    revision1.Body = "Hello";
                    repo.Add(entry1);
                });

            Database.WithRepository(
                repo =>
                {
                    var result = repo.Find(new GetEntriesQuery(EntryStatus.PublicBlog), 0, 2);
                    Assert.AreEqual(2, result.Count);
                    Assert.AreEqual(result.TotalResults, 2);
                    Assert.AreEqual(1, result[0].TagsCommaSeparated.Split(new[]{","}, StringSplitOptions.RemoveEmptyEntries).Length);
                });
        }
    }
}
