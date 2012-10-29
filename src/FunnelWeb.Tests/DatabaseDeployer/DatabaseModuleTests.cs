using Autofac;
using FunnelWeb.DatabaseDeployer;
using NUnit.Framework;

namespace FunnelWeb.Tests.DatabaseDeployer
{
    [TestFixture]
    public class DatabaseModuleTests
    {
        private IContainer container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new DatabaseModule());
            container = builder.Build();
        }

        [Test]
        public void RegistersCorrectTypes()
        {
            Assert.IsTrue(container.IsRegistered<IDatabaseUpgradeDetector>());
            Assert.IsTrue(container.IsRegistered<IApplicationDatabase>());
        }
    }
}
