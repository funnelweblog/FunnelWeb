using System;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories.Internal;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration
{
    [TestFixture]
    public class UpdateCommentCountTest : QueryIntegrationTest
    {
        public UpdateCommentCountTest()
            : base(TheDatabase.CanBeDirty)
        {
        }

        public override void TestQuery()
        {
            var name = "test-" + Guid.NewGuid();
            Entry entry = null;

            Database.WithRepository(
                repo =>
                    {
                        entry = new Entry {Name = name, Author = "A1", Status = EntryStatus.PublicBlog};
                        var revision = entry.Revise();
                        revision.Body = "Hello";
                        repo.Add(entry);
                        var comment = entry.Comment();
                        comment.AuthorName = "Test";
                        comment.AuthorEmail = "test@test.net";
                        comment.AuthorUrl = "";
                        comment.Body = "Comment";
                        comment.Posted = DateTime.Now;
                        comment.Entry = entry;
                        comment.IsSpam = false;
                        repo.Add(comment);

                        var comment2 = entry.Comment();
                        comment2.AuthorName = "Test";
                        comment2.AuthorEmail = "test@test.net";
                        comment2.AuthorUrl = "";
                        comment2.Body = "Comment";
                        comment2.Posted = DateTime.Now;
                        comment2.Entry = entry;
                        comment2.IsSpam = false;
                        repo.Add(comment2);

                        var comment3 = entry.Comment();
                        comment3.AuthorName = "Test";
                        comment3.AuthorEmail = "test@test.net";
                        comment3.AuthorUrl = "";
                        comment3.Body = "Comment";
                        comment3.Posted = DateTime.Now;
                        comment3.Entry = entry;
                        comment3.IsSpam = true;
                        repo.Add(comment3);
                    });

            Database.WithSession(
                s =>
                    {
                        var repo = new AdminRepository(s);

                        repo.UpdateCommentCountFor(entry.Id);
                    });

            Database.WithRepository(
                repo => Assert.AreEqual(2, repo.Get<Entry>(entry.Id).CommentCount));
        }
    }
}