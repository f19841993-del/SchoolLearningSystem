using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class ExerciseRepository : GenericRepository<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(AppDbContext context) : base(context)
        {
        }

        // جلب جميع التمارين المرتبطة بدرس معين مع تحسين الأداء
        public async Task<IEnumerable<Exercise>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Exercises
                .AsNoTracking() // 👈 الأداء الأفضل للقراءة فقط
                .Where(e => e.LessonId == lessonId)
                .ToListAsync();
        }

        // جلب التمارين حسب مستوى الصعوبة
        public async Task<IEnumerable<Exercise>> GetByDifficultyAsync(DifficultyLevel difficulty)
        {
            return await _context.Exercises
                .AsNoTracking() // 👈 الأداء الأفضل للقراءة فقط
                .Where(e => e.Difficulty == difficulty)
                .ToListAsync();
        }
    }
}