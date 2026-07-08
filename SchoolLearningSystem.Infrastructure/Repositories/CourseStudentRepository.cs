using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    // 💡 لا يرث من GenericRepository<T> لأن CourseStudent له مفتاح مركّب (CourseId + StudentId)
    // 💡 منطق الأعمال (التحقق من التكرار، وجود الكورس/الطالب) أصبح مسؤولية CourseStudentService
    // هذا الريبو يقدّم فقط عمليات وصول بيانات بسيطة (Data Access)
    public class CourseStudentRepository : ICourseStudentRepository
    {
        private readonly AppDbContext _context;

        public CourseStudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseStudent>> GetAllAsync()
        {
            return await _context.CourseStudents.AsNoTracking().ToListAsync();
        }

        public async Task<CourseStudent?> GetByIdAsync(int courseId, int studentId)
        {
            return await _context.CourseStudents.FindAsync(courseId, studentId);
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
            var entity = await GetByIdAsync(courseId, studentId);
            if (entity != null)
            {
                _context.CourseStudents.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CourseStudent>> GetByCourseIdAsync(int courseId)
        {
            return await _context.CourseStudents
                .AsNoTracking()
                .Where(cs => cs.CourseId == courseId)
                .Include(cs => cs.Student)
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseStudent>> GetByStudentIdAsync(int studentId)
        {
            return await _context.CourseStudents
                .AsNoTracking()
                .Where(cs => cs.StudentId == studentId)
                .Include(cs => cs.Course)
                .ToListAsync();
        }

        public async Task<int> CountByCourseIdAsync(int courseId)
        {
            return await _context.CourseStudents.CountAsync(cs => cs.CourseId == courseId);
        }

        public async Task<int> CountByStudentIdAsync(int studentId)
        {
            return await _context.CourseStudents.CountAsync(cs => cs.StudentId == studentId);
        }

        public async Task<(IEnumerable<CourseStudent> Items, int TotalCount)> GetPagedByCourseIdAsync(int courseId, int pageNumber, int pageSize)
        {
            var query = _context.CourseStudents
                .AsNoTracking()
                .Include(cs => cs.Student)
                .Where(cs => cs.CourseId == courseId);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(cs => cs.StudentId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IEnumerable<CourseStudent> Items, int TotalCount)> GetPagedByStudentIdAsync(int studentId, int pageNumber, int pageSize)
        {
            var query = _context.CourseStudents
                .AsNoTracking()
                .Include(cs => cs.Course)
                .Where(cs => cs.StudentId == studentId);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(cs => cs.CourseId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}