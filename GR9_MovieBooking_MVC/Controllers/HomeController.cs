using System.Drawing;
using System.Drawing.Imaging;
using GR9_MovieBooking_MVC.Models;
using GR9_MovieBooking_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BarcodeLib;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace GR9_MovieBooking_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:6969/api/Movie/getAllMovie");
            string jsonContent = await response.Content.ReadAsStringAsync();
            List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(jsonContent);
            var nowPlaying = movies.Where(m => m.StartDate <= DateTime.UtcNow && m.EndDate > DateTime.UtcNow).OrderByDescending(m => m.Id).ToList();
            var comingSoon = movies.Where(m => m.StartDate > DateTime.UtcNow).ToList();
            ViewBag.NowPlaying = nowPlaying;
            ViewBag.ComingSoon = comingSoon;
            return View();
        }
        public async Task<IActionResult> Movies(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return NotFound();
            }
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:6969/api/Movie/getmovies?search=" + search);
            if (!response.IsSuccessStatusCode) { return NotFound(); }
            string jsonContent = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movie>>(jsonContent).Where( m=>m.EndDate >= DateTime.Now).OrderByDescending(m => m.Id).ToList();
            ViewBag.Search = search;
            return View(movies);
        }
        public async Task<IActionResult> Movie(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:6969/api/Movie/getById/?id="+id);
            string jsonContent = await response.Content.ReadAsStringAsync();
            Movie movie = JsonConvert.DeserializeObject<Movie>(jsonContent);
            if (movie == null || movie.EndDate<=DateTime.Now) { return NotFound();}
            
            var showtimes = movie.Showtimes.Where(s => s.IsDeleted == false).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Time.ToString() });
            var movieViewModel = new MovieViewModel { Movie = movie, Showtimes = showtimes, SelectedShowdate = DateTime.Now, SelectedShowtime = 0 };
            return View(movieViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(MovieViewModel movieViewModel)
        {
            HttpResponseMessage movieResponse = await _httpClient.GetAsync("http://localhost:6969/api/Movie/getAllMovie");
            string movieJsonContent = await movieResponse.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movie>>(movieJsonContent);

            HttpResponseMessage showDateResponse = await _httpClient.GetAsync("http://localhost:6969/api/ShowDateTime/getAllShowDate");
            string showDateJsonContent = await showDateResponse.Content.ReadAsStringAsync();
            var showDates = JsonConvert.DeserializeObject<List<Showdate>>(showDateJsonContent);

            HttpResponseMessage showTimeResponse = await _httpClient.GetAsync("http://localhost:6969/api/ShowDateTime/getAllShowTime");
            string showTimeJsonContent = await showTimeResponse.Content.ReadAsStringAsync();
            var showTimes = JsonConvert.DeserializeObject<List<Showtime>>(showTimeJsonContent);

            var selectedShowdate = movieViewModel.SelectedShowdate;
            var selectedShowtime = movieViewModel.SelectedShowtime;
            var selectedMovie = showTimes.FirstOrDefault(s => s.Id == selectedShowtime)?.MovieId;
            var inDateTime = showDates.Where(s => s.Date == selectedShowdate && s.ShowtimeId == selectedShowtime);
            if (inDateTime.Any())
            {
                if (inDateTime.Select(a => a.Reserved).FirstOrDefault() == 100)
                {
                    TempData["Info"] = "The screening time you chose is full.";
                    return RedirectToAction(nameof(Movie), new { id = selectedMovie });
                }
            }
            ViewBag.MTitle = movies.FirstOrDefault(m=>m.Id==selectedMovie)?.Title.ToString();
            ViewBag.MPoster = movies.FirstOrDefault(m => m.Id == selectedMovie)?.Poster.ToString();
            ViewBag.Showtime = showTimes.FirstOrDefault(s => s.Id == selectedShowtime)?.Time.ToString();
            ViewBag.Showdate = selectedShowdate.ToString("MM/dd/yyyy");
            HttpContext.Session.SetInt32("ShowtimeId", selectedShowtime);
            HttpContext.Session.SetString("Showdate", selectedShowdate.ToString());
            return View();
        }
        [HttpPost]
        public IActionResult CheckoutPost(CheckoutViewModel checkoutViewModel)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("FirstName", checkoutViewModel.FirstName);
                HttpContext.Session.SetString("LastName", checkoutViewModel.LastName);
                //HttpContext.Session.SetString("Email", checkoutViewModel.Email);
               
                
            }
            return RedirectToAction(nameof(Ticket));
        }
        public IActionResult Ticket()
        {
            
                
                    
                    TicketViewModel ticketViewModel = new()
                    {
                        TicketNumber = 1,
                        FullName = HttpContext.Session.GetString("FirstName") + " " + HttpContext.Session.GetString("LastName"),
                        MovieTitle = "Title go here",
                        Date = DateTime.Now.Date,
                        Time = DateTime.Now.ToString()
                    };
                    HttpContext.Session.Clear();
                    return View(ticketViewModel);
            
        }
        public IActionResult GenerateBarcode(string toEncode)
        {
            Barcode barcode = new();
            Image image = barcode.Encode(TYPE.CODE39Extended, toEncode, Color.Black, Color.White, 2500, 1000);
            return File(ConvertImageToByte(image), "image/jpeg");
        }
        private static byte[] ConvertImageToByte(Image image)
        {
            using MemoryStream memoryStream = new();
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            return memoryStream.ToArray();
        }
    }
}
