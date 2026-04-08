using ClaimCare.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClaimCare.Services.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ClaimCareDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ClaimCareDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<bool> ExistsAsync(int id) => await _dbSet.FindAsync(id) != null;

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}