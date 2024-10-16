using System.ComponentModel.DataAnnotations;

namespace GR9_MovieBooking_MVC.Models
{
	public class Showdate
	{
		[Key]
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public int Reserved { get; set; }
		public int ShowtimeId { get; set; }
		public virtual Showtime Showtime { get; set; }
		public virtual ICollection<Reservation> Reservations { get; set; }
	}
}
