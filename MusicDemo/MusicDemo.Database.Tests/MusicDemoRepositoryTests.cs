using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicDemo.Database.Models;

namespace MusicDemo.Database.Tests
{
	[TestClass]
	[TestCategory("Database Tests - MusicDemoRepository")]
	public class MusicDemoRepositoryTests
	{
		#region Artist Tests
		[TestMethod]
		public async Task ArtistAddAsync_SavesItem()
		{
			Mock<DbSet<Artist>> mockArtists = new Mock<DbSet<Artist>>();
			Mock<MusicDemoDbContext> mockContext = new Mock<MusicDemoDbContext>();
			mockContext.Setup(m => m.Artists).Returns(mockArtists.Object);

			MusicDemoRepository repo = new MusicDemoRepository(mockContext.Object);
			await repo.ArtistAddAsync(new Artist { Name = "MxPx" });

			mockArtists.Verify(m => m.Add(It.Is<Artist>(a => a.Name == "MxPx")), Times.Once());
			mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
		}
		[TestMethod]
		public async Task ArtistDeleteByIDAsync_DeletesItem()
		{
			Mock<DbSet<Artist>> mockArtists = GetMockArtists();
			Mock<MusicDemoDbContext> mockContext = new Mock<MusicDemoDbContext>();
			mockContext.Setup(m => m.Artists).Returns(mockArtists.Object);

			MusicDemoRepository repo = new MusicDemoRepository(mockContext.Object);
			await repo.ArtistDeleteByIDAsync(1);

			mockArtists.Verify(m => m.Remove(It.Is<Artist>(a => a.ArtistID == 1)), Times.Once());
			mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
		}
		[TestMethod]
		public async Task ArtistGetAllAsync_SortsByName()
		{
			Mock<DbSet<Artist>> mockArtists = GetMockArtists();
			Mock<MusicDemoDbContext> mockContext = new Mock<MusicDemoDbContext>();
			mockContext.Setup(m => m.Artists).Returns(mockArtists.Object);

			MusicDemoRepository repo = new MusicDemoRepository(mockContext.Object);
			List<Artist> artists = await repo.ArtistGetAllAsync();

			Assert.AreEqual(2, artists.Count);
			Assert.AreEqual("Foxxx", artists[0].Name);
			Assert.AreEqual("Kaskade", artists[1].Name);
		}
		[TestMethod]
		public async Task ArtistGetByIDAsync_ReturnsItem()
		{
			Mock<DbSet<Artist>> mockArtists = GetMockArtists();
			Mock<MusicDemoDbContext> mockContext = new Mock<MusicDemoDbContext>();
			mockContext.Setup(m => m.Artists).Returns(mockArtists.Object);

			MusicDemoRepository repo = new MusicDemoRepository(mockContext.Object);
			Artist artist = await repo.ArtistGetByIDAsync(1);

			Assert.IsNotNull(artist);
			Assert.AreEqual(1, artist.ArtistID);
		}
		#endregion

		#region Album Tests
		[TestMethod]
		public async Task AlbumAddAsync_SavesItem()
		{
			Mock<DbSet<Album>> mockAlbums = new Mock<DbSet<Album>>();
			Mock<MusicDemoDbContext> mockContext = new Mock<MusicDemoDbContext>();
			mockContext.Setup(m => m.Albums).Returns(mockAlbums.Object);

			MusicDemoRepository repo = new MusicDemoRepository(mockContext.Object);
			await repo.AlbumAddAsync(new Album { Name = "MxPx" });

			mockAlbums.Verify(m => m.Add(It.Is<Album>(a => a.Name == "MxPx")), Times.Once());
			mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
		}
		[TestMethod]
		public async Task AlbumDeleteByIDAsync_DeletesItem()
		{
			Mock<DbSet<Album>> mockAlbums = GetMockAlbums();
			Mock<MusicDemoDbContext> mockContext = new Mock<MusicDemoDbContext>();
			mockContext.Setup(m => m.Albums).Returns(mockAlbums.Object);

			MusicDemoRepository repo = new MusicDemoRepository(mockContext.Object);
			await repo.AlbumDeleteByIDAsync(1, 1);

			mockAlbums.Verify(m => m.Remove(It.Is<Album>(a => a.AlbumID == 1)), Times.Once());
			mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
		}
		[TestMethod]
		public async Task AlbumGetByIDAsync_ReturnsItem()
		{
			Mock<DbSet<Album>> mockAlbums = GetMockAlbums();
			Mock<MusicDemoDbContext> mockContext = new Mock<MusicDemoDbContext>();
			mockContext.Setup(m => m.Albums).Returns(mockAlbums.Object);

			MusicDemoRepository repo = new MusicDemoRepository(mockContext.Object);
			Album album = await repo.AlbumGetByIDAsync(1, 1);

			Assert.IsNotNull(album);
			Assert.AreEqual(1, album.ArtistID);
			Assert.AreEqual(1, album.AlbumID);
		}
		#endregion

		#region Track Tests
		[TestMethod]
		public async Task TrackAddAsync_SavesItem()
		{
			Mock<DbSet<Track>> mockTracks = new Mock<DbSet<Track>>();
			Mock<MusicDemoDbContext> mockContext = new Mock<MusicDemoDbContext>();
			mockContext.Setup(m => m.Tracks).Returns(mockTracks.Object);

			MusicDemoRepository repo = new MusicDemoRepository(mockContext.Object);
			await repo.TrackAddAsync(new Track { Name = "MxPx" });

			mockTracks.Verify(m => m.Add(It.Is<Track>(a => a.Name == "MxPx")), Times.Once());
			mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
		}
		[TestMethod]
		public async Task TrackDeleteByIDAsync_DeletesItem()
		{
			Mock<DbSet<Track>> mockTracks = GetMockTracks();
			Mock<MusicDemoDbContext> mockContext = new Mock<MusicDemoDbContext>();
			mockContext.Setup(m => m.Tracks).Returns(mockTracks.Object);

			MusicDemoRepository repo = new MusicDemoRepository(mockContext.Object);
			await repo.TrackDeleteByIDAsync(1, 1);

			mockTracks.Verify(m => m.Remove(It.Is<Track>(a => a.TrackID == 1)), Times.Once());
			mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
		}
		[TestMethod]
		public async Task TrackGetByIDAsync_ReturnsItem()
		{
			Mock<DbSet<Track>> mockTracks = GetMockTracks();
			Mock<MusicDemoDbContext> mockContext = new Mock<MusicDemoDbContext>();
			mockContext.Setup(m => m.Tracks).Returns(mockTracks.Object);

			MusicDemoRepository repo = new MusicDemoRepository(mockContext.Object);
			Track track = await repo.TrackGetByIDAsync(1, 1);

			Assert.IsNotNull(track);
			Assert.AreEqual(1, track.AlbumID);
			Assert.AreEqual(1, track.TrackID);
		}

		#endregion

		#region Helpers
		private Mock<DbSet<Artist>> GetMockArtists()
		{
			IQueryable<Artist> data = new List<Artist>
			{
				new Artist { ArtistID = 1, Name = "Kaskade" },
				new Artist { ArtistID = 2, Name = "Foxxx" }
			}.AsQueryable();

			Mock<DbSet<Artist>> mockArtists = new Mock<DbSet<Artist>>();
			mockArtists.As<IDbAsyncEnumerable<Artist>>()
				.Setup(m => m.GetAsyncEnumerator())
				.Returns(new TestDBAsyncEnumerator<Artist>(data.GetEnumerator()));
			mockArtists.As<IQueryable<Artist>>()
				.Setup(m => m.Provider)
				.Returns(new TestDBAsyncQueryProvider<Artist>(data.Provider));
			mockArtists.As<IQueryable<Artist>>().Setup(m => m.Expression).Returns(data.Expression);
			mockArtists.As<IQueryable<Artist>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockArtists.As<IQueryable<Artist>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

			return mockArtists;
		}
		private Mock<DbSet<Album>> GetMockAlbums()
		{
			IQueryable<Album> data = new List<Album>
			{
				new Album { ArtistID = 1, AlbumID = 1, Name = "Kaskade" },
				new Album { ArtistID = 1, AlbumID = 2, Name = "Foxxx" }
			}.AsQueryable();

			Mock<DbSet<Album>> mockAlbums = new Mock<DbSet<Album>>();
			mockAlbums.As<IDbAsyncEnumerable<Album>>()
				.Setup(m => m.GetAsyncEnumerator())
				.Returns(new TestDBAsyncEnumerator<Album>(data.GetEnumerator()));
			mockAlbums.As<IQueryable<Album>>()
				.Setup(m => m.Provider)
				.Returns(new TestDBAsyncQueryProvider<Album>(data.Provider));
			mockAlbums.As<IQueryable<Album>>().Setup(m => m.Expression).Returns(data.Expression);
			mockAlbums.As<IQueryable<Album>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockAlbums.As<IQueryable<Album>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

			return mockAlbums;
		}
		private Mock<DbSet<Track>> GetMockTracks()
		{
			IQueryable<Track> data = new List<Track>
			{
				new Track { AlbumID = 1, TrackID = 1, Name = "Kaskade" },
				new Track { AlbumID = 1, TrackID = 2, Name = "Foxxx" }
			}.AsQueryable();

			Mock<DbSet<Track>> mockTracks = new Mock<DbSet<Track>>();
			mockTracks.As<IDbAsyncEnumerable<Track>>()
				.Setup(m => m.GetAsyncEnumerator())
				.Returns(new TestDBAsyncEnumerator<Track>(data.GetEnumerator()));
			mockTracks.As<IQueryable<Track>>()
				.Setup(m => m.Provider)
				.Returns(new TestDBAsyncQueryProvider<Track>(data.Provider));
			mockTracks.As<IQueryable<Track>>().Setup(m => m.Expression).Returns(data.Expression);
			mockTracks.As<IQueryable<Track>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockTracks.As<IQueryable<Track>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

			return mockTracks;
		}
		#endregion
	}
}
