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
        public CurriculumRepository(AppDbContext context) : base(context)
        {
        }

        // تنفيذ البحث باستخدام الـ GradeLevel مع تحسين الأداء بـ AsNoTracking
        public async Task<Curriculum?> GetByGradeLevelAsync(GradeLevel gradeLevel)
        {
            return await _context.Curriculums
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.GradeLevel == gradeLevel);
        }
    }
}