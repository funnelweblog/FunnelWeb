using System;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration.Queries
{
    [TestFixture]
    public class SearchTagsByNameQueryReturnsTag : QueryIntegrationTest
    {
        public SearchTagsByNameQueryReturnsTag()
            : base(TheDatabase.CanBeDirty)
        {
        }

        public override void TestQuery()
        {
            var name = Guid.NewGuid().ToString();

            Database.WithRepository(
                repo =>
                {
                    var tag = new Tag { Name = name };
                    repo.Add(tag);
                });

            Database.WithRepository(
                repo =>
                {
                    var result = repo.FindFirstOrDefault(new SearchTagsByNameQuery(name));
                    Assert.NotNull(result);
                    Assert.AreEqual(name, result.Name);
                });
        }
    }
}
