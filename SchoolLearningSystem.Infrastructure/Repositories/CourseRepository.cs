using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        // الـ _context معرف في الـ GenericRepository كـ protected، لذا يمكننا الوصول إليه مباشرة
        public CourseRepository(AppDbContext context) : base(context)
        {
        }

        // 1. تنفيذ الاستعلام الخاص بالمنهج (للقراءة فقط -> AsNoTracking)
        public async Task<IEnumerable<Course>> GetByCurriculumIdAsync(int curriculumId)
        {
            return await _context.Courses
                .AsNoTracking()
                .Where(c => c.CurriculumId == curriculumId)
                .ToListAsync();
        }

        // 2. جلب الطلاب المسجلين (للقراءة فقط -> AsNoTracking)
        public async Task<IEnumerable<Student>> GetStudentsByCourseIdAsync(int courseId)
        {
            return await _context.Students
                .AsNoTracking()
                .Where(s => s.CourseStudents.Any(cs => cs.CourseId == courseId))
                .ToListAsync();
        }

        // 3. جلب الدروس (للقراءة فقط -> AsNoTracking)
        public async Task<IEnumerable<Lesson>> GetLessonsByCourseIdAsync(int courseId)
        {
            return await _context.Lessons
                .AsNoTracking()
                .Where(l => l.CourseId == courseId)
                .ToListAsync();
        }

        // 4. جلب الامتحانات (للقراءة فقط -> AsNoTracking)
        public async Task<IEnumerable<Exam>> GetExamsByCourseIdAsync(int courseId)
        {
            return await _context.Exams
                .AsNoTracking()
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }

        // 🔹 عمليات الكتابة والتعديل (تبقى بدون AsNoTracking لأننا نحتاج تتبعها)
        public async Task EnrollStudentAsync(int courseId, int studentId)
        {
            var enrollment = new CourseStudent { CourseId = courseId, StudentId = studentId };
            await _context.CourseStudents.AddAsync(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveStudentAsync(int courseId, int studentId)
        {
            var enrollment = await _context.CourseStudents
                .FirstOrDefaultAsync(cs => cs.CourseId == courseId && cs.StudentId == studentId);

            if (enrollment != null)
            {
                _context.CourseStudents.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
        }
    }
}