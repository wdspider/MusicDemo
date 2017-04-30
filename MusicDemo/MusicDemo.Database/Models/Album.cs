using System.Collections.Generic;

namespace MusicDemo.Database.Models
{
	public class Album
	{
		#region Properties
		public int AlbumID { get; set; }
		public string Name { get; set; }

		public int ArtistID { get; set; }
		public virtual List<Track> Tracks { get; set; }
		#endregion
	}
}
