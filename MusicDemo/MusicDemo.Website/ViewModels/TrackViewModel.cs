using System.ComponentModel.DataAnnotations;

namespace MusicDemo.Website.ViewModels
{
	public class TrackViewModel
	{
		#region Properties
		[Display(Name = "Track ID")]
		public int TrackID { get; set; }
		[Display(Name = "Track Name")]
		[Required]
		public string Name { get; set; }
		[Display(Name = "Track Number")]
		[Required]
		public int Number { get; set; }

		[Display(Name = "Album ID")]
		public int AlbumID { get; set; }
		[Display(Name = "Artist ID")]
		public int ArtistID { get; set; }
		#endregion
	}
}