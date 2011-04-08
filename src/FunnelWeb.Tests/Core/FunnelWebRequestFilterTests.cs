using System;
using System.Data;
using System.Web.Mvc;
using FunnelWeb.Filters;
using NHibernate;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Core
{
    [TestFixture]
    public class FunnelWebRequestFilterTests
    {
        private FunnelWebRequestAttribute filter;
        private ISession session;

        [SetUp]
        public void SetUp()
        {
            session = Substitute.For<ISession>();
            filter = new FunnelWebRequestAttribute(() => session);
        }

        [Test]
        public void RequestIsWrappedInSerializableTransaction()
        {
            filter.OnActionExecuting(null);
            session.Received().BeginTransaction(IsolationLevel.Serializable);
        }

        [Test]
        public void TransactionIsCommittedOnSuccessfulRequest()
        {
            filter.OnResultExecuted(new ResultExecutedContext());
            
            session.Received().Flush();
            session.Transaction.Received().Commit();
        }

        [Test]
        public void TransactionIsRolledBackOnFailedRequest()
        {
            filter.OnResultExecuted(new ResultExecutedContext { Exception = new DivideByZeroException()});

            session.Received().Clear();
            session.Transaction.Received().Rollback();
        }
    }
}
