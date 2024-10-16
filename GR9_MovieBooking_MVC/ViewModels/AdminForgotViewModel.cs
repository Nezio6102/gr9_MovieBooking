using System.ComponentModel.DataAnnotations;

namespace GR9_MovieBooking_MVC.ViewModels
{
	public class AdminForgotViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
