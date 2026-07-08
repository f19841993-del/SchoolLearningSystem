using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
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
    }
}