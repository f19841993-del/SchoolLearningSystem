using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {
        public ExamRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Exam>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Exams
                .AsNoTracking()
                .Where(e => e.CourseId == courseId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetExamsByDifficultyAsync(DifficultyLevel difficulty)
        {
            return await _context.Exams
                .AsNoTracking()
                .Where(e => e.Difficulty == difficulty && !e.IsDeleted)
                .ToListAsync();
        }

        // 🆕 جلب كل امتحانات درس معيّن
        public async Task<IEnumerable<Exam>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Exams
                .AsNoTracking()
                .Where(e => e.LessonId == lessonId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> GetTotalExamsByLessonIdAsync(int lessonId)
        {
            return await _context.Exams
                .CountAsync(e => e.LessonId == lessonId && !e.IsDeleted);
        }
    }
}