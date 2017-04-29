using System.Data.Entity;
using MusicDemo.Database.Models;

namespace MusicDemo.Database
{
	public class MusicDemoDbContext : DbContext
	{
		#region Tables
		public virtual DbSet<Artist> Artists { get; set; }
		public virtual DbSet<Album> Albums { get; set; }
		public virtual DbSet<Track> Tracks { get; set; }
		#endregion
	}
}
