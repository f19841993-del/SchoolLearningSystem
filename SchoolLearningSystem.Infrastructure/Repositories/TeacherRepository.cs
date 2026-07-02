using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(AppDbContext context) : base(context)
        {
        }

        // 🔹 تنفيذ الاستعلام المخصص لجلب الكورسات الخاصة بمعلم معين
        public async Task<IEnumerable<Course>> GetCoursesByTeacherIdAsync(int teacherId)
        {
            // نستخدم AsNoTracking لتحسين الأداء لأننا نجلب بيانات للعرض فقط
            return await _context.Courses
                .AsNoTracking()
                .Where(c => c.TeacherId == teacherId)
                .ToListAsync();
        }
    }
}