using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class ResultRepository : GenericRepository<Result>, IResultRepository
    {
        public ResultRepository(AppDbContext context) : base(context)
        {
        }

        // جلب تاريخ الطالب بالكامل (للقراءة فقط -> AsNoTracking)
        public async Task<IEnumerable<Result>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Results
                .AsNoTracking()
                .Where(r => r.StudentId == studentId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        // جلب نتائج امتحان معين
        public async Task<IEnumerable<Result>> GetByExamIdAsync(int examId)
        {
            return await _context.Results
                .AsNoTracking()
                .Where(r => r.ExamId == examId)
                .ToListAsync();
        }

        // جلب نتائج درس معين
        public async Task<IEnumerable<Result>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Results
                .AsNoTracking()
                .Where(r => r.LessonId == lessonId)
                .ToListAsync();
        }

        // حساب متوسط درجات الطالب في كورس معين (عملية إحصائية)
        public async Task<double> GetAverageScoreByStudentIdAsync(int studentId, int courseId)
        {
            // العملية تتم في SQL، نستخدم AsNoTracking لأنها عملية حسابية للقراءة فقط
            return await _context.Results
                .AsNoTracking()
                .Where(r => r.StudentId == studentId && r.Exam.CourseId == courseId)
                .Select(r => r.Score)
                .DefaultIfEmpty(0)
                .AverageAsync();
        }
    }
}