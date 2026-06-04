using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using WebApiTemplate.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.CourseStudents).ThenInclude(cs => cs.Course)
                .Include(s => s.Results).ThenInclude(r => r.Exam)
                .Include(s => s.MemorizeSessions).ThenInclude(ms => ms.Lesson)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.CourseStudents).ThenInclude(cs => cs.Course)
                .Include(s => s.Results).ThenInclude(r => r.Exam)
                .Include(s => s.MemorizeSessions).ThenInclude(ms => ms.Lesson)
                .ToListAsync();
        }

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }
    }
}
