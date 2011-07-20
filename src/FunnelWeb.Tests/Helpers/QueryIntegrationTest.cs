using System;
using NUnit.Framework;

namespace FunnelWeb.Tests.Helpers
{
    public abstract class QueryIntegrationTest
    {
        private readonly TheDatabase requirements;
        private static TemporaryDatabase SqlDatabase { get; set; }
        private static SqlCeTemporaryDatabase SqlCeDatabase { get; set; }

        protected QueryIntegrationTest(TheDatabase requirements)
        {
            this.requirements = requirements;
        }

        [Test]
        public void RunTestQuery()
        {
            Database = SqlDatabase;
            TestQuery();
            Database = SqlCeDatabase;
            TestQuery();
        }

        protected ITemporaryDatabase Database { get; private set; }

        public abstract void TestQuery();

        [SetUp]
        public void SetUp()
        {
            if ((requirements & TheDatabase.MustBeFresh) == TheDatabase.MustBeFresh || SqlDatabase == null)
            {
                SqlDatabase = new TemporaryDatabase();
                SqlDatabase.CreateAndDeploy();
            }

            if ((requirements & TheDatabase.MustBeFresh) == TheDatabase.MustBeFresh || SqlCeDatabase == null)
            {
                SqlCeDatabase = new SqlCeTemporaryDatabase();
                SqlCeDatabase.CreateAndDeploy();
            }
        }
    }
}