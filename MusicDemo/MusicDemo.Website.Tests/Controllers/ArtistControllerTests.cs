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
		public async Task Create_SavesNewArtistToBackend()
		{
			ArtistViewModel newArtist = new ArtistViewModel { Name = "New Artist" };
			List<Artist> artistSource = new List<Artist>();
			Mock<BackendProvider> mockBackend = new Mock<BackendProvider>(autoMapper);
			mockBackend.Setup(m => m.ArtistGetAllAsync()).ReturnsAsync(artistSource.OrderBy(a => a.Name).ToList());
			mockBackend.Setup(m => m.ArtistAddAsync(It.IsAny<Artist>())).Returns(Task.FromResult(true)).Callback<Artist>(a => artistSource.Add(a));

			ArtistController controller = new ArtistController(mockBackend.Object, autoMapper);
			ViewResult result = (await controller.Create(newArtist)) as ViewResult;
			List<ArtistViewModel> viewModel = result.Model as List<ArtistViewModel>;

			Assert.IsNotNull(result);
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(1, viewModel.Count);
			Assert.AreEqual("New Artist", viewModel[0].Name);
		}
		#endregion
	}
}
