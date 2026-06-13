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

        // 1. جلب الدروس حسب الكورس
        public async Task<IEnumerable<Lesson>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Lessons
                .Where(l => l.CourseId == courseId)
                .ToListAsync();
        }

        // 2. جلب الأسئلة الخاصة بدرس معين (نستخدم DbSet الأسئلة)
        public async Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(int lessonId)
        {
            return await _context.Questions
                .Where(q => q.LessonId == lessonId)
                .ToListAsync();
        }

        // 3. إحصائيات: عدد الأسئلة (استخدام CountAsync بدلاً من جلب كل القائمة لزيادة السرعة)
        public async Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId)
        {
            return await _context.Questions
                .CountAsync(q => q.LessonId == lessonId);
        }

        // 4. إحصائيات: عدد الامتحانات
        public async Task<int> GetTotalExamsByLessonIdAsync(int lessonId)
        {
            // نفترض هنا وجود علاقة بين الامتحان والدرس
            return await _context.Exams
                .CountAsync(e => e.LessonId == lessonId);
        }
    }
}