using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class StudentAnswerDetailRepository : GenericRepository<StudentAnswerDetail>, IStudentAnswerDetailRepository
    {
        public StudentAnswerDetailRepository(AppDbContext context) : base(context)
        {
        }

        // 1. جلب تاريخ إجابات الطالب
        public async Task<IEnumerable<StudentAnswerDetail>> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentAnswerDetails
                .Where(s => s.StudentId == studentId)
                .OrderByDescending(s => s.Timestamp) // الأحدث أولاً
                .ToListAsync();
        }

        // 2. جلب إجابات كل الطلاب على سؤال معين (لتحليل سهولة/صعوبة السؤال)
        public async Task<IEnumerable<StudentAnswerDetail>> GetByQuestionIdAsync(int questionId)
        {
            return await _context.StudentAnswerDetails
                .Where(s => s.QuestionId == questionId)
                .ToListAsync();
        }

        // 3. جلب آخر N إجابة (لتحديث الـ Dashboard)
        public async Task<IEnumerable<StudentAnswerDetail>> GetRecentAnswersAsync(int studentId, int count)
        {
            return await _context.StudentAnswerDetails
                .Where(s => s.StudentId == studentId)
                .OrderByDescending(s => s.Timestamp)
                .Take(count) // جلب العدد المطلوب فقط
                .ToListAsync();
        }

        // 4. استعلام ذكي: جلب الإجابات الخاطئة لدرس معين (أساس جلسة المراجعة)
        public async Task<IEnumerable<StudentAnswerDetail>> GetIncorrectAnswersByStudentIdAsync(int studentId, int lessonId)
        {
            // نحتاج ربط (Include) كلاس Question للوصول إلى LessonId
            return await _context.StudentAnswerDetails
                .Include(s => s.Question)
                .Where(s => s.StudentId == studentId
                            && s.Question.LessonId == lessonId
                            && !s.IsCorrect)
                .ToListAsync();
        }
    }
}