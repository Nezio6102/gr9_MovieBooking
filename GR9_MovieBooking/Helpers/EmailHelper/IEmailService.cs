namespace GR9_MovieBooking.Helpers.EmailSenderHelper
{
	public interface IEmailService
	{
		Task Send(EmailModel message);
	}
}