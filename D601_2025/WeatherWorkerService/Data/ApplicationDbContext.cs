using Microsoft.EntityFrameworkCore;
using WeatherWorkerService.Models;

namespace WeatherWorkerService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<WeatherModel> Weather { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
