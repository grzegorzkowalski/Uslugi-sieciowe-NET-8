using BlogCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogCMS.Data
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public BlogDbContext(DbContextOptions<BlogDbContext> options): base(options)
        {
        }
    }
}
