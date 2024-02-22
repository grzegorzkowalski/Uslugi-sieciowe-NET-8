using BlogGrpcService.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogGrpcService.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
    }
}
