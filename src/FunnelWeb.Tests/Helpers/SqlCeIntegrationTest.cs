using System;
using NUnit.Framework;

namespace FunnelWeb.Tests.Helpers
{
    public abstract class SqlCeIntegrationTest
    {
        private readonly TheDatabase requirements;
        protected static SqlCeTemporaryDatabase Database { get; private set; }

        protected SqlCeIntegrationTest(TheDatabase requirements)
        {
            this.requirements = requirements;
        }

        [SetUp]
        public void SetUp()
        {
            if ((requirements & TheDatabase.MustBeFresh) == TheDatabase.MustBeFresh || Database == null)
            {
                Database = new SqlCeTemporaryDatabase();
                Database.CreateAndDeploy();
            }
        }
    }
}