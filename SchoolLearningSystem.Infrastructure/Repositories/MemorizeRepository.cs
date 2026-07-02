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

        // 1. جلب الجلسة الحالية (غير المكتملة) للطالب
        public async Task<MemorizeSession?> GetActiveSessionByStudentIdAsync(int studentId)
        {
            return await _context.MemorizeSessions
                .AsNoTracking()
                // من الأفضل دائماً جلب الـ AnswerDetails إذا كنت ستستخدمها فوراً في الـ Service
                .Include(s => s.AnswerDetails)
                .FirstOrDefaultAsync(s => s.StudentId == studentId && !s.IsCompleted);
        }

        // 2. جلب سجل جلسات الطالب (التاريخ) - مرتبة من الأحدث للأقدم
        public async Task<IEnumerable<MemorizeSession>> GetSessionHistoryByStudentIdAsync(int studentId)
        {
            return await _context.MemorizeSessions
                .AsNoTracking()
                .Where(s => s.StudentId == studentId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }
    }
}