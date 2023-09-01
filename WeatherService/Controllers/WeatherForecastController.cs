using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Models;  // Assuming your DbContext and Model are in a folder called Models

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherContext _context;  // Injecting the DbContext

        public WeatherController(WeatherContext context)
        {
            _context = context;  // Initialization
        }

        // GET api/weather/{city}
        [HttpGet("{city}")]
        public ActionResult<WeatherInfo> Get(string city)
        {
            // Validation of the input can be done here
            if (string.IsNullOrEmpty(city) || string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("Invalid city name.");
            }

            // Querying the database to find weather information for the city
            var weather = _context.WeatherInfos.FirstOrDefault(w => w.City.ToLower() == city.ToLower());

            // If no data found, return a NotFound status
            if (weather == null)
            {
                return NotFound($"Weather information for {city} not found.");
            }

            // If data found, return it
            return Ok(weather);
        }
    }
}