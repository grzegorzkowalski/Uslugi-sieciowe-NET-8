using BlogCMS.Data;
using BlogCMS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogCMS.Repository
{
    public class EfCoreRepository<T> : IRepository<T> where T : class
    {
        private readonly BlogDbContext _context;
        private DbSet<T> _entities;

        public EfCoreRepository(BlogDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _entities.ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _entities.FindAsync(id);

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
            var state = _context.Entry(entity).State == EntityState.Modified;
            return state;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var el = await GetByIdAsync(id);
            if (el != null)
            {
                _entities.Remove(el);
                await _context.SaveChangesAsync();
            }
            var state = _context.Entry(el).State == EntityState.Deleted;
            return state;
        }
    }
}
