
using NUnit.Framework;

namespace FunnelWeb.Tests.Helpers
{
    [TestFixture]
    public abstract class Specification<TSubject>
    {
        protected TSubject Subject { get; private set; }

        public abstract TSubject Given();

        public abstract void When();

        [SetUp]
        public void Initialize()
        {
            Subject = Given();
            When();
        }
    }
}
