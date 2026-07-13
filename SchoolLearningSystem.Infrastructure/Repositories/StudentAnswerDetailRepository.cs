using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Domain.Models;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class StudentAnswerDetailRepository
        : GenericRepository<StudentAnswerDetail>, IStudentAnswerDetailRepository
    {
        public StudentAnswerDetailRepository(AppDbContext context) : base(context)
        {
        }

        // تاريخ إجابات الطالب بالكامل، الأحدث أولاً
        public async Task<IEnumerable<StudentAnswerDetail>> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentAnswerDetails
                .AsNoTracking()
                .Where(a => a.StudentId == studentId && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // كل إجابات الطلاب على سؤال معيّن (لتحليل صعوبته الفعلية)
        public async Task<IEnumerable<StudentAnswerDetail>> GetByQuestionIdAsync(int questionId)
        {
            return await _context.StudentAnswerDetails
                .AsNoTracking()
                .Where(a => a.QuestionId == questionId && !a.IsDeleted)
                .ToListAsync();
        }

        // آخر N إجابة للطالب (لتتبع التطور اللحظي)
        public async Task<IEnumerable<StudentAnswerDetail>> GetRecentAnswersAsync(int studentId, int count)
        {
            return await _context.StudentAnswerDetails
                .AsNoTracking()
                .Where(a => a.StudentId == studentId && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        // إجابات الطالب الخاطئة ضمن درس معيّن (لإعادة التدريب المستهدف)
        // 💡 نستخدم a.Question.LessonId مباشرة بالفلترة (بدون Include) - EF Core
        // يترجم هذا إلى JOIN بقاعدة البيانات تلقائياً لأنه مجرد شرط تصفية، لا نحتاج
        // تحميل بيانات Question الكاملة بالذاكرة.
        public async Task<IEnumerable<StudentAnswerDetail>> GetIncorrectAnswersByStudentIdAsync(int studentId, int lessonId)
        {
            return await _context.StudentAnswerDetails
                .AsNoTracking()
                .Where(a => a.StudentId == studentId
                            && !a.IsCorrect
                            && a.Question.LessonId == lessonId
                            && !a.IsDeleted)
                .ToListAsync();
        }


        public async Task<IEnumerable<QuestionDifficultyStats>> GetQuestionDifficultyStatsAsync(int? lessonId, int topN)
        {
            var query = _context.StudentAnswerDetails.AsNoTracking().Where(a => !a.IsDeleted);

            if (lessonId.HasValue)
                query = query.Where(a => a.Question.LessonId == lessonId.Value);

            return await query
                .GroupBy(a => new { a.QuestionId, a.Question.Text, a.Question.LessonId, a.Question.Lesson.Title })
                .Select(g => new
                {
                    g.Key.QuestionId,
                    g.Key.Text,
                    g.Key.LessonId,
                    g.Key.Title,
                    TotalAttempts = g.Count(),
                    IncorrectCount = g.Count(a => !a.IsCorrect)
                })
                .OrderByDescending(x => x.TotalAttempts == 0 ? 0 : (double)x.IncorrectCount / x.TotalAttempts)
                .Take(topN)   // 👈 الآن الـ SQL نفسه يجيب topN صف بس، مو كل الأسئلة
                .Select(x => new QuestionDifficultyStats(x.QuestionId, x.Text, x.LessonId, x.Title, x.TotalAttempts, x.IncorrectCount))
                .ToListAsync();
        }
    }
}