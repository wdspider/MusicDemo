using System.Collections.Generic;

namespace MusicDemo.Database.Models
{
	public class Artist
	{
		#region Properties
		public int ArtistID { get; set; }
		public string Name { get; set; }

		public virtual List<Album> Albums { get; set; }
		#endregion
	}
}
