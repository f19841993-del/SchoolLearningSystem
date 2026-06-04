using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;


namespace SchoolLearningSystem.Infrastructure.Infrastructure
{
    public class ResultRepository : IResultRepository
    {
        private readonly AppDbContext _context;

        public ResultRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Result>> GetAllAsync()
        {
            return await _context.Results.Include(r => r.Exam).Include(r => r.Student).ToListAsync();
        }

        public async Task<Result?> GetByIdAsync(int id)
        {
            return await _context.Results.Include(r => r.Exam).Include(r => r.Student).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Result result)
        {
            await _context.Results.AddAsync(result);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Result result)
        {
            _context.Results.Update(result);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result != null)
            {
                _context.Results.Remove(result);
                await _context.SaveChangesAsync();
            }
        }
    }

}
