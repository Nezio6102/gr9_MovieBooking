using GR9_MovieBooking.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GR9_MovieBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowDateTimeController : ControllerBase
    {
        private readonly GR9MovieDbContext _context;
        public ShowDateTimeController(GR9MovieDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAllShowDate")]
        public async Task<IActionResult> getAllShowDate()
        {
            return Ok(_context.Showdates.ToList());
        }

        [HttpGet("getAllShowTime")]
        public async Task<IActionResult> getAllShowTime()
        {
            return Ok(_context.Showtimes.ToList()); 
        }
    }
}
