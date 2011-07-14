using NUnit.Framework;

namespace FunnelWeb.Tests.Helpers
{
    public abstract class IntegrationTest
    {
        private readonly TheDatabase requirements;
        protected static TemporaryDatabase Database { get; private set; }
        
        protected IntegrationTest(TheDatabase requirements)
        {
            this.requirements = requirements;
        }

        [SetUp]
        public void SetUp()
        {
            if ((requirements & TheDatabase.MustBeFresh) == TheDatabase.MustBeFresh || Database == null)
            {
                Database = new TemporaryDatabase();
                Database.CreateAndDeploy();
            }
        }
    }

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
