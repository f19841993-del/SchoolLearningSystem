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
        // نمرر الـ AppDbContext للـ Base Class (الـ GenericRepository)
        public ExamRepository(AppDbContext context) : base(context)
        {
        }

        // استعلام لجلب كل امتحانات كورس معين
        public async Task<IEnumerable<Exam>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Exams
                .AsNoTracking() // تحسين أداء للقراءة فقط
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }

        // جلب الامتحانات حسب مستوى الصعوبة لدعم الـ AI Engine
        public async Task<IEnumerable<Exam>> GetExamsByDifficultyAsync(DifficultyLevel difficulty)
        {
            return await _context.Exams
                .AsNoTracking()
                .Where(e => e.Difficulty == difficulty)
                .ToListAsync();
        }

        // جلب عدد الامتحانات لكل درس (مفيدة في لوحات تحكم المعلمين)
        public async Task<int> GetTotalExamsByLessonIdAsync(int lessonId)
        {
            return await _context.Exams
                .CountAsync(e => e.LessonId == lessonId);
        }
    }
}