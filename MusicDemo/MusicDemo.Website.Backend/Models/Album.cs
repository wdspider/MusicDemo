using System.Collections.Generic;

namespace MusicDemo.Website.Backend.Models
{
	public class Album
	{
		#region Properties
		public int AlbumID { get; set; }
		public string Name { get; set; }

		public int ArtistID { get; set; }
		public List<Track> Tracks { get; set; }
		#endregion
	}
}