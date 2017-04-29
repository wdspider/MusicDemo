using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicDemo.Website.ViewModels
{
	public class AlbumDetailsViewModel
	{
		#region Properties
		[Display(Name = "Album ID")]
		public int AlbumID { get; set; }
		[Display(Name = "Album Name")]
		public string Name { get; set; }
		[Display(Name = "Artist ID")]
		public int ArtistID { get; set; }
		public List<TrackViewModel> Tracks { get; set; }
		#endregion

	}
}