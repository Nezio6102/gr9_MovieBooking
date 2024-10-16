using System.ComponentModel.DataAnnotations;

namespace GR9_MovieBooking_MVC.Models
{
	public class Reservation
	{
		[Key]
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? Email { get; set; }
		public int ShowdateId { get; set; }
		public virtual Showdate Showdate { get; set; }
	}
}
