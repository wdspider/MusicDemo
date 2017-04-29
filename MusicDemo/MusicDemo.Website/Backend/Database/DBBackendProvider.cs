using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MusicDemo.Database;
using MusicDemo.Website.Models;
using DBModels = MusicDemo.Database.Models;

namespace MusicDemo.Website.Backend.Database
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
			// Add new artist to database
			int recordsChanged = await repository.ArtistAddAsync(autoMapper.Map<DBModels.Artist>(artist));
			return recordsChanged == 1;
		}
		public override async Task<bool> ArtistDeleteByIDAsync(int artistID)
		{
			// Delete artist from database
			int recordsChanged = await repository.ArtistDeleteByIDAsync(artistID);
			return recordsChanged > 0;
		}
		public override async Task<List<Artist>> ArtistGetAllAsync()
		{
			// Return all artists in database
			return autoMapper.Map<List<Artist>>(await repository.ArtistGetAllAsync());
		}
		public override async Task<Artist> ArtistGetByIDAsync(int artistID)
		{
			// Return desired artist
			return autoMapper.Map<Artist>(await repository.ArtistGetByIDAsync(artistID));
		}
		public override async Task<bool> ArtistUpdateAsync(Artist artist)
		{
			// Update artist
			int recordsChanged = await repository.ArtistUpdateAsync(autoMapper.Map<DBModels.Artist>(artist));
			return recordsChanged == 1;
		}
		#endregion

		#region Album
		public override async Task<bool> AlbumAddAsync(Album album)
		{
			// Add new album to database
			int recordsChanged = await repository.AlbumAddAsync(autoMapper.Map<DBModels.Album>(album));
			return recordsChanged == 1;
		}
		public override async Task<bool> AlbumDeleteByIDAsync(int albumID)
		{
			// Delete album from database
			int recordsChanged = await repository.AlbumDeleteByIDAsync(albumID);
			return recordsChanged > 0;
		}
		public override async Task<Album> AlbumGetByIDAsync(int albumID)
		{
			// Return desired album
			return autoMapper.Map<Album>(await repository.AlbumGetByIDAsync(albumID));
		}
		public override async Task<bool> AlbumUpdateAsync(Album album)
		{
			// Update album
			int recordsChanged = await repository.AlbumUpdateAsync(autoMapper.Map<DBModels.Album>(album));
			return recordsChanged == 1;
		}
		#endregion

		#region Track
		public override async Task<bool> TrackAddAsync(Track track)
		{
			// Add new track to database
			int recordsChanged = await repository.TrackAddAsync(autoMapper.Map<DBModels.Track>(track));
			return recordsChanged == 1;
		}
		public override async Task<bool> TrackDeleteByIDAsync(int trackID)
		{
			// Delete track from database
			int recordsChanged = await repository.TrackDeleteByIDAsync(trackID);
			return recordsChanged > 0;
		}
		public override async Task<Track> TrackGetByIDAsync(int traickID)
		{
			// Return desired track
			return autoMapper.Map<Track>(await repository.TrackGetByIDAsync(traickID));
		}
		public override async Task<bool> TrackUpdateAsync(Track track)
		{
			// Update track
			int recordsChanged = await repository.TrackUpdateAsync(autoMapper.Map<DBModels.Track>(track));
			return recordsChanged == 1;
		}
		#endregion
		#endregion
	}
}