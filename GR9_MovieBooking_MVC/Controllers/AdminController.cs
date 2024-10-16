//using GR9_MovieBooking_MVC.Contexts;
//using GR9_MovieBooking_MVC.Helpers;
//using GR9_MovieBooking_MVC.Helpers.EmailSenderHelper;
using GR9_MovieBooking_MVC.Models;
using GR9_MovieBooking_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;


//using Microsoft.EntityFrameworkCore;
using System.Web;

namespace GR9_MovieBooking_MVC.Controllers
{
	public class AdminController : Controller
	{
		
		private readonly HttpClient _client = new HttpClient();
		[AllowAnonymous]
		public async  Task<IActionResult> Index()
		{
			

			if (TempData["Username"] != null)
			{
				return RedirectToAction(nameof(Add));
			}
			return View();
				}
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Index(AdminLoginViewModel adminLoginViewModel, string? returnUrl)
		{
			var json = JsonConvert.SerializeObject(adminLoginViewModel);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			
			HttpResponseMessage response = await _client.PostAsync("http://localhost:6969/api/Admin/Login", content);
			Console.WriteLine(response.StatusCode);
			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction(nameof(Add));
			}
			returnUrl ??= Url.Content("~/Admin/Add");
			if (ModelState.IsValid)
			{
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					HttpContext.Session.SetString("Username",adminLoginViewModel.Username);
					return LocalRedirect(returnUrl);
				}
				if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
				{
					ViewBag.Error = "This account is locked out.";
				}
				else
				{
					ViewBag.Error = "Incorrect username or password.";
				}
			}
			return View(adminLoginViewModel);
		}
		[AllowAnonymous]
		public IActionResult Forgot()
		{
			return View();
		}
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Forgot(AdminForgotViewModel adminForgotViewModel)
		{
			var json = JsonConvert.SerializeObject(adminForgotViewModel);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _client.PostAsync("http://localhost:6969/api/Admin/Forgot", content);

			if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
				ViewBag.Message = "Account doesnt exist";
			}
			if (response.IsSuccessStatusCode) {
				ViewBag.Message = "Please check your email to reset your password.";
			}
			
			return View();
			
		}
		[AllowAnonymous]
		public IActionResult Reset(string Email, string Token)
		{
			if (Email == null || Token == null)
			{
				ViewBag.Error = "Invalid password reset URL.";
			}
			return View();
		}
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Reset(AdminResetViewModel adminResetViewModel)
		{
			var json = JsonConvert.SerializeObject(adminResetViewModel);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _client.PostAsync("http://localhost:6969/api/Admin/ResetPassword", content);

			if (response.StatusCode != System.Net.HttpStatusCode.BadRequest)
			{
				if (response.IsSuccessStatusCode) {
					ViewBag.Message = "Your password is reset. Please click <a href='/Admin/' class='alert-link'>here to login</a>";
				}
				else
				{
					ViewBag.Errors = response.Content;
				}
				return View();
			}
			return View(adminResetViewModel);
		}
		public IActionResult Add()
			{
				return View();
			}
		[HttpPost]
		public async Task<IActionResult> Add(Movie movie)
		{
			var json = JsonConvert.SerializeObject(movie);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _client.PostAsync("http://localhost:6969/api/Movie/Add", content);

			if (ModelState.IsValid)
			{
				if (response.IsSuccessStatusCode)
				{
					TempData["Success"] = "The movie was added successfully";
					return RedirectToAction(nameof(Add));
				}
			}
			return View(movie);
		}
		public async Task<IActionResult> Movies(string search = "", int page = 1)
		{
			HttpResponseMessage response = await _client.GetAsync("http://localhost:6969/api/Movie/getMovies?search=" + search + "&page=" + page);
			if (response.IsSuccessStatusCode)
			{
				var jsonContent = await response.Content.ReadAsStringAsync();
				GetMovieWithPagingDTO mvWithPagin = JsonConvert.DeserializeObject<GetMovieWithPagingDTO>(jsonContent);
				List<Movie> movies = mvWithPagin.movies;
				ViewBag.Search = search;
				ViewBag.Pagination = mvWithPagin.pagination;
				TempData["CurrentPage"] = page;
				return View(movies);
			}
			return View();
			
		}
		public async Task<IActionResult> Edit(int? id)
		{
			HttpResponseMessage responseMessage = await _client.GetAsync("http://localhost:6969/api/Movie/getById?id=" + id);
			if (!responseMessage.IsSuccessStatusCode) return NotFound();
			var jsonContent = await responseMessage.Content.ReadAsStringAsync();
			Movie movie = JsonConvert.DeserializeObject<Movie>(jsonContent);
			return View(movie);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, Movie movie)
		{
			if (id != movie.Id)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				var json = JsonConvert.SerializeObject(movie);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage responseMessage = await _client.PostAsync("http://localhost:6969/api/Movie/edit?id=" + id,content);
				Console.WriteLine(responseMessage.StatusCode);
				if (responseMessage.IsSuccessStatusCode)
				{
					
					TempData["Success"] = "The movie was updated successfully";
				}
				
				return RedirectToAction(nameof(Movies), new { page = CurrentPage() });
			}
			return View(movie);
		}
		public async Task<IActionResult> Delete(int? id)
		{
			HttpResponseMessage responseMessage = await _client.DeleteAsync("http://localhost:6969/api/Movie/delete?id=" + id);
			if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound) return NotFound();	
			if (responseMessage.IsSuccessStatusCode)
				TempData["Success"] = "The movie was deleted successfully";
			
			return RedirectToAction(nameof(Movies), new { page = CurrentPage() });
		}
		public IActionResult Account()
		{
			Console.WriteLine("Session: " + HttpContext.Session.GetString("Username"));
			if (HttpContext.Session.GetString("Username") != null)
			{
				AdminAccountViewModel adminEditViewModel = new()
				{
					Username = HttpContext.Session.GetString("Username")
				};
				return View(adminEditViewModel);
			}
			return NotFound();
		}
		[HttpPost]
		public async Task<IActionResult> Account(AdminAccountViewModel adminAccountViewModel)
		{
			if (ModelState.IsValid)
			{
				var json = JsonConvert.SerializeObject(adminAccountViewModel);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage responseMessage = await _client.PostAsync("http://localhost:6969/api/Admin/ChangePassword" , content);
				if (responseMessage != null)
				{
					
					if (responseMessage.IsSuccessStatusCode)
					{
						TempData["Success"] = "Your account was updated successfully";
					}
					else
					{
						ViewBag.Errors = "Error saving changes";
					}
				}
				else
				{
					return NotFound();
				}
			}
			return View(adminAccountViewModel);
		}
		public async Task<IActionResult> Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction(nameof(Index));
		}
		//	private bool MovieExists(int id)
		//	{
		//		return _context.Movies.Any(e => e.Id == id);
		//	}
		//	private static List<Showtime> Showtimes(Movie movie)
		//	{
		//		var duration = movie.Duration.Split(' ');
		//		int hours = int.Parse(duration[0][..duration[0].IndexOf('h')]);
		//		int minutes = int.Parse(duration[1][..duration[1].IndexOf('m')]);
		//		if (hours > 0)
		//		{
		//			TimeOnly time = new(9, 0);
		//			List<Showtime> showtimes = new();
		//			do
		//			{
		//				Showtime showtime = new()
		//				{
		//					Time = time.ToString(),
		//					MovieId = movie.Id,
		//				};
		//				showtimes.Add(showtime);
		//				time = time.AddHours(hours + 1);
		//				time = time.AddMinutes(minutes);
		//			} while (time.IsBetween(new TimeOnly(9, 0), new TimeOnly(0, 0)));
		//			return showtimes;
		//		}
		//		return new List<Showtime>();
		//	}
		private int CurrentPage()
		{
			return (int)(TempData["CurrentPage"] ?? 1);
		}
	}
}