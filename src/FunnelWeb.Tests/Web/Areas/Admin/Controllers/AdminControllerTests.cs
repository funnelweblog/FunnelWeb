using System.Linq;
using System.Web.Mvc;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Tests.Web.Controllers;
using FunnelWeb.Web.Areas.Admin.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Areas.Admin.Controllers
{
	[TestFixture]
	public class AdminControllerTests : ControllerTests
	{
		protected AdminController Controller { get; set; }
		protected IAdminRepository AdminRepository { get; set; }

		[SetUp]
		public void SetUp()
		{
			TestAuthenticationAndAuthorization.SetTestUserToCurrentPrincipal();
			CustomResolver.Initiate();

			Controller = new AdminController
											 {
												 AdminRepository = AdminRepository = Substitute.For<IAdminRepository>(),
												 ControllerContext = ControllerContext,
												 Repository = Repository
											 };
		}

		[Test]
		public void Index()
		{
			var result = (ViewResult)Controller.Index();

			Assert.That(result.ViewName, Is.EqualTo(string.Empty));
		}

		[Test]
		public void DeleteComment()
		{
			var entry = Substitute.For<Entry>();
			entry.Id.Returns(3);
			Repository.Get<Comment>(0).Returns(new Comment { Entry = entry });
			var result = (RedirectToRouteResult)Controller.DeleteComment(0);

			Assert.That(result.RouteValues["Action"], Is.EqualTo("Comments"));
		}

		[Test]
		public void DeleteAllSpam()
		{
			var result = (RedirectToRouteResult)Controller.DeleteAllSpam();

			Assert.That(result.RouteValues["Action"], Is.EqualTo("Comments"));
		}

		[Test]
		public void DeletePingback()
		{
			var result = (RedirectToRouteResult)Controller.DeletePingback(0);

			Assert.That(result.RouteValues["Action"], Is.EqualTo("Pingbacks"));
		}

		[Test]
		public void ToggleSpam()
		{
			var result = (RedirectToRouteResult)Controller.ToggleSpam(0);

			Assert.That(result.RouteValues["Action"], Is.EqualTo("Comments"));
		}

		[Test]
		public void ToggePingbackSpam()
		{
			var result = (RedirectToRouteResult)Controller.TogglePingbackSpam(0);

			Assert.That(result.RouteValues["Action"], Is.EqualTo("Pingbacks"));
		}

		[Test]
		public void DeleteCommentUpdatesCommentCount()
		{
			// arrange
			var entry = Substitute.For<Entry>();
			entry.Id.Returns(3);
			Repository.Get<Comment>(0).Returns(new Comment { Entry = entry });

			// act
			Controller.DeleteComment(0);

			// assert
			AdminRepository.Received().UpdateCommentCountFor(3);
		}

		[Test]
		public void ToggleSpamUpdatesCommentCount()
		{
			// arrange
			var entry = Substitute.For<Entry>();
			entry.Id.Returns(3);
			Repository.Get<Comment>(0).Returns(new Comment { Entry = entry });

			// act
			Controller.ToggleSpam(0);

			// assert
			AdminRepository.Received().UpdateCommentCountFor(3);
		}

		[Test]
		public void DeleteAllSpamUpdatesCommentCount()
		{
			// arrange
			var entry = Substitute.For<Entry>();
			entry.Id.Returns(3);
			var entry2 = Substitute.For<Entry>();
			entry2.Id.Returns(4);
			var comments = new[]
															 {
																	 new Comment {Entry = entry},
																	 new Comment {Entry = entry},
																	 new Comment {Entry = entry2}
															 };
			Repository.Find(Arg.Any<GetSpamQuery>()).Returns(comments);

			// act
			Controller.DeleteAllSpam();

			// assert
			Assert.AreEqual(3, Repository.ReceivedCalls().Count(c => c.GetMethodInfo().Name == "Remove"));
			AdminRepository.Received().UpdateCommentCountFor(3);
			AdminRepository.Received().UpdateCommentCountFor(4);
		}
	}
}