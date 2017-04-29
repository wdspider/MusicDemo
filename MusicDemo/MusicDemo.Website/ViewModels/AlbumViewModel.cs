using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicDemo.Website.ViewModels
{
	public class AlbumViewModel
	{
		#region Properties
		[Display(Name = "Album ID")]
		public int AlbumID { get; set; }

		[Display(Name = "Album Name")]
		[Required]
		public string Name { get; set; }

		[Display(Name = "Artist ID")]
		public int ArtistID { get; set; }
		#endregion
	}
}