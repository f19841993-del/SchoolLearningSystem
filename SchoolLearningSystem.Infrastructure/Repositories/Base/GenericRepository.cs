// في ملف GenericRepository.cs
using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Interfaces.Base;
using SchoolLearningSystem.Infrastructure.Data;
namespace SchoolLearningSystem.Infrastructure.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context) => _context = context;

        public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
        public async Task UpdateAsync(T entity) => _context.Set<T>().Update(entity);
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null) _context.Set<T>().Remove(entity);
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }

}
