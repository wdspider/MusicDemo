using System.Collections.Generic;
using System.Linq;
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
	[TestCategory("Controller Tests - ArtistController")]
	public class ArtistControllerTests
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

		#region Index Tests
		[TestMethod]
		public async Task Index_ReturnsView()
		{
			List<Artist> artistSource = new List<Artist>
			{
				new Artist{ Name = "Kaskade" },
				new Artist{ Name = "Foxxx" }
			};
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistGetAllAsync()).ReturnsAsync(artistSource.OrderBy(a => a.Name).ToList());

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Index()) as ViewResult;
			List<ArtistViewModel> viewModel = result.Model as List<ArtistViewModel>;

			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(2, viewModel.Count);
			Assert.AreEqual("Foxxx", viewModel[0].Name);
			Assert.AreEqual("Kaskade", viewModel[1].Name);
		}
		#endregion

		#region Create Tests
		[TestMethod]
		public void Create_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			ViewResult result = (controller.Create()) as ViewResult;

			Assert.IsNotNull(result);
		}
		[TestMethod]
		public async Task Create_SuccessfulSave_RedirectsToIndex()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistAddAsync(It.IsAny<Artist>())).ReturnsAsync(true);

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Create(new ArtistViewModel { Name = "MxPx" })) as RedirectToRouteResult;

			mockBackend.Verify(m => m.ArtistAddAsync(It.Is<Artist>(a => a.Name == "MxPx")), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.RouteValues["action"]);
		}
		[TestMethod]
		public async Task Create_UnsuccessfulSave_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistAddAsync(It.IsAny<Artist>())).ReturnsAsync(false);

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Create(new ArtistViewModel { ArtistID = 1 })) as ViewResult;
			ArtistViewModel viewModel = result?.Model as ArtistViewModel;

			mockBackend.Verify(m => m.ArtistAddAsync(It.Is<Artist>(a => a.ArtistID == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
		}
		#endregion

		#region Edit Tests
		[TestMethod]
		public async Task Edit_FoundItem_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistGetByIDAsync(It.IsAny<int>())).ReturnsAsync(new Artist { ArtistID = 1 });

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Edit(1)) as ViewResult;
			ArtistViewModel viewModel = result?.Model as ArtistViewModel;
			
			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
		}
		[TestMethod]
		public async Task Edit_NotFoundItem_RedirectsToIndex()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistGetByIDAsync(It.IsAny<int>())).ReturnsAsync((Artist)null);

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Edit(1)) as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.RouteValues["action"]);
		}
		[TestMethod]
		public async Task Edit_SuccessfulSave_RedirectsToIndex()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistUpdateAsync(It.IsAny<Artist>())).ReturnsAsync(true);

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Edit(new ArtistViewModel { ArtistID = 1 })) as RedirectToRouteResult;

			mockBackend.Verify(m => m.ArtistUpdateAsync(It.Is<Artist>(a => a.ArtistID == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.RouteValues["action"]);
		}
		[TestMethod]
		public async Task Edit_UnsuccessfulSave_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistUpdateAsync(It.IsAny<Artist>())).ReturnsAsync(false);

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Edit(new ArtistViewModel { ArtistID = 1 })) as ViewResult;
			ArtistViewModel viewModel = result?.Model as ArtistViewModel;

			mockBackend.Verify(m => m.ArtistUpdateAsync(It.Is<Artist>(a => a.ArtistID == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
		}
		#endregion

		#region Details Tests
		[TestMethod]
		public async Task Details_FoundItem_ReturnsView()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistGetByIDAsync(It.IsAny<int>())).ReturnsAsync(new Artist { ArtistID = 1 });

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Details(1)) as ViewResult;
			ArtistDetailsViewModel viewModel = result?.Model as ArtistDetailsViewModel;

			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.ArtistID);
		}
		[TestMethod]
		public async Task Details_NotFoundItem_RedirectsToIndex()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistGetByIDAsync(It.IsAny<int>())).ReturnsAsync((Artist)null);

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Details(1)) as RedirectToRouteResult;

			mockBackend.Verify(m => m.ArtistGetByIDAsync(It.Is<int>(a => a == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.RouteValues["action"]);
		}
		#endregion

		#region Delete Tests
		[TestMethod]
		public async Task Delete_RedirectsToIndex()
		{
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistDeleteByIDAsync(It.IsAny<int>())).ReturnsAsync(true);

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			RedirectToRouteResult result = (await controller.Delete(1)) as RedirectToRouteResult;

			mockBackend.Verify(m => m.ArtistDeleteByIDAsync(It.Is<int>(a => a == 1)), Times.Once());
			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.RouteValues["action"]);
		}
		#endregion
	}
}
