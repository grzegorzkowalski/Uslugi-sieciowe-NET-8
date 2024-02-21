using Microsoft.EntityFrameworkCore;
using TravelQuotesApi.Models;

namespace TravelQuotesApi.Data
{
    public class ApplicationDbContext : DbContext
    {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
      {
      }

      public DbSet<Quote> Quotes { get; set; }
    }
}
