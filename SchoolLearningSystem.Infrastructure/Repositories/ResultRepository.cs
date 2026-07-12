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

        // جلب تاريخ الطالب بالكامل، الأحدث أولاً
        public async Task<IEnumerable<Result>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Results
                .AsNoTracking()
                .Where(r => r.StudentId == studentId && !r.IsDeleted)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Result>> GetByExamIdAsync(int examId)
        {
            return await _context.Results
                .AsNoTracking()
                .Where(r => r.ExamId == examId && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Result>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Results
                .AsNoTracking()
                .Where(r => r.LessonId == lessonId && !r.IsDeleted)
                .ToListAsync();
        }
        // يحسب عدد الدروس المميزة اللي عند الطالب نتيجة (Result) عليها ضمن كورس معيّن
        // نستخدم r.Lesson.CourseId مباشرة بالفلترة (JOIN تلقائي) بدون Include كامل
        public async Task<int> CountDistinctCompletedLessonsAsync(int studentId, int courseId)
        {
            return await _context.Results
                .AsNoTracking()
                .Where(r => r.StudentId == studentId
                            && r.LessonId != null
                            && r.Lesson!.CourseId == courseId
                            && !r.IsDeleted)
                .Select(r => r.LessonId)
                .Distinct()
                .CountAsync();
        }

        // متوسط كل درجات الطالب (بكل الكورسات/الدروس/الامتحانات مجتمعة)
        public async Task<double> GetAverageScoreByStudentIdAsync(int studentId)
        {
            return await _context.Results
                .AsNoTracking()
                .Where(r => r.StudentId == studentId && !r.IsDeleted)
                .Select(r => r.Score)
                .DefaultIfEmpty(0)
                .AverageAsync();
        }

        // متوسط درجات كل الطلاب بدرس معيّن (يفيد لمعرفة مدى صعوبة الدرس)
        public async Task<double> GetAverageScoreByLessonIdAsync(int lessonId)
        {
            return await _context.Results
                .AsNoTracking()
                .Where(r => r.LessonId == lessonId && !r.IsDeleted)
                .Select(r => r.Score)
                .DefaultIfEmpty(0)
                .AverageAsync();
        }

        // متوسط درجات كل الطلاب بامتحان معيّن (يفيد لمعرفة مدى صعوبة الامتحان)
        public async Task<double> GetAverageScoreByExamIdAsync(int examId)
        {
            return await _context.Results
                .AsNoTracking()
                .Where(r => r.ExamId == examId && !r.IsDeleted)
                .Select(r => r.Score)
                .DefaultIfEmpty(0)
                .AverageAsync();
        }
    }
}