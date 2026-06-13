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

        // جلب جميع التمارين المرتبطة بدرس معين
        public async Task<IEnumerable<Exercise>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Exercises
                .Where(e => e.LessonId == lessonId)
                .ToListAsync();
        }

        // 💡 خدمة الذكاء الاصطناعي: جلب التمارين حسب مستوى الصعوبة
        public async Task<IEnumerable<Exercise>> GetByDifficultyAsync(DifficultyLevel difficulty)
        {
            // تأكد أن اسم الخاصية في كلاس Exercise هو "Difficulty"
            // إذا كانت مسمى آخر (مثل DifficultyLevel)، قم بتغييره هنا
            return await _context.Exercises
                .Where(e => e.Difficulty == difficulty)
                .ToListAsync();
        }
    }
}