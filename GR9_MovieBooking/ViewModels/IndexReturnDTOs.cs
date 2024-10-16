using GR9_MovieBooking.Models;

namespace GR9_MovieBooking.ViewModels
{
    public class IndexReturnDTOs
    {
        public List<Movie> playingNow { get; set; }
        public List<Movie> comingSoon { get; set; }

        public IndexReturnDTOs() { }

        public IndexReturnDTOs(List<Movie> playingNow, List<Movie> comingSoon)
        {
            this.playingNow = playingNow;
            this.comingSoon = comingSoon;
        }
    }
}
