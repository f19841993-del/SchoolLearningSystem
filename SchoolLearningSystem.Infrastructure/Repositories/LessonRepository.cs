using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(AppDbContext context) : base(context)
        {
        }

        // 🔹 جلب كل دروس كورس معيّن، مرتبة حسب التسلسل (Order)
        public async Task<IEnumerable<Lesson>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Lessons
                .AsNoTracking() // 🚀 تحسين أداء لعمليات القراءة فقط
                .Where(l => l.CourseId == courseId && !l.IsDeleted) // استبعاد الدروس المحذوفة منطقياً
                .OrderBy(l => l.Order) // ضروري لعرض الدروس بترتيبها الصحيح
                .ToListAsync();
        }

        // 🔹 جلب الدرس التالي بالتسلسل بعد درس معيّن (يفيد نظام التعلم التكيفي / AI)
        public async Task<Lesson?> GetNextLessonAsync(int courseId, int currentLessonOrder)
        {
            return await _context.Lessons
                .AsNoTracking()
                .Where(l => l.CourseId == courseId
                            && l.Order > currentLessonOrder
                            && !l.IsDeleted) // استبعاد الدروس المحذوفة منطقياً
                .OrderBy(l => l.Order)
                .FirstOrDefaultAsync();
        }

        // 🔹 جلب الدرس المرتبط بتمرين معيّن (Exercise)
        public async Task<Lesson?> GetByExerciseIdAsync(int exerciseId)
        {
            return await _context.Lessons
                .AsNoTracking()
                .Where(l => !l.IsDeleted && l.Exercises.Any(e => e.Id == exerciseId))
                .FirstOrDefaultAsync();
        }
    }
}