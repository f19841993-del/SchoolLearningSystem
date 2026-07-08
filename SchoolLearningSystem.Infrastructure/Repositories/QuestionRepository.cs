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

        public async Task<IEnumerable<Question>> GetByExamIdAsync(int examId)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(q => q.ExamId == examId && !q.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(int lessonId)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(q => q.LessonId == lessonId && !q.IsDeleted)
                .ToListAsync();
        }

        // 🆕 جلب الأسئلة حسب مستوى الصعوبة
        public async Task<IEnumerable<Question>> GetByDifficultyAsync(DifficultyLevel difficultyLevel)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(q => q.DifficultyLevel == difficultyLevel && !q.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> CountByExamIdAsync(int examId)
        {
            return await _context.Questions
                .CountAsync(q => q.ExamId == examId && !q.IsDeleted);
        }

        public async Task<int> CountByDifficultyAsync(DifficultyLevel difficultyLevel)
        {
            return await _context.Questions
                .CountAsync(q => q.DifficultyLevel == difficultyLevel && !q.IsDeleted);
        }

        public async Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId)
        {
            return await _context.Questions
                .CountAsync(q => q.LessonId == lessonId && !q.IsDeleted);
        }
    }
}