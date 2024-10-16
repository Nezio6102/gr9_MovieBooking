using GR9_MovieBooking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GR9_MovieBooking.Contexts
{
	public class GR9MovieDbContext : IdentityDbContext
	{
		public GR9MovieDbContext() { }
		public GR9MovieDbContext(DbContextOptions<GR9MovieDbContext> options) : base(options) { }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<Showtime> Showtimes { get; set; }
		public DbSet<Showdate> Showdates { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Seed();
		}
	}
}
