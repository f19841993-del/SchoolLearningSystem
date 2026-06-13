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
        // نستلم الـ DbContext ونمرره للـ GenericRepository
        public ExamRepository(AppDbContext context) : base(context)
        {
        }

        // استعلام لجلب كل امتحانات كورس معين
        public async Task<IEnumerable<Exam>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Exams
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }

        // 💡 إضافة ذكية للذكاء الاصطناعي:
        // جلب الامتحانات حسب مستوى الصعوبة
        public async Task<IEnumerable<Exam>> GetExamsByDifficultyAsync(DifficultyLevel difficulty)
        {
            return await _context.Exams
                .Where(e => e.Difficulty == difficulty)
                .ToListAsync();
        }
    }
}