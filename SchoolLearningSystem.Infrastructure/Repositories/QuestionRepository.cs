using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }

        // جلب الأسئلة المرتبطة باختبار معين (للقراءة فقط -> AsNoTracking)
        public async Task<IEnumerable<Question>> GetByExamIdAsync(int examId)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(q => q.ExamId == examId)
                .ToListAsync();
        }

        // جلب الأسئلة المرتبطة بدرس معين (للقراءة فقط -> AsNoTracking)
        public async Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(int lessonId)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(q => q.LessonId == lessonId)
                .ToListAsync();
        }

        // إحصائيات: عدد الأسئلة في امتحان محدد
        public async Task<int> CountByExamIdAsync(int examId)
        {
            return await _context.Questions
                .CountAsync(q => q.ExamId == examId);
        }

        // إحصائيات: عدد الأسئلة حسب الصعوبة
        public async Task<int> CountByDifficultyAsync(DifficultyLevel difficultyLevel)
        {
            return await _context.Questions
                .CountAsync(q => q.DifficultyLevel == difficultyLevel);
        }

        // إحصائيات: عدد الأسئلة في درس محدد
        public async Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId)
        {
            return await _context.Questions
                .CountAsync(q => q.LessonId == lessonId);
        }
    }
}