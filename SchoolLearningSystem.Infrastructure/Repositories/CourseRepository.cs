using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId)
        {
            return await _context.Courses
                .AsNoTracking()
                .Where(c => c.TeacherId == teacherId)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetByCurriculumIdAsync(int curriculumId)
        {
            return await _context.Courses
                .AsNoTracking()
                .Where(c => c.CurriculumId == curriculumId)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }

        public async Task<Course?> GetWithDetailsAsync(int courseId)
        {
            return await _context.Courses
                .AsNoTracking()
                .Where(c => !c.IsDeleted) // 💡 إضافة مقترحة للتوافق مع بقية النظام
                .Include(c => c.Teacher)
                .Include(c => c.Curriculum)
                .Include(c => c.Lessons)
                .Include(c => c.Exams)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<int> GetEnrolledStudentsCountAsync(int courseId)
        {
            return await _context.CourseStudents
                .AsNoTracking()
                .CountAsync(cs => cs.CourseId == courseId);
        }
    }
}