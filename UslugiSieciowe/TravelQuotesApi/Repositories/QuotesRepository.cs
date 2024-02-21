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

        public async Task CreateAsync(Quote entity)
        {
            _context.Quotes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Quote>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Quote> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Quote entity)
        {
            throw new NotImplementedException();
        }
    }
}
