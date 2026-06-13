using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class CurriculumRepository : GenericRepository<Curriculum>, ICurriculumRepository
    {
        // نستخدم الـ DbContext عبر الـ GenericRepository
        public CurriculumRepository(AppDbContext context) : base(context)
        {
        }

        // تنفيذ البحث باستخدام الـ GradeLevel (الـ Enum)
        public async Task<Curriculum?> GetByGradeLevelAsync(GradeLevel gradeLevel)
        {
            // نستخدم FirstOrDefaultAsync لجلب المنهج الوحيد المرتبط بهذه المرحلة
            return await _context.Curriculums
                .FirstOrDefaultAsync(c => c.GradeLevel == gradeLevel);
        }
    }
}