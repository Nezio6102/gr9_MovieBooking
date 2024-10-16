using GR9_MovieBooking.Helpers;
using GR9_MovieBooking.Models;
namespace GR9_MovieBooking.ViewModels
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
