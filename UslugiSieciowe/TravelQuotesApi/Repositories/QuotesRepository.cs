using Microsoft.EntityFrameworkCore;
using TravelQuotesApi.Data;
using TravelQuotesApi.Interfaces;
using TravelQuotesApi.Models;

namespace TravelQuotesApi.Repositories
{
    public class QuotesRepository : IRepository<Quote>
    {
        private readonly ApplicationDbContext _context;
        public QuotesRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Quote>> GetAllAsync()
        {
            return await _context.Quotes.ToListAsync();
        }

        public async Task<Quote> GetByIdAsync(int id)
        {
            return await _context.Quotes.FindAsync(id);
        }

        public async Task CreateAsync(Quote entity)
        {
            _context.Quotes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var quote = await GetByIdAsync(id);
            if (quote != null)
            {
                _context.Quotes.Remove(quote);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Quote entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
