//using BarcodeLib;
using GR9_MovieBooking.Contexts;
using GR9_MovieBooking.Models;
using GR9_MovieBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace GR9_MovieBooking.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    public class CheckoutController : ControllerBase
	{
		private readonly GR9MovieDbContext _context;
		public CheckoutController(GR9MovieDbContext context)
		{
			_context = context;
		}
		
		
		[HttpPost("/checkout")]
		public IActionResult Checkout(MovieViewModel movieViewModel)
		{
			var selectedShowdate = movieViewModel.SelectedShowdate;
			var selectedShowtime = movieViewModel.SelectedShowtime;
			var selectedMovie = _context.Showtimes.Find(selectedShowtime)?.MovieId;
			var inDateTime = _context.Showdates.Where(s => s.Date == selectedShowdate && s.ShowtimeId == selectedShowtime);
			if (inDateTime.Any())
			{
				if (inDateTime.Select(a => a.Reserved).FirstOrDefault() == 100)
				{
					return BadRequest();
				}
			}
			CheckoutReturnDTO checkoutReturnDTO = new CheckoutReturnDTO(_context.Movies.Find(selectedMovie)?.Title.ToString(),
                _context.Movies.Find(selectedMovie)?.Poster.ToString(),
                _context.Showtimes.Find(selectedShowtime)?.Time.ToString(),
                selectedShowdate.ToString("MM/dd/yyyy")
				);
			return Ok(checkoutReturnDTO);
		}
		
		[HttpPost("Ticket")]
		public IActionResult Ticket(string SessionId, string PaymentStatus, string Showdate, int ShowtimeId, string FirstName, string LastName)
		{
			if (SessionId != null)
			{
				//var service = new SessionService();
				//Session session = service.Get(HttpContext.Session.GetString("SessionId"));
				if (PaymentStatus == "paid")
				{
					var showdate = _context.Showdates.Where(s => s.Date == DateTime.Parse(Showdate) && s.ShowtimeId == ShowtimeId).FirstOrDefault();
					if (showdate != null)
					{
						showdate.Reserved += 1;
						_context.Showdates.Update(showdate);
					}
					else
					{
						showdate = new()
						{
							Date = DateTime.Parse(Showdate) == null ?DateTime.Parse(Showdate): DateTime.Now,
							Reserved = 1,
							ShowtimeId = ShowtimeId,
						};
						_context.Showdates.Add(showdate);
					}
					Reservation reservation = new()
					{
						FirstName = FirstName ?? string.Empty,
						LastName = LastName ?? string.Empty,
						Showdate = showdate,
					};
					_context.Reservations.Add(reservation);
					_context.SaveChanges();
					TicketViewModel ticketViewModel = new()
					{
						TicketNumber = reservation.Id,
						FullName = FirstName + LastName,
						MovieTitle = _context.Movies.First(m => m.Id == _context.Showtimes.First(s => s.Id == ShowtimeId).MovieId).Title,
						Date = DateTime.Parse(Showdate ?? DateTime.Now.ToString()),
						Time = _context.Showtimes.First(s => s.Id == ShowtimeId).Time,
					};
					return Ok(ticketViewModel);
				}
			}
			return NotFound();
		}
	}
}