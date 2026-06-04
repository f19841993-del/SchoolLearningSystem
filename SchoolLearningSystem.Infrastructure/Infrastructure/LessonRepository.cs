using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SchoolLearningSystem.Infrastructure.Infrastructure
{
    public class LessonRepository : ILessonRepository
    {
        private readonly AppDbContext _context;

        public LessonRepository(AppDbContext context)
        {
            _context = context;
        }

        // CRUD الأساسي
        public async Task<IEnumerable<Lesson>> GetAllAsync()
        {
            return await _context.Lessons
                .Include(l => l.Course)
                .Include(l => l.Exams)
                .Include(l => l.Exercises)
                .Include(l => l.Results)
                .Include(l => l.MemorizeSessions)
                .Include(l => l.Questions) // 🔹 إضافة الأسئلة
                .ToListAsync();
        }

        public async Task<Lesson?> GetByIdAsync(int id)
        {
            return await _context.Lessons
                .Include(l => l.Course)
                .Include(l => l.Exams)
                .Include(l => l.Exercises)
                .Include(l => l.Results)
                .Include(l => l.MemorizeSessions)
                .Include(l => l.Questions) // 🔹 إضافة الأسئلة
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddAsync(Lesson lesson)
        {
            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Lesson>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Lessons
                .Include(l => l.Course)
                .Where(l => l.CourseId == courseId)
                .ToListAsync();
        }

        // علاقات جديدة
        public async Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(int lessonId)
        {
            return await _context.Questions
                .Where(q => q.LessonId == lessonId)
                .ToListAsync();
        }

        // إحصائيات
        public async Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId)
        {
            return await _context.Questions
                .CountAsync(q => q.LessonId == lessonId);
        }

        public async Task<int> GetTotalExamsByLessonIdAsync(int lessonId)
        {
            return await _context.Exams
                .CountAsync(e => e.LessonId == lessonId);
        }
    }
}
