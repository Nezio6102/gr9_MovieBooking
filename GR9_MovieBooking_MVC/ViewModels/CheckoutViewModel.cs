using System.ComponentModel.DataAnnotations;

namespace GR9_MovieBooking_MVC.ViewModels
{
	public class CheckoutViewModel
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		//[Required]
		//[EmailAddress]
		//public string Email { get; set; }
	}
}
