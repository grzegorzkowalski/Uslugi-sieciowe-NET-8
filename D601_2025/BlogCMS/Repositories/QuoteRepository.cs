using BlogCMS.Data;
using BlogCMS.Interfaces;
using BlogCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogCMS.Repositories
{
    public class QuoteRepository : IRepository<Quote>
    {
        private readonly ApplicationDbContext _context;

        public QuoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Quote> CreateAsync(Quote entity)
        {
            _context.Quotes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if (quote != null)
            {
                _context.Quotes.Remove(quote);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Quote>> GetAllAsync()
        {
            return await _context.Quotes.ToListAsync();
        }

        public async Task<Quote> GetByIdAsync(int id)
        {
            return await _context.Quotes.FindAsync(id);
        }

        public async Task UpdateAsync(Quote entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
