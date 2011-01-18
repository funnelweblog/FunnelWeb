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
        public void TagControllerTests_Empty_Repository_Returns_No_Tags()
        {
            //Arrange
            var repo = Substitute.For<ITagRepository>();
            var controller = new TagController(repo);

            //Act
            var result = controller.Index() as JsonResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf(typeof (IEnumerable<Tag>), result.Data);
            Assert.IsFalse(((IEnumerable<Tag>) result.Data).Any());
        }

        [Test]
        public void TagControllerTests_GetAll_Returns_Entire_Repository()
        {
            //Arrange
            var repo = Substitute.For<ITagRepository>();
            repo.GetTags(Arg.Any<string>()).Returns(Enumerable
                                       .Range(0, 5)
                                       .Select(x => new Tag
                                                        {
                                                            Name = x.ToString()
                                                        })
                                       .AsQueryable());
            var controller = new TagController(repo);

            //Act
            var result = controller.Index() as JsonResult;

            //Assert
            Assert.IsTrue(((IQueryable<Tag>)result.Data).Count() == 5);
        }

        [Test]
        public void TagControllerTests_Tag_Accessible_By_Full_Name()
        {
            //Arrange
            var repo = Substitute.For<ITagRepository>();
            var tagName = "Demo";
            repo.GetTag(Arg.Is(tagName)).Returns(new Tag {Name = tagName});

            var controller = new TagController(repo);
            //Act
            var result = controller.Tag(tagName) as JsonResult;
            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(((dynamic) result.Data).Name == tagName);
        }

        [Test]
        public void TagControllerTests_Tag_Accessible_By_Partial_Name()
        {
            //Arrange

            //Act

            //Assert
            Assert.Inconclusive();
        }

        [Test]
        public void TagControllerTests_Null_Result_When_Tag_Name_Not_Matched()
        {
            //Arrange
            var repo = Substitute.For<ITagRepository>();
            repo.GetTag(Arg.Any<string>()).Returns(x => null);

            var controller = new TagController(repo);
            //Act
            var result = controller.Tag(string.Empty) as JsonResult;
            //Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
        }

        [Test]
        public void TagControllerTests_Creating_Tag_Returns_As_Result()
        {
            //Arrange

            //Act

            //Assert
            Assert.Inconclusive();
        }

        [Test]
        public void TagControllerTests_All_Pages_For_A_Tag_Can_Be_Resolved()
        {
            //Arrange

            //Act

            //Assert
            Assert.Inconclusive();
        }
    }
}
