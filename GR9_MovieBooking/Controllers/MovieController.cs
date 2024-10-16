using GR9_MovieBooking.Contexts;
using GR9_MovieBooking.Helpers;
using GR9_MovieBooking.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GR9_MovieBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace GR9_MovieBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly GR9MovieDbContext _context;
        public MovieController(GR9MovieDbContext context)
        {
            _context = context;
        }


        [HttpGet("getById")]
        public IActionResult GetMovieById(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }
            var movie = _context.Movies.Include(m => m.Showtimes).FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpPost("edit")]
        public IActionResult Edit(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var oldMovie = _context.Movies.Find(movie.Id);
                    if (oldMovie?.Duration != movie.Duration)
                    {
                        var showtimesToDelete = _context.Showtimes.Where(s => s.MovieId == movie.Id);
                        if (showtimesToDelete.Any())
                        {
                            showtimesToDelete = showtimesToDelete.AsEnumerable().Select(s => { s.IsDeleted = true; return s; }).AsQueryable();
                            _context.UpdateRange(showtimesToDelete);
                        }
                    }
                    _context.Movies.Update(movie);
                    _context.SaveChanges();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return BadRequest(movie);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                var showtimesToDelete = _context.Showtimes.Where(s => s.MovieId == movie.Id);
                if (showtimesToDelete.Any())
                {
                    _context.Showtimes.RemoveRange(showtimesToDelete);
                }
                _context.Movies.Remove(movie);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Something went wrong");
        }
        [HttpGet("getAllMovie")]
        public IActionResult Get() { 
            return Ok(_context.Movies.ToList());
        
        }
        
        [HttpGet("getMovies")]
        public IActionResult GetMovies(string? search, int page=1)
        {
            List<Movie> movies;
            if (!string.IsNullOrEmpty(search))
            {
                movies = _context.Movies.Where(m => m.Title.Contains(search)).OrderByDescending(m => m.ReleaseDate).ToList();
            }
            else
            {
                movies = _context.Movies.OrderByDescending(m => m.ReleaseDate).ToList();
            }
            page = (page == null||page<1) ? 1 : page;
            int moviesCount = movies.Count;
            var pagination = new PaginationHelper(moviesCount, page, 24);
            int toSkip = (page - 1) * pagination.PageSize;
            movies = movies.Skip(toSkip).Take(pagination.PageSize).ToList();
            var returnMovieWithPaging = new GetMovieWithPagingDTO(movies, pagination);
            if (!movies.Any() && page != 1)
            {
                return NotFound();
            }
            return Ok(returnMovieWithPaging);
        }

        [HttpPost("Add")]
        public IActionResult Add(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();
                if (Showtimes(movie).Any())
                {
                    _context.Showtimes.AddRange(Showtimes(movie));
                }
                _context.SaveChanges();
                return Ok("The movie is added successfully");
            }
            return BadRequest(movie);
        }
        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
        private static List<Showtime> Showtimes(Movie movie)
        {
            var duration = movie.Duration.Split(' ');
            int hours = int.Parse(duration[0][..duration[0].IndexOf('h')]);
            int minutes = int.Parse(duration[1][..duration[1].IndexOf('m')]);
            if (hours > 0)
            {
                TimeOnly time = new(9, 0);
                List<Showtime> showtimes = new();
                do
                {
                    Showtime showtime = new()
                    {
                        Time = time.ToString(),
                        MovieId = movie.Id,
                    };
                    showtimes.Add(showtime);
                    time = time.AddHours(hours + 1);
                    time = time.AddMinutes(minutes);
                } while (time.IsBetween(new TimeOnly(9, 0), new TimeOnly(0, 0)));
                return showtimes;
            }
            return new List<Showtime>();
        }
    }

    
}
