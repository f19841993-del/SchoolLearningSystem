using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Infrastructure.Infrastructure
{
    public class CourseStudentRepository : ICourseStudentRepository
    {
        private readonly AppDbContext _context;

        public CourseStudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseStudent>> GetAllAsync()
        {
            return await _context.CourseStudents
                .Include(cs => cs.Course)   // تحميل العلاقة مع Course
                .Include(cs => cs.Student) // تحميل العلاقة مع Student
                .ToListAsync();
        }

        public async Task<CourseStudent?> GetByIdAsync(int courseId, int studentId)
        {
            return await _context.CourseStudents
                .Include(cs => cs.Course)
                .Include(cs => cs.Student)
                .FirstOrDefaultAsync(cs => cs.CourseId == courseId && cs.StudentId == studentId);
        }


        public async Task AddAsync(CourseStudent courseStudent)
        {
            await _context.CourseStudents.AddAsync(courseStudent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseStudent courseStudent)
        {
            _context.CourseStudents.Update(courseStudent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int courseId, int studentId)
        {
            var relation = await _context.CourseStudents
                .FirstOrDefaultAsync(cs => cs.CourseId == courseId && cs.StudentId == studentId);

            if (relation != null)
            {
                _context.CourseStudents.Remove(relation);
                await _context.SaveChangesAsync();
            }
        }


        // دوال إضافية حسب الحاجة
        public async Task<IEnumerable<CourseStudent>> GetByCourseIdAsync(int courseId)
        {
            return await _context.CourseStudents
                .Where(cs => cs.CourseId == courseId)
                .Include(cs => cs.Student)
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseStudent>> GetByStudentIdAsync(int studentId)
        {
            return await _context.CourseStudents
                .Where(cs => cs.StudentId == studentId)
                .Include(cs => cs.Course)
                .ToListAsync();
        }

      
    }
}
