using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class CourseStudentRepository : ICourseStudentRepository
    {
        private readonly AppDbContext _context;

        public CourseStudentRepository(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 للقراءة فقط -> AsNoTracking
        public async Task<IEnumerable<CourseStudent>> GetAllAsync()
        {
            return await _context.CourseStudents.AsNoTracking().ToListAsync();
        }

        public async Task<CourseStudent?> GetByIdAsync(int courseId, int studentId)
        {
            // استخدام FindAsync مع المفاتيح المركبة هو الطريقة الاحترافية للبحث
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

        // 🔹 للقراءة فقط -> AsNoTracking
        public async Task<IEnumerable<CourseStudent>> GetByCourseIdAsync(int courseId)
        {
            return await _context.CourseStudents
                .AsNoTracking()
                .Where(cs => cs.CourseId == courseId)
                .Include(cs => cs.Student) // لجلب بيانات الطالب مع الاشتراك
                .ToListAsync();
        }

        // 🔹 للقراءة فقط -> AsNoTracking
        public async Task<IEnumerable<CourseStudent>> GetByStudentIdAsync(int studentId)
        {
            return await _context.CourseStudents
                .AsNoTracking()
                .Where(cs => cs.StudentId == studentId)
                .Include(cs => cs.Course) // لجلب بيانات الكورس مع الاشتراك
                .ToListAsync();
        }

        public async Task<int> CountByCourseIdAsync(int courseId)
        {
            // EF Core سيحول هذا الكود إلى SELECT COUNT(*) FROM ... وهو أسرع بكثير
            return await _context.CourseStudents.CountAsync(cs => cs.CourseId == courseId);
        }

        public async Task<int> CountByStudentIdAsync(int studentId)
        {
            return await _context.CourseStudents.CountAsync(cs => cs.StudentId == studentId);
        }

        // 🔹 للقراءة فقط -> AsNoTracking
        public async Task<(IEnumerable<CourseStudent> Items, int TotalCount)> GetPagedByCourseIdAsync(int courseId, int pageNumber, int pageSize)
        {
            var query = _context.CourseStudents
                .AsNoTracking()
                .Include(cs => cs.Student) // 👈 مهم جداً لكي لا يكون الطالب null في الـ DTO
                .Where(cs => cs.CourseId == courseId); // الفلترة أولاً

            var totalCount = await query.CountAsync(); // الحساب في DB
            var items = await query
                .Skip((pageNumber - 1) * pageSize) // الترقيم في DB
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // 🔹 للقراءة فقط -> AsNoTracking
        public async Task<(IEnumerable<CourseStudent> Items, int TotalCount)> GetPagedByStudentIdAsync(int studentId, int pageNumber, int pageSize)
        {
            var query = _context.CourseStudents
                .AsNoTracking()
                .Include(cs => cs.Course) // 👈 مهم جداً لكي لا يكون الكورس null في الـ DTO
                .Where(cs => cs.StudentId == studentId); // الفلترة أولاً

            var totalCount = await query.CountAsync(); // الحساب في DB
            var items = await query
                .Skip((pageNumber - 1) * pageSize) // الترقيم في DB
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}