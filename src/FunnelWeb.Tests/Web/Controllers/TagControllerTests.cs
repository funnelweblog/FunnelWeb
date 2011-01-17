using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Web.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Controllers
{
    [TestFixture]
    public class TagControllerTests
    {

        [Test]
        public void TagControllerTests_Empty_Repository_Returns_No_Categories()
        {
            //Arrange
            var repo = Substitute.For<ITagRepository>();
            var controller = new TagController(repo);

            //Act
            var result = controller.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOf(typeof (IEnumerable<Tag>), result.Model);
            Assert.IsFalse(((IEnumerable<Tag>) result.Model).Any());
        }
    }
}
