using System;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration.Queries
{
    [TestFixture]
    public class GetFullEntriesQueryTests : IntegrationTest
    {
        public GetFullEntriesQueryTests()
            : base(TheDatabase.MustBeFresh)
        {
        }

        [Test]
        public void ReturnsEntriesWithComments()
        {
            var name = "test-" + Guid.NewGuid();

            Database.WithRepository(
                repo =>
                {
                    var entry1 = new Entry { Name = name, Author = "A1" };
                    var revision1 = entry1.Revise();
                    revision1.Body = "Hello";
                    repo.Add(entry1);
                    var comment1 = entry1.Comment();
                    comment1.AuthorName = "A2";
                    comment1.AuthorEmail = "a@b.com";
                    comment1.AuthorUrl = "somesite.com";
                    comment1.Body = "some comment";
                    repo.Add(comment1);
                });

            Database.WithRepository(
                repo =>
                {
                    var result = repo.Find(new GetFullEntriesQuery(true), 0, 1);
                    Assert.AreEqual(1, result.Count);
                    Assert.AreEqual(1, result.Single().Comments.Count);
                });
        }
    }
}
