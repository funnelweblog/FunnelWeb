using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Repositories;
using FunnelWeb.Repositories.Queries;
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
            var repo = Substitute.For<IRepository>();
            repo.Find(Arg.Any<SearchTagsByNameQuery>()).Returns(x => Enumerable.Empty<Tag>());
            var controller = new TagController(repo);

            //Act
            var result = controller.Index() as JsonResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsFalse(((IEnumerable<object>) result.Data).Any());
        }

        [Test]
        public void TagControllerTests_GetAll_Returns_Entire_Repository()
        {
            //Arrange
            var repo = Substitute.For<IRepository>();
            repo.Find(Arg.Any<SearchTagsByNameQuery>())
                .Returns(Enumerable
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
            Assert.IsTrue(((IEnumerable<object>)result.Data).Count() == 5);
        }

        [Test]
        public void TagControllerTests_Tag_Accessible_By_Full_Name()
        {
            //Arrange
            var repo = Substitute.For<IRepository>();
            const string tagName = "Demo";
            repo.FindFirstOrDefault(Arg.Is<SearchTagsByNameQuery>(q=>q.TagName == tagName))
                .Returns(new Tag {Name = tagName});

            var controller = new TagController(repo);
            //Act
            var result = controller.Tag(tagName) as JsonResult;
            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(((dynamic) result.Data).Name == tagName);
        }

        [Test]
        public void TagControllerTests_Null_Result_When_Tag_Name_Not_Matched()
        {
            // arrange
            var repo = Substitute.For<IRepository>();
            repo.FindFirstOrDefault(Arg.Any<SearchTagsByNameQuery>()).Returns(x => null);

            var controller = new TagController(repo);

            // act
            var result = controller.Tag(string.Empty) as JsonResult;
            
            // assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
        }
    }
}
