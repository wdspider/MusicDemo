using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MusicDemo.Database;
using MusicDemo.Website.Backend.Models;
using DBModels = MusicDemo.Database.Models;

namespace MusicDemo.Website.Backend.BackendProviders.Database
{
	public class DBBackendProvider : BackendProvider
	{
		#region Internal State
		private readonly MusicDemoRepository repository;
		#endregion

		#region Constructors
		public DBBackendProvider(IMapper autoMapper, MusicDemoRepository repo) : base(autoMapper)
		{
			// Initialize internal state
			repository = repo;
		}
		#endregion

		#region BackendProvider Implementation
		#region Artist
		public override async Task<bool> ArtistAddAsync(Artist artist)
		{
			// Map artist to db model
			DBModels.Artist dbArtist = autoMapper.Map<DBModels.Artist>(artist);

			// Add new artist to database
			int recordsChanged;
			try { recordsChanged = await repository.ArtistAddAsync(dbArtist); }
			catch { recordsChanged = 0; }
			return recordsChanged > 0;
		}
		public override async Task<bool> ArtistDeleteByIDAsync(int artistID)
		{
			// Delete artist from database
			int recordsChanged;
			try { recordsChanged = await repository.ArtistDeleteByIDAsync(artistID); }
			catch { recordsChanged = 0; }
			return recordsChanged > 0;
		}
		public override async Task<List<Artist>> ArtistGetAllAsync()
		{
			// Get artists from database
			List<DBModels.Artist> artists;
			try { artists = await repository.ArtistGetAllAsync(); }
			catch { artists = null; }

			// Return all artists in database
			return autoMapper.Map<List<Artist>>(artists);
		}
		public override async Task<Artist> ArtistGetByIDAsync(int artistID)
		{
			// Get artist from database
			DBModels.Artist artist;
			try { artist = await repository.ArtistGetByIDAsync(artistID); }
			catch { artist = null; }

			// Return desired artist
			return autoMapper.Map<Artist>(artist);
		}
		public override async Task<bool> ArtistUpdateAsync(Artist artist)
		{
			// Map artist to db model
			DBModels.Artist dbArtist = autoMapper.Map<DBModels.Artist>(artist);

			// Update artist
			int recordsChanged;
			try { recordsChanged = await repository.ArtistUpdateAsync(dbArtist); }
			catch { recordsChanged = 0; }
			return recordsChanged > -1;
		}
		#endregion

		#region Album
		public override async Task<bool> AlbumAddAsync(Album album)
		{
			// Map album to db model
			DBModels.Album dbAlbum = autoMapper.Map<DBModels.Album>(album);

			// Add new album to database
			int recordsChanged;
			try { recordsChanged = await repository.AlbumAddAsync(dbAlbum); }
			catch { recordsChanged = 0; }
			return recordsChanged > 0;
		}
		public override async Task<bool> AlbumDeleteByIDAsync(int artistID, int albumID)
		{
			// Delete album from database
			int recordsChanged;
			try { recordsChanged = await repository.AlbumDeleteByIDAsync(artistID, albumID); }
			catch { recordsChanged = 0; }
			return recordsChanged > 0;
		}
		public override async Task<Album> AlbumGetByIDAsync(int artistID, int albumID)
		{
			// Get album from database
			DBModels.Album album;
			try { album = await repository.AlbumGetByIDAsync(artistID, albumID); }
			catch { album = null; }

			// Return desired album
			return autoMapper.Map<Album>(album);
		}
		public override async Task<bool> AlbumUpdateAsync(Album album)
		{
			// Map album to db model
			DBModels.Album dbAlbum = autoMapper.Map<DBModels.Album>(album);

			// Update album
			int recordsChanged;
			try { recordsChanged = await repository.AlbumUpdateAsync(dbAlbum); }
			catch { recordsChanged = 0; }
			return recordsChanged > -1;
		}
		#endregion

		#region Track
		public override async Task<bool> TrackAddAsync(Track track)
		{
			// Map track to db model
			DBModels.Track dbTrack = autoMapper.Map<DBModels.Track>(track);

			// Add new track to database
			int recordsChanged;
			try { recordsChanged = await repository.TrackAddAsync(dbTrack); }
			catch { recordsChanged = 0; }
			return recordsChanged > 0;
		}
		public override async Task<bool> TrackDeleteByIDAsync(int albumID, int trackID)
		{
			// Delete track from database
			int recordsChanged;
			try { recordsChanged = await repository.TrackDeleteByIDAsync(albumID, trackID); }
			catch { recordsChanged = 0; }
			return recordsChanged > 0;
		}
		public override async Task<Track> TrackGetByIDAsync(int albumID, int trackID)
		{
			// Get track from database
			DBModels.Track track;
			try { track = await repository.TrackGetByIDAsync(albumID, trackID); }
			catch { track = null; }

			// Return desired track
			return autoMapper.Map<Track>(track);
		}
		public override async Task<bool> TrackUpdateAsync(Track track)
		{
			// Map track to db model
			DBModels.Track dbTrack = autoMapper.Map<DBModels.Track>(track);

			// Update track
			int recordsChanged;
			try { recordsChanged = await repository.TrackUpdateAsync(dbTrack); }
			catch { recordsChanged = 0; }
			return recordsChanged > -1;
		}
		#endregion
		#endregion
	}
}