using GR9_MovieBooking.Models;

namespace GR9_MovieBooking.ViewModels
{
    public class GetAllMovieDTO
    {
        public List<Movie> movies { get; set; }
        public string search { get; set; }
        public int MyProperty { get; set; }
    }
}
