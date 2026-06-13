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

        // جلب تاريخ الطالب بالكامل (الأحدث أولاً لتحليل التطور)
        public async Task<IEnumerable<Result>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Results
                .Where(r => r.StudentId == studentId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        // جلب نتائج امتحان معين (مهم لتحليل سهولة/صعوبة الامتحان)
        public async Task<IEnumerable<Result>> GetByExamIdAsync(int examId)
        {
            return await _context.Results
                .Where(r => r.ExamId == examId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Result>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Results
                .Where(r => r.LessonId == lessonId)
                .ToListAsync();
        }

        // 💡 إضافة ذكية: حساب متوسط درجات الطالب في كورس معين
        public async Task<double> GetAverageScoreByStudentIdAsync(int studentId, int courseId)
        {
            // نستخدم AverageAsync ليقوم SQL بحساب المتوسط مباشرة (أداء عالٍ)
            var average = await _context.Results
                .Where(r => r.StudentId == studentId && r.Exam.CourseId == courseId)
                .Select(r => r.Score)
                .DefaultIfEmpty(0) // في حال لم توجد نتائج، يعيد 0 بدلاً من خطأ
                .AverageAsync();

            return average;
        }
    }
}