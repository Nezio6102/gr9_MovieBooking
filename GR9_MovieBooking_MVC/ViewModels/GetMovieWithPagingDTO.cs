using GR9_MovieBooking_MVC.Helpers;
using GR9_MovieBooking_MVC.Models;
namespace GR9_MovieBooking_MVC.ViewModels
{
	public class GetMovieWithPagingDTO
	{
        public List<Movie> movies { get; set; }
        public PaginationHelper pagination { get; set; }

		public GetMovieWithPagingDTO(List<Movie> movies, PaginationHelper pagination)
		{
			this.movies = movies;
			this.pagination = pagination;
		}

	}
}
