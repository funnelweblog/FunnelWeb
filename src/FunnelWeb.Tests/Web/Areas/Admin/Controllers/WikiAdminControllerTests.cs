using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Repositories;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Tests.Web.Controllers;
using FunnelWeb.Web.Areas.Admin.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Areas.Admin.Controllers
{
	[TestFixture]
	public class WikiAdminControllerTests : ControllerTests
	{
		protected WikiAdminController AdminController { get; set; }

		[SetUp]
		public void Setup()
		{
			AdminController = new WikiAdminController
														{
															Repository = Repository = Substitute.For<IRepository>(),
															ControllerContext = ControllerContext = ControllerContext
														};

			Identity = Substitute.For<IIdentity>();
			User = Substitute.For<IPrincipal>();
			User.Identity.Returns(Identity);
			ControllerContext.HttpContext.User.Returns(User);
		}

		[Test]
		public void EditReturnsExistingPageWhenFound()
		{
			var entry = new EntryRevision { Name = "awesome-post" };
			Repository.FindFirstOrDefault(Arg.Any<EntryByNameQuery>()).Returns(entry);

			var feeds = new List<Tag>().AsQueryable();
			Repository.FindAll<Tag>().Returns(feeds);

			var result = (ViewResult)AdminController.Edit(entry.Name, null);

			Assert.AreEqual("Edit", result.ViewName);
			Assert.AreEqual(feeds, ((EntryRevision)result.ViewData.Model).AllTags);
			Assert.AreEqual(entry.Name.ToString(), ((EntryRevision)result.ViewData.Model).Name.ToString());

			Repository.Received().FindFirstOrDefault(Arg.Any<EntryByNameQuery>());
			Repository.Received().FindAll<Tag>();
		}

		[Test]
		public void EditReturnsNewModelWhenEntryNotFound()
		{
			// arrange
			const string newPostName = "new-awesome-post";

			// act
			var result = (ViewResult)AdminController.Edit(newPostName, null);

			// assert
			Assert.IsNotNull(result.ViewData.Model);
			Assert.IsInstanceOf(typeof(EntryRevision), result.ViewData.Model);
			Assert.NotNull(((EntryRevision)result.ViewData.Model).Tags);
			Assert.AreEqual("new-awesome-post", ((EntryRevision)result.ViewData.Model).Name.ToString());
		}

	}
}
