using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicDemo.Database;
using MusicDemo.Website.Backend.BackendProviders.Database;
using MusicDemo.Website.Backend.Models;
using DBModels = MusicDemo.Database.Models;

namespace MusicDemo.Website.Backend.Tests.BackendProviders
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
		public async Task ArtistAddNewAsync_SavesArtistToRepo()
		{
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.ArtistAddAsync(It.IsAny<DBModels.Artist>())).ReturnsAsync(1);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			await backend.ArtistAddAsync(new Artist { Name = "MxPx" });

			mockRepo.Verify(m => m.ArtistAddAsync(It.Is<DBModels.Artist>(a => a.Name == "MxPx")), Times.Once());
		}
		[TestMethod]
		public async Task ArtistDeleteByIDAsync_DeletesFromRepo()
		{
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.ArtistDeleteByIDAsync(It.IsAny<int>())).ReturnsAsync(1);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			await backend.ArtistDeleteByIDAsync(1);

			mockRepo.Verify(m => m.ArtistDeleteByIDAsync(It.Is<int>(a => a == 1)), Times.Once());
		}
		[TestMethod]
		public async Task ArtistGetAllAsync_ReturnsList()
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
		[TestMethod]
		public async Task ArtistGetByIDAsync_ReturnsItem()
		{
			DBModels.Artist dbArtist = new DBModels.Artist { ArtistID = 1 };
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.ArtistGetByIDAsync(It.IsAny<int>())).ReturnsAsync(dbArtist);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			Artist artist = await backend.ArtistGetByIDAsync(1);

			Assert.IsNotNull(artist);
			Assert.AreEqual(1, artist.ArtistID);
		}
		[TestMethod]
		public async Task ArtistUpdateAsync_UpdatesItem()
		{
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.ArtistUpdateAsync(It.IsAny<DBModels.Artist>())).ReturnsAsync(1);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			await backend.ArtistUpdateAsync(new Artist { ArtistID = 1 });

			mockRepo.Verify(m => m.ArtistUpdateAsync(It.Is<DBModels.Artist>(a => a.ArtistID == 1)), Times.Once());
		}
		#endregion

		#region Album Tests
		[TestMethod]
		public async Task AlbumAddNewAsync_SavesToRepo()
		{
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.AlbumAddAsync(It.IsAny<DBModels.Album>())).ReturnsAsync(1);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			await backend.AlbumAddAsync(new Album { Name = "MxPx" });

			mockRepo.Verify(m => m.AlbumAddAsync(It.Is<DBModels.Album>(a => a.Name == "MxPx")), Times.Once());
		}
		[TestMethod]
		public async Task AlbumDeleteByIDAsync_DeletesFromRepo()
		{
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.AlbumDeleteByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(1);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			await backend.AlbumDeleteByIDAsync(1, 1);

			mockRepo.Verify(m => m.AlbumDeleteByIDAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 1)), Times.Once());
		}
		[TestMethod]
		public async Task AlbumGetByIDAsync_ReturnsItem()
		{
			DBModels.Album dbAlbum = new DBModels.Album { ArtistID = 1, AlbumID = 1 };
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.AlbumGetByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(dbAlbum);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			Album album = await backend.AlbumGetByIDAsync(1, 1);

			Assert.IsNotNull(album);
			Assert.AreEqual(1, album.ArtistID);
			Assert.AreEqual(1, album.AlbumID);
		}
		[TestMethod]
		public async Task AlbumUpdateAsync_UpdatesItem()
		{
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.AlbumUpdateAsync(It.IsAny<DBModels.Album>())).ReturnsAsync(1);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			await backend.AlbumUpdateAsync(new Album { AlbumID = 1 });

			mockRepo.Verify(m => m.AlbumUpdateAsync(It.Is<DBModels.Album>(a => a.AlbumID == 1)), Times.Once());
		}
		#endregion

		#region Track Tests
		[TestMethod]
		public async Task TrackAddNewAsync_SavesToRepo()
		{
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.TrackAddAsync(It.IsAny<DBModels.Track>())).ReturnsAsync(1);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			await backend.TrackAddAsync(new Track { Name = "MxPx" });

			mockRepo.Verify(m => m.TrackAddAsync(It.Is<DBModels.Track>(t => t.Name == "MxPx")), Times.Once());
		}
		[TestMethod]
		public async Task TrackDeleteByIDAsync_DeletesFromRepo()
		{
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.TrackDeleteByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(1);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			await backend.TrackDeleteByIDAsync(1, 1);

			mockRepo.Verify(m => m.TrackDeleteByIDAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 1)), Times.Once());
		}
		[TestMethod]
		public async Task TrackGetByIDAsync_ReturnsItem()
		{
			DBModels.Track dbTrack = new DBModels.Track { AlbumID = 1, TrackID = 1 };
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.TrackGetByIDAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(dbTrack);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			Track track = await backend.TrackGetByIDAsync(1, 1);

			Assert.IsNotNull(track);
			Assert.AreEqual(1, track.AlbumID);
			Assert.AreEqual(1, track.TrackID);
		}
		[TestMethod]
		public async Task TrackUpdateAsync_UpdatesItem()
		{
			Mock<MusicDemoRepository> mockRepo = new Mock<MusicDemoRepository>(null);
			mockRepo.Setup(m => m.TrackUpdateAsync(It.IsAny<DBModels.Track>())).ReturnsAsync(1);

			DBBackendProvider backend = new DBBackendProvider(autoMapper, mockRepo.Object);
			await backend.TrackUpdateAsync(new Track { TrackID = 1 });

			mockRepo.Verify(m => m.TrackUpdateAsync(It.Is<DBModels.Track>(t => t.TrackID == 1)), Times.Once());
		}
		#endregion
	}
}
