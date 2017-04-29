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
		public async Task ArtistAdd_SavesArtistToContext()
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
		public async Task ArtistGetAll_SortsByName()
		{
			IQueryable<Artist> data = new List<Artist>
			{
				new Artist{Name = "Kaskade"},
				new Artist{Name = "Foxxx"}
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

			Mock<MusicDemoDbContext> mockContext = new Mock<MusicDemoDbContext>();
			mockContext.Setup(m => m.Artists).Returns(mockArtists.Object);

			MusicDemoRepository repo = new MusicDemoRepository(mockContext.Object);
			List<Artist> artists = await repo.ArtistGetAllAsync();

			Assert.AreEqual(2, artists.Count);
			Assert.AreEqual("Foxxx", artists[0].Name);
			Assert.AreEqual("Kaskade", artists[1].Name);
		}
		#endregion
	}
}
