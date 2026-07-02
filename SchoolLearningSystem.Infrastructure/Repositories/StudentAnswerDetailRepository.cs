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
                .AsNoTracking() // 🚀 تحسين أداء
                .Where(s => s.StudentId == studentId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        // 2. جلب إجابات كل الطلاب على سؤال معين
        public async Task<IEnumerable<StudentAnswerDetail>> GetByQuestionIdAsync(int questionId)
        {
            return await _context.StudentAnswerDetails
                .AsNoTracking() // 🚀 تحسين أداء
                .Where(s => s.QuestionId == questionId)
                .ToListAsync();
        }

        // 3. جلب آخر N إجابة
        public async Task<IEnumerable<StudentAnswerDetail>> GetRecentAnswersAsync(int studentId, int count)
        {
            return await _context.StudentAnswerDetails
                .AsNoTracking() // 🚀 تحسين أداء
                .Where(s => s.StudentId == studentId)
                .OrderByDescending(s => s.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        // 4. جلب الإجابات الخاطئة لدرس معين (استعلام تحليلي)
        public async Task<IEnumerable<StudentAnswerDetail>> GetIncorrectAnswersByStudentIdAsync(int studentId, int lessonId)
        {
            return await _context.StudentAnswerDetails
                .AsNoTracking() // 🚀 تحسين أداء
                .Include(s => s.Question) // نستخدم Include للوصول لبيانات السؤال
                .Where(s => s.StudentId == studentId
                          && s.Question.LessonId == lessonId
                          && !s.IsCorrect)
                .ToListAsync();
        }
    }
}