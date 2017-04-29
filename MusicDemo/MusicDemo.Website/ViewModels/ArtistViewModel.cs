using System.ComponentModel.DataAnnotations;

namespace MusicDemo.Website.ViewModels
{
	public class ArtistViewModel
	{
		#region Properties
		[Display(Name = "Artist ID")]
		public int ArtistID { get; set; }
		[Required]
		[Display(Name = "Artist Name")]
		public string Name { get; set; }
		#endregion
	}
}