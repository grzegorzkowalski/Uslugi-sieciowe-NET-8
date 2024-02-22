using Microsoft.EntityFrameworkCore;
using WeatherWorkerService.Models;

namespace BlogCMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Weather> Weather { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
    }
}
