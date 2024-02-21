using Microsoft.EntityFrameworkCore;
using RandomQuoteGrpc.Models;

namespace RandomQuoteGrpc.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<QuoteModel> Quotes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tutaj możesz zdefiniować początkowy zestaw cytatów do migracji
        }
    }
}
