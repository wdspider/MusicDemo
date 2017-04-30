using System.Linq;
using AutoMapper;
using MusicDemo.Website.Backend.Models;

namespace MusicDemo.Website.ViewModels
{
	public class ViewModelMappingProfile : Profile
	{
		public ViewModelMappingProfile()
		{
			// Map Models => View Models
			CreateMap<Artist, ArtistViewModel>();
			CreateMap<Artist, ArtistDetailsViewModel>()
				.ForMember(m => m.Albums, opt => opt.MapFrom(s => s.Albums.OrderBy(a => a.Name).ToList()));
			CreateMap<Album, AlbumViewModel>();
			CreateMap<Album, AlbumDetailsViewModel>()
				.ForMember(m => m.Tracks, opt => opt.MapFrom(s => s.Tracks.OrderBy(t => t.Number).ToList()));
			CreateMap<Track, TrackViewModel>()
				.ForMember(m => m.ArtistID, opt => opt.Ignore());

			// Map View Models => Models
			CreateMap<ArtistViewModel, Artist>()
				.ForMember(m => m.Albums, opt => opt.Ignore());
			CreateMap<AlbumViewModel, Album>()
				.ForMember(m => m.Tracks, opt => opt.Ignore());
			CreateMap<TrackViewModel, Track>();
		}
	}
}