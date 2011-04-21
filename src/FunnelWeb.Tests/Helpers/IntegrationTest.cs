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
}
