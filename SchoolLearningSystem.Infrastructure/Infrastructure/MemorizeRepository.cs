using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;

namespace SchoolLearningSystem.Infrastructure.Infrastructure
{
    public class MemorizeRepository : IMemorizeRepository
    {
        private readonly AppDbContext _context;

        public MemorizeRepository(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 CRUD الأساسي
        public async Task<IEnumerable<MemorizeSession>> GetAllAsync()
        {
            return await _context.MemorizeSessions
                .Include(m => m.Student)
                .Include(m => m.Lesson)
                .Include(m => m.Exercise)
                .ToListAsync();
        }

        public async Task<MemorizeSession?> GetByIdAsync(int id)
        {
            return await _context.MemorizeSessions
                .Include(m => m.Student)
                .Include(m => m.Lesson)
                .Include(m => m.Exercise)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(MemorizeSession session)
        {
            await _context.MemorizeSessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MemorizeSession session)
        {
            _context.MemorizeSessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var session = await _context.MemorizeSessions.FindAsync(id);
            if (session != null)
            {
                _context.MemorizeSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }

        // 🔹 علاقات إضافية
        public async Task<IEnumerable<MemorizeSession>> GetByStudentIdAsync(int studentId)
        {
            return await _context.MemorizeSessions
                .Include(m => m.Student)
                .Include(m => m.Lesson)
                .Include(m => m.Exercise)
                .Where(m => m.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MemorizeSession>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.MemorizeSessions
                .Include(m => m.Student)
                .Include(m => m.Lesson)
                .Include(m => m.Exercise)
                .Where(m => m.LessonId == lessonId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MemorizeSession>> GetByExerciseIdAsync(int exerciseId)
        {
            return await _context.MemorizeSessions
                .Include(m => m.Student)
                .Include(m => m.Lesson)
                .Include(m => m.Exercise)
                .Where(m => m.ExerciseId == exerciseId)
                .ToListAsync();
        }
    }
}
