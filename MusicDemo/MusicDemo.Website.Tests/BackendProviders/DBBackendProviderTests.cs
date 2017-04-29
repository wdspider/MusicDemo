using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicDemo.Database;
using MusicDemo.Website.Backend.Database;
using MusicDemo.Website.Models;
using DBModels = MusicDemo.Database.Models;

namespace MusicDemo.Website.Tests.BackendProviders
{
	[TestClass]
	[TestCategory("Backend Tests - DBBackendProvider")]
	public class DBBackendProviderTests
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
				cfg.AddProfile<DBModelMappingProfile>();
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

		#region Artist Tests
		[TestMethod]
		public async Task ArtistAddNew_SavesArtistToRepo()
		{
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.ArtistAddAsync(It.IsAny<DBModels.Artist>())).ReturnsAsync(1);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			await backend.ArtistAddAsync(new Artist { Name = "MxPx" });

			mockRepo.Verify(m => m.ArtistAddAsync(It.Is<DBModels.Artist>(a => a.Name == "MxPx")), Times.Once());
		}
		[TestMethod]
		public async Task ArtistGetAll_ReturnsList()
		{
			List<DBModels.Artist> dbArtists = new List<DBModels.Artist>
			{
				new DBModels.Artist{ Name = "Kaskade" },
				new DBModels.Artist{ Name = "Foxxx" }
			};
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.ArtistGetAllAsync()).ReturnsAsync(dbArtists.OrderBy(a => a.Name).ToList());

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			List<Artist> artists = await backend.ArtistGetAllAsync();

			Assert.AreEqual(2, artists.Count);
			Assert.AreEqual("Foxxx", artists[0].Name);
			Assert.AreEqual("Kaskade", artists[1].Name);
		}
		#endregion
	}
}
