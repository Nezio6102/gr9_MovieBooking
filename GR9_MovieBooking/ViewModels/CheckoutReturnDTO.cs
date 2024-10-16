namespace GR9_MovieBooking.ViewModels
{
    public class CheckoutReturnDTO
    {
        public string MTitle { get; set; }
        public string MPoster  { get; set; }
        public string ShowTime { get; set; }
        public string ShowDate { get; set; }

        public CheckoutReturnDTO(string mTitle, string mPoster, string showTime, string showDate)
        {
            MTitle = mTitle;
            MPoster = mPoster;
            ShowTime = showTime;
            ShowDate = showDate;
        }

        public CheckoutReturnDTO()
        {
        }
    }
}
