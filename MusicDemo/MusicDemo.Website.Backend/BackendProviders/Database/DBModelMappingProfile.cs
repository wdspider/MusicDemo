using AutoMapper;
using MusicDemo.Website.Backend.Models;
using DBModels = MusicDemo.Database.Models;

namespace MusicDemo.Website.Backend.BackendProviders.Database
{
	public class DBModelMappingProfile : Profile
	{
		public DBModelMappingProfile()
		{
			// Map DB Models -> Website Models
			CreateMap<DBModels.Artist, Artist>();
			CreateMap<DBModels.Album, Album>();
			CreateMap<DBModels.Track, Track>();

			// Map Website Models -> DB Models
			CreateMap<Artist, DBModels.Artist>()
				.ForMember(m => m.Albums, opt => opt.Ignore());
			CreateMap<Album, DBModels.Album>()
				.ForMember(m => m.Tracks, opt => opt.Ignore());
			CreateMap<Track, DBModels.Track>();
		}
	}
}