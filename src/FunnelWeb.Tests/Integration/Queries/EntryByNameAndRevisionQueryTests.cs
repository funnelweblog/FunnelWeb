using System;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Tests.Helpers;
using NHibernate;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration.Queries
{
    [TestFixture]
    public class EntryByNameAndRevisionQueryTests : IntegrationTest
    {
        public EntryByNameAndRevisionQueryTests()
            : base(TheDatabase.CanBeDirty)
        {
        }

        [Test]
        public void MatchesEntryByName()
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
                    var entry = repo.FindFirst(new EntryByNameAndRevisionQuery(name, 1));
                    Assert.AreEqual("A1", entry.Author);
                });
        }

        [Test]
        public void LoadsSpecifiedRevisionAsLatest()
        {
            var name = "test-" + Guid.NewGuid();

            Database.WithRepository(
                repo =>
                {
                    var entry = new Entry { Name = name, Author = "A1" };
                    var revision1 = entry.Revise();
                    revision1.Body = "Hello";
                    var revision2 = entry.Revise();
                    revision2.Body = "Goodbye";
                    repo.Add(entry);
                });

            Database.WithRepository(
                repo =>
                {
                    var entry = repo.FindFirst(new EntryByNameAndRevisionQuery(name, 1));
                    Assert.AreEqual("Hello", entry.Body);

                    entry = repo.FindFirst(new EntryByNameAndRevisionQuery(name, 2));
                    Assert.AreEqual("Goodbye", entry.Body);
                });
        }
    }
}
