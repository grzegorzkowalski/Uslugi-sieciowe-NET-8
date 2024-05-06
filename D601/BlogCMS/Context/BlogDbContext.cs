using BlogCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogCMS.Context
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
    }
}
