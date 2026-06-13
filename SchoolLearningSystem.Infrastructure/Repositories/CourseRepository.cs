using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        // الـ _context معرف في الـ GenericRepository كـ protected، لذا يمكننا الوصول إليه مباشرة
        public CourseRepository(AppDbContext context) : base(context)
        {
        }

        // 1. تنفيذ الاستعلام الخاص (الذي أضفته أنت)
        public async Task<IEnumerable<Course>> GetByCurriculumIdAsync(int curriculumId)
        {
            return await _context.Courses
                .Where(c => c.CurriculumId == curriculumId)
                .ToListAsync();
        }

        // 2. تنفيذ باقي الدوال الخاصة التي تتطلبها ICourseRepository
        public async Task<IEnumerable<Student>> GetStudentsByCourseIdAsync(int courseId)
        {
            // استعلام لجلب الطلاب المسجلين في كورس معين (بافتراض وجود علاقة في الكيانات)
            return await _context.Students
                .Where(s => s.CourseStudents.Any(cs => cs.CourseId == courseId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Lesson>> GetLessonsByCourseIdAsync(int courseId)
        {
            return await _context.Lessons
                .Where(l => l.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetExamsByCourseIdAsync(int courseId)
        {
            return await _context.Exams
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }

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