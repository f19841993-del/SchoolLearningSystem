using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class MemorizeRepository : GenericRepository<MemorizeSession>, IMemorizeRepository
    {
        public MemorizeRepository(AppDbContext context) : base(context)
        {
        }

        // جلب جلسات طالب معين (يستخدم للـ AI لتحليل نمط دراسة الطالب)
        public async Task<IEnumerable<MemorizeSession>> GetByStudentIdAsync(int studentId)
        {
            return await _context.MemorizeSessions
                .Where(s => s.StudentId == studentId)
                .OrderByDescending(s => s.CreatedAt) // جلب الأحدث أولاً
                .ToListAsync();
        }

        // جلب جلسات درس معين (مفيد لتحليل صعوبة الدرس عالمياً)
        public async Task<IEnumerable<MemorizeSession>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.MemorizeSessions
                .Where(s => s.LessonId == lessonId)
                .ToListAsync();
        }

        // جلب جلسات تمرين معين
        public async Task<IEnumerable<MemorizeSession>> GetByExerciseIdAsync(int exerciseId)
        {
            return await _context.MemorizeSessions
                .Where(s => s.ExerciseId == exerciseId)
                .ToListAsync();
        }
    }
}