using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicDemo.Website.Backend.BackendProviders;
using MusicDemo.Website.Backend.Models;
using MusicDemo.Website.Controllers;
using MusicDemo.Website.ViewModels;

namespace MusicDemo.Website.Tests.Controllers
{
	[TestClass]
	[TestCategory("Controller Tests - TrackController")]
	public class TrackControllerTests
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
		public async Task Create_FoundAlbum_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.AlbumGetByIDAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync(new Album { ArtistID = 1, AlbumID = 1 });

			TrackController controller = new TrackController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Create(1, 1)) as ViewResult;
			TrackViewModel viewModel = result?.Model as TrackViewModel;

			mockBackend.Verify(m => m.AlbumGetByIDAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
			Assert.AreEqual(1, viewModel.AlbumID);
		}
		[TestMethod]
		public async Task Create_NotFoundAlbum_RedirectsToAlbumDetails()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.AlbumGetByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Album)null);

			TrackController controller = new TrackController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Create(1, 1)) as RedirectToRouteResult;

			mockBackend.Verify(m => m.AlbumGetByIDAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"]);
			Assert.AreEqual("Album", result.RouteValues["controller"]);
			Assert.AreEqual(1, result.RouteValues["artistID"]);
			Assert.AreEqual(1, result.RouteValues["albumID"]);
		}
		[TestMethod]
		public async Task Create_SuccessfulSave_RedirectsToAlbumDetails()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.TrackAddAsync(It.IsAny<Track>())).ReturnsAsync(true);

			TrackController controller = new TrackController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Create(new TrackViewModel { ArtistID = 1, AlbumID = 1, TrackID = 1 })) as RedirectToRouteResult;

			mockBackend.Verify(m => m.TrackAddAsync(It.Is<Track>(a => a.AlbumID == 1 && a.TrackID == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"]);
			Assert.AreEqual("Album", result.RouteValues["controller"]);
			Assert.AreEqual(1, result.RouteValues["artistID"]);
			Assert.AreEqual(1, result.RouteValues["albumID"]);
		}
		[TestMethod]
		public async Task Create_UnsuccessfulSave_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.TrackAddAsync(It.IsAny<Track>())).ReturnsAsync(false);

			TrackController controller = new TrackController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Create(new TrackViewModel { ArtistID = 1, AlbumID = 1, TrackID = 1 })) as ViewResult;
			TrackViewModel viewModel = result?.Model as TrackViewModel;

			mockBackend.Verify(m => m.TrackAddAsync(It.Is<Track>(a => a.AlbumID == 1 && a.TrackID == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
			Assert.AreEqual(1, viewModel.AlbumID);
			Assert.AreEqual(1, viewModel.TrackID);
		}
		#endregion

		#region Edit Tests
		[TestMethod]
		public async Task Edit_FoundItem_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.TrackGetByIDAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync(new Track { AlbumID = 1, TrackID = 1 });

			TrackController controller = new TrackController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Edit(1, 1, 1)) as ViewResult;
			TrackViewModel viewModel = result?.Model as TrackViewModel;

			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
			Assert.AreEqual(1, viewModel.AlbumID);
			Assert.AreEqual(1, viewModel.TrackID);
		}
		[TestMethod]
		public async Task Edit_NotFoundItem_RedirectsToAlbumDetails()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.TrackGetByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Track)null);

			TrackController controller = new TrackController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Edit(1, 1, 1)) as RedirectToRouteResult;

			mockBackend.Verify(m => m.TrackGetByIDAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"]);
			Assert.AreEqual("Album", result.RouteValues["controller"]);
			Assert.AreEqual(1, result.RouteValues["artistID"]);
			Assert.AreEqual(1, result.RouteValues["albumID"]);
		}
		[TestMethod]
		public async Task Edit_SuccessfulSave_RedirectsToAlbumDetails()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.TrackUpdateAsync(It.IsAny<Track>())).ReturnsAsync(true);

			TrackController controller = new TrackController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Edit(new TrackViewModel { ArtistID = 1, AlbumID = 1, TrackID = 1 })) as RedirectToRouteResult;

			mockBackend.Verify(m => m.TrackUpdateAsync(It.Is<Track>(a => a.AlbumID == 1 && a.TrackID == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"]);
			Assert.AreEqual("Album", result.RouteValues["controller"]);
			Assert.AreEqual(1, result.RouteValues["artistID"]);
			Assert.AreEqual(1, result.RouteValues["albumID"]);
		}
		[TestMethod]
		public async Task Edit_UnsuccessfulSave_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.TrackUpdateAsync(It.IsAny<Track>())).ReturnsAsync(false);

			TrackController controller = new TrackController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Edit(new TrackViewModel { ArtistID = 1, AlbumID = 1, TrackID = 1 })) as ViewResult;
			TrackViewModel viewModel = result?.Model as TrackViewModel;

			mockBackend.Verify(m => m.TrackUpdateAsync(It.Is<Track>(a => a.AlbumID == 1 && a.TrackID == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
			Assert.AreEqual(1, viewModel.AlbumID);
			Assert.AreEqual(1, viewModel.TrackID);
		}
		#endregion

		#region Delete Tests
		[TestMethod]
		public async Task Delete_RedirectsToAlbumDetails()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.TrackDeleteByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

			TrackController controller = new TrackController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Delete(1, 1, 1)) as RedirectToRouteResult;

			mockBackend.Verify(m => m.TrackDeleteByIDAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"]);
			Assert.AreEqual("Album", result.RouteValues["controller"]);
			Assert.AreEqual(1, result.RouteValues["artistID"]);
			Assert.AreEqual(1, result.RouteValues["albumID"]);
		}
		#endregion
	}
}
