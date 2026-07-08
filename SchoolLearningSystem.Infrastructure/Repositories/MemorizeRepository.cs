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

        // جلب الجلسة النشطة (لم تُكمل بعد + أُنشئت اليوم بالضبط)
        // 💡 بدون AsNoTracking عن قصد: غالباً نجلب هذي الجلسة لنعدّل عليها مباشرة
        // (تسجيل إجابة جديدة، تحديث SuccessRate، إلخ)
        public async Task<MemorizeSession?> GetActiveSessionByStudentIdAsync(int studentId)
        {
            var today = DateTime.UtcNow.Date;

            return await _context.MemorizeSessions
                .Where(m => m.StudentId == studentId
                            && !m.IsCompleted
                            && m.CreatedAt.Date == today
                            && !m.IsDeleted)
                .FirstOrDefaultAsync();
        }

        // سجل جلسات الطالب بالكامل، الأحدث أولاً (للقراءة فقط)
        public async Task<IEnumerable<MemorizeSession>> GetSessionHistoryByStudentIdAsync(int studentId)
        {
            return await _context.MemorizeSessions
                .AsNoTracking()
                .Where(m => m.StudentId == studentId && !m.IsDeleted)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        // 🆕 جلب جلسة معيّنة مع كل إجاباتها التفصيلية (للمراجعة الكاملة، للقراءة فقط)
        public async Task<MemorizeSession?> GetSessionWithAnswersAsync(int sessionId)
        {
            return await _context.MemorizeSessions
                .AsNoTracking()
                .Include(m => m.AnswerDetails)
                .FirstOrDefaultAsync(m => m.Id == sessionId && !m.IsDeleted);
        }
    }
}