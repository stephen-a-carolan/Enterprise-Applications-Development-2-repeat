using Microsoft.EntityFrameworkCore;

namespace WeatherService.Models
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
        {
        }

        public DbSet<WeatherInfo> WeatherInfos { get; set; }  // DbSet for WeatherInfo
    }
}

