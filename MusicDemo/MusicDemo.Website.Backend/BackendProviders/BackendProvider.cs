using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MusicDemo.Website.Backend.Models;

namespace MusicDemo.Website.Backend.BackendProviders
{
	public abstract class BackendProvider
	{
		#region Internal State
		protected readonly IMapper autoMapper;
		#endregion

		#region Constructors
		protected BackendProvider(IMapper mapper)
		{
			// Initialize internal state
			autoMapper = mapper;
		}
		#endregion

		#region Class Methods
		#region Artist
		public abstract Task<bool> ArtistAddAsync(Artist artist);
		public abstract Task<bool> ArtistDeleteByIDAsync(int artistID);
		public abstract Task<List<Artist>> ArtistGetAllAsync();
		public abstract Task<Artist> ArtistGetByIDAsync(int artistID);
		public abstract Task<bool> ArtistUpdateAsync(Artist artist);
		#endregion

		#region Album
		public abstract Task<bool> AlbumAddAsync(Album album);
		public abstract Task<bool> AlbumDeleteByIDAsync(int artistID, int albumID);
		public abstract Task<Album> AlbumGetByIDAsync(int artistID, int albumID);
		public abstract Task<bool> AlbumUpdateAsync(Album album);
		#endregion

		#region Track
		public abstract Task<bool> TrackAddAsync(Track track);
		public abstract Task<bool> TrackDeleteByIDAsync(int albumID, int trackID);
		public abstract Task<Track> TrackGetByIDAsync(int albumID, int trackID);
		public abstract Task<bool> TrackUpdateAsync(Track track);
		#endregion
		#endregion
	}
}
