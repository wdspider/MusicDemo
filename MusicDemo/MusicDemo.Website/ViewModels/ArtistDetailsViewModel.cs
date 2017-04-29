using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicDemo.Website.ViewModels
{
	public class ArtistDetailsViewModel
	{
		#region Propreties
		[Display(Name = "Artist ID")]
		public int ArtistID { get; set; }
		[Display(Name = "Artist Name")]
		public string Name { get; set; }
		public List<AlbumViewModel> Albums { get; set; }
		#endregion
	}
}