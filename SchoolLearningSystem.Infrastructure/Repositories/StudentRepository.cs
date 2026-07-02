using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context)
        {
        }

        // 1. جلب الطلاب حسب المرحلة الدراسية مع تحسين الأداء
        public async Task<IEnumerable<Student>> GetByGradeLevelAsync(GradeLevel gradeLevel)
        {
            return await _context.Students
                .AsNoTracking() // 🚀 تحسين الأداء للقراءة
                .Where(s => s.GradeLevel == gradeLevel)
                .ToListAsync();
        }

        // 2. جلب الطالب مع سجل تقدمه (مهم جداً لمحرك الـ SRS)
        public async Task<Student?> GetStudentWithProgressAsync(int studentId)
        {
            // هنا لا نستخدم AsNoTracking لأننا غالباً نجلب الطالب لنعدل على تقدمه
            return await _context.Students
                .Include(s => s.Progresses)
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }
    }
}