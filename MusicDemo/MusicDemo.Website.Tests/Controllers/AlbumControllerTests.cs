using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicDemo.Website.Backend;
using MusicDemo.Website.Controllers;
using MusicDemo.Website.Models;
using MusicDemo.Website.ViewModels;

namespace MusicDemo.Website.Tests.Controllers
{
	[TestClass]
	[TestCategory("Controller Tests - AlbumController")]
	public class AlbumControllerTests
	{
		#region Internal State
		private static IMapper autoMapper;
		#endregion

		[ClassInitialize]
		public static void ClassInit(TestContext context)
		{
			// Initialize automapper
			MapperConfiguration mapConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<ViewModelMappingProfile>();
			});
			mapConfig.AssertConfigurationIsValid();
			autoMapper = mapConfig.CreateMapper();
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			// Reset automapper
			autoMapper = null;
		}

		#region Create Tests
		[TestMethod]
		public async Task Create_FoundArtist_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistGetByIDAsync(It.IsAny<int>())).ReturnsAsync(new Artist { ArtistID = 1 });

			AlbumController controller = new AlbumController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Create(1)) as ViewResult;
			AlbumViewModel viewModel = result?.Model as AlbumViewModel;

			mockBackend.Verify(m => m.ArtistGetByIDAsync(It.Is<int>(a => a == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
		}
		[TestMethod]
		public async Task Create_NotFoundArtist_RedirectsToArtistIndex()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistGetByIDAsync(It.IsAny<int>())).ReturnsAsync((Artist)null);

			AlbumController controller = new AlbumController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Create(1)) as RedirectToRouteResult;

			mockBackend.Verify(m => m.ArtistGetByIDAsync(It.Is<int>(a => a == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.RouteValues["action"]);
			Assert.AreEqual("Artist", result.RouteValues["controller"]);
		}
		[TestMethod]
		public async Task Create_SuccessfulSave_RedirectsToArtistDetails()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.AlbumAddAsync(It.IsAny<Album>())).ReturnsAsync(true);

			AlbumController controller = new AlbumController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Create(new AlbumViewModel { ArtistID = 1, AlbumID = 1 })) as RedirectToRouteResult;

			mockBackend.Verify(m => m.AlbumAddAsync(It.Is<Album>(a => a.ArtistID == 1 && a.AlbumID == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"]);
			Assert.AreEqual("Artist", result.RouteValues["controller"]);
			Assert.AreEqual(1, result.RouteValues["artistID"]);
		}
		[TestMethod]
		public async Task Create_UnsuccessfulSave_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.AlbumAddAsync(It.IsAny<Album>())).ReturnsAsync(false);

			AlbumController controller = new AlbumController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Create(new AlbumViewModel { ArtistID = 1, AlbumID = 1 })) as ViewResult;
			AlbumViewModel viewModel = result?.Model as AlbumViewModel;

			mockBackend.Verify(m => m.AlbumAddAsync(It.Is<Album>(a => a.ArtistID == 1 && a.AlbumID == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
			Assert.AreEqual(1, viewModel.AlbumID);
		}
		#endregion

		#region Edit Tests
		[TestMethod]
		public async Task Edit_FoundItem_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.AlbumGetByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new Album { ArtistID = 1, AlbumID = 1 });

			AlbumController controller = new AlbumController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Edit(1, 1)) as ViewResult;
			AlbumViewModel viewModel = result?.Model as AlbumViewModel;

			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
			Assert.AreEqual(1, viewModel.AlbumID);
		}
		[TestMethod]
		public async Task Edit_NotFoundItem_RedirectsToArtistDetails()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.AlbumGetByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Album)null);

			AlbumController controller = new AlbumController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Edit(1, 1)) as RedirectToRouteResult;

			mockBackend.Verify(m => m.AlbumGetByIDAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"]);
			Assert.AreEqual("Artist", result.RouteValues["controller"]);
			Assert.AreEqual(1, result.RouteValues["artistID"]);
		}
		[TestMethod]
		public async Task Edit_SuccessfulSave_RedirectsToArtistDetails()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.AlbumUpdateAsync(It.IsAny<Album>())).ReturnsAsync(true);

			AlbumController controller = new AlbumController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Edit(new AlbumViewModel { ArtistID = 1, AlbumID = 1 })) as RedirectToRouteResult;

			mockBackend.Verify(m => m.AlbumUpdateAsync(It.Is<Album>(a => a.ArtistID == 1 && a.AlbumID == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"]);
			Assert.AreEqual("Artist", result.RouteValues["controller"]);
			Assert.AreEqual(1, result.RouteValues["artistID"]);
		}
		[TestMethod]
		public async Task Edit_UnsuccessfulSave_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.AlbumUpdateAsync(It.IsAny<Album>())).ReturnsAsync(false);

			AlbumController controller = new AlbumController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Edit(new AlbumViewModel { ArtistID = 1, AlbumID = 1 })) as ViewResult;
			AlbumViewModel viewModel = result?.Model as AlbumViewModel;

			mockBackend.Verify(m => m.AlbumUpdateAsync(It.Is<Album>(a => a.ArtistID == 1 && a.AlbumID == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
			Assert.AreEqual(1, viewModel.AlbumID);
		}
		#endregion

		#region Details Tests
		[TestMethod]
		public async Task Details_FoundItem_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.AlbumGetByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new Album { ArtistID = 1, AlbumID = 1 });

			AlbumController controller = new AlbumController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Details(1, 1)) as ViewResult;
			AlbumDetailsViewModel viewModel = result?.Model as AlbumDetailsViewModel;

			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
			Assert.AreEqual(1, viewModel.AlbumID);
		}
		[TestMethod]
		public async Task Details_NotFoundItem_RedirectsToArtistDetails()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.AlbumGetByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Album)null);

			AlbumController controller = new AlbumController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Details(1, 1)) as RedirectToRouteResult;

			mockBackend.Verify(m => m.AlbumGetByIDAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"]);
			Assert.AreEqual("Artist", result.RouteValues["controller"]);
			Assert.AreEqual(1, result.RouteValues["artistID"]);
		}
		#endregion

		#region Delete Tests
		[TestMethod]
		public async Task Delete_RedirectsToArtistDetails()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.AlbumDeleteByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

			AlbumController controller = new AlbumController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Delete(1, 1)) as RedirectToRouteResult;

			mockBackend.Verify(m => m.AlbumDeleteByIDAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"]);
			Assert.AreEqual("Artist", result.RouteValues["controller"]);
			Assert.AreEqual(1, result.RouteValues["artistID"]);
		}
		#endregion
	}
}
