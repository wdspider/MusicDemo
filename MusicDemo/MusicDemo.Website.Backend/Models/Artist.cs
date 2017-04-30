using System.Collections.Generic;

namespace MusicDemo.Website.Backend.Models
{
	public class Artist
	{
		#region Properties
		public int ArtistID { get; set; }
		public string Name { get; set; }
		public List<Album> Albums { get; set; }
		#endregion
	}
}