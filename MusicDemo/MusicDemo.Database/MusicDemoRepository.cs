using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
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
			Artist toUpdate;
			try { toUpdate = await dbContext.Artists.FirstOrDefaultAsync(a => a.ArtistID == artist.ArtistID); }
			catch { toUpdate = null; }
			if (toUpdate == null) return -1;

			// Update found artist
			dbContext.Entry(toUpdate).CurrentValues.SetValues(artist);
			int recordsChanged;
			try { recordsChanged = await dbContext.SaveChangesAsync(); }
			catch { recordsChanged = 0; }
			return recordsChanged;
		}
		#endregion

		#region Album
		public virtual Task<int> AlbumAddAsync(Album album)
		{
			// Add album to database
			dbContext.Albums.Add(album);
			return dbContext.SaveChangesAsync();
		}
		public virtual Task<int> AlbumDeleteByIDAsync(int artistID, int albumID)
		{
			// Delete album
			Album toDelete = new Album { ArtistID = artistID, AlbumID = albumID };
			dbContext.Albums.Attach(toDelete);
			dbContext.Albums.Remove(toDelete);
			return dbContext.SaveChangesAsync();
		}
		public virtual Task<Album> AlbumGetByIDAsync(int artistID, int albumID)
		{
			// Return desired album
			return dbContext.Albums.FirstOrDefaultAsync(a => a.ArtistID == artistID && a.AlbumID == albumID);
		}
		public virtual async Task<int> AlbumUpdateAsync(Album album)
		{
			// Find album to update
			Album toUpdate;
			try { toUpdate = await dbContext.Albums.FirstOrDefaultAsync(a => a.ArtistID == album.ArtistID && a.AlbumID == album.AlbumID); }
			catch { toUpdate = null; }
			if (toUpdate == null) return -1;

			// Update found album
			dbContext.Entry(toUpdate).CurrentValues.SetValues(album);
			int recordsChanged;
			try { recordsChanged = await dbContext.SaveChangesAsync(); }
			catch { recordsChanged = 0; }
			return recordsChanged;
		}
		#endregion

		#region Track
		public virtual Task<int> TrackAddAsync(Track track)
		{
			// Add track to database
			dbContext.Tracks.Add(track);
			return dbContext.SaveChangesAsync();
		}
		public virtual Task<int> TrackDeleteByIDAsync(int albumID, int trackID)
		{
			// Delete track
			Track toDelete = new Track { AlbumID = albumID, TrackID = trackID };
			dbContext.Tracks.Attach(toDelete);
			dbContext.Tracks.Remove(toDelete);
			return dbContext.SaveChangesAsync();
		}
		public virtual Task<Track> TrackGetByIDAsync(int albumID, int trackID)
		{
			// Return desired track
			return dbContext.Tracks.FirstOrDefaultAsync(t => t.AlbumID == albumID && t.TrackID == trackID);
		}
		public virtual async Task<int> TrackUpdateAsync(Track track)
		{
			// Find track to update
			Track toUpdate;
			try { toUpdate = await dbContext.Tracks.FirstOrDefaultAsync(t => t.AlbumID == track.AlbumID && t.TrackID == track.TrackID); }
			catch { toUpdate = null; }
			if (toUpdate == null) return -1;

			// Update found track
			dbContext.Entry(toUpdate).CurrentValues.SetValues(track);
			int recordChanges;
			try { recordChanges = await dbContext.SaveChangesAsync(); }
			catch { recordChanges = 0; }
			return recordChanges;
		}
		#endregion
		#endregion
	}
}
