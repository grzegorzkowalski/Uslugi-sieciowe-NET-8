using Microsoft.EntityFrameworkCore;
using Postcard.gRPC.Models;

namespace Postcard.gRPC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<PostcardModel> Postcards { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
    }
}
