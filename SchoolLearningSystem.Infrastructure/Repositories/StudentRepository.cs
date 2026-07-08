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

        // جلب الطلاب حسب المرحلة الدراسية
        public async Task<IEnumerable<Student>> GetByGradeLevelAsync(GradeLevel gradeLevel)
        {
            return await _context.Students
                .AsNoTracking()
                .Where(s => s.GradeLevel == gradeLevel && !s.IsDeleted)
                .ToListAsync();
        }

        // جلب الطالب مع سجل تقدمه (مهم جداً لمحرك الـ SRS)
        // 💡 بدون AsNoTracking عن قصد: هذي الدالة غالباً تُستخدم قبل تعديل سجلات التقدم
        public async Task<Student?> GetStudentWithProgressAsync(int studentId)
        {
            return await _context.Students
                .Include(s => s.Progresses)
                .FirstOrDefaultAsync(s => s.Id == studentId && !s.IsDeleted);
        }
    }
}