using System.ComponentModel.DataAnnotations.Schema;

namespace MusicDemo.Database.Models
{
	public class Track
	{
		#region Properties
		public int TrackID { get; set; }
		public string Name { get; set; }
		[Index(IsUnique = true)]
		public int Number { get; set; }

		public int AlbumID { get; set; }
		public virtual Album Album { get; set; }
		#endregion
	}
}
