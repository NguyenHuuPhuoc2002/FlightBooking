using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomesController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetList()
        {
            return Ok();
        }
    }
    
}
