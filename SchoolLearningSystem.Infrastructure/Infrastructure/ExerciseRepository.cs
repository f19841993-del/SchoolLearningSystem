using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly AppDbContext _context;

        public ExerciseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return await _context.Exercises
                .Include(e => e.Lesson)                // تحميل العلاقة مع Lesson
                .Include(e => e.MemorizeSessions)      // تحميل العلاقة مع MemorizeSessions
                .ToListAsync();
        }

        public async Task<Exercise?> GetByIdAsync(int id)
        {
            return await _context.Exercises
                .Include(e => e.Lesson)
                .Include(e => e.MemorizeSessions)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Exercise exercise)
        {
            await _context.Exercises.AddAsync(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Exercise exercise)
        {
            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise != null)
            {
                _context.Exercises.Remove(exercise);
                await _context.SaveChangesAsync();
            }
        }

        // مثال لدوال إضافية حسب الحاجة
        public async Task<IEnumerable<Exercise>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Exercises
                .Where(e => e.LessonId == lessonId)
                .Include(e => e.MemorizeSessions)
                .ToListAsync();
        }
    }
}
