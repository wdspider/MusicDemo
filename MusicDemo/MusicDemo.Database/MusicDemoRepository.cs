using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MusicDemo.Database.Models;

namespace MusicDemo.Database
{
	public class MusicDemoRepository
	{
		#region Internal State
		private readonly MusicDemoDbContext dbContext;
		#endregion

		#region Constructors
		public MusicDemoRepository(MusicDemoDbContext context)
		{
			// Initialize internal state
			dbContext = context;
		}
		#endregion

		#region Class Methods
		#region Artist
		public virtual Task<int> ArtistAddAsync(Artist artist)
		{
			// Add artist to database
			dbContext.Artists.Add(artist);
			return dbContext.SaveChangesAsync();
		}
		public virtual Task<int> ArtistDeleteByIDAsync(int artistID)
		{
			// Delete artist
			Artist toDelete = new Artist { ArtistID = artistID };
			dbContext.Artists.Attach(toDelete);
			dbContext.Artists.Remove(toDelete);
			return dbContext.SaveChangesAsync();
		}
		public virtual Task<List<Artist>> ArtistGetAllAsync()
		{
			// Sort artists by name before returning
			return dbContext.Artists.OrderBy(a => a.Name).ToListAsync();
		}
		public virtual Task<Artist> ArtistGetByIDAsync(int artistID)
		{
			// Return desired artist
			return dbContext.Artists.FirstOrDefaultAsync(a => a.ArtistID == artistID);
		}
		public virtual async Task<int> ArtistUpdateAsync(Artist artist)
		{
			// Find artist to update
			Artist toUpdate = await dbContext.Artists.FirstOrDefaultAsync(a => a.ArtistID == artist.ArtistID);
			if (toUpdate == null) return 0;

			// Update found artist
			dbContext.Entry(toUpdate).CurrentValues.SetValues(artist);
			return await dbContext.SaveChangesAsync();
		}
		#endregion

		#region Album
		public virtual Task<int> AlbumAddAsync(Album album)
		{
			// Add album to database
			dbContext.Albums.Add(album);
			return dbContext.SaveChangesAsync();
		}
		public virtual Task<int> AlbumDeleteByIDAsync(int albumID)
		{
			// Delete album
			Album toDelete = new Album { AlbumID = albumID };
			dbContext.Albums.Attach(toDelete);
			dbContext.Albums.Remove(toDelete);
			return dbContext.SaveChangesAsync();
		}
		public virtual Task<Album> AlbumGetByIDAsync(int albumID)
		{
			// Return desired album
			return dbContext.Albums.FirstOrDefaultAsync(a => a.AlbumID == albumID);
		}
		public virtual async Task<int> AlbumUpdateAsync(Album album)
		{
			// Find album to update
			Album toUpdate = await dbContext.Albums.FirstOrDefaultAsync(a => a.AlbumID == album.AlbumID);
			if (toUpdate == null) return 0;

			// Update found album
			dbContext.Entry(toUpdate).CurrentValues.SetValues(album);
			return await dbContext.SaveChangesAsync();
		}
		#endregion

		#region Track
		public virtual Task<int> TrackAddAsync(Track track)
		{
			// Add track to database
			dbContext.Tracks.Add(track);
			return dbContext.SaveChangesAsync();
		}
		public virtual Task<int> TrackDeleteByIDAsync(int trackID)
		{
			// Delete track
			Track toDelete = new Track { TrackID = trackID };
			dbContext.Tracks.Attach(toDelete);
			dbContext.Tracks.Remove(toDelete);
			return dbContext.SaveChangesAsync();
		}
		public virtual Task<Track> TrackGetByIDAsync(int trackID)
		{
			// Return desired track
			return dbContext.Tracks.FirstOrDefaultAsync(t => t.TrackID == trackID);
		}
		public virtual async Task<int> TrackUpdateAsync(Track track)
		{
			// Find track to update
			Track toUpdate = await dbContext.Tracks.FirstOrDefaultAsync(t => t.TrackID == track.TrackID);
			if (toUpdate == null) return 0;

			// Update found track
			dbContext.Entry(toUpdate).CurrentValues.SetValues(track);
			return await dbContext.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
