using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    // 💡 لا يرث من GenericRepository<T> لأن StudentQuestionProgress له مفتاح مركّب
    // (StudentId + QuestionId)، ولا يحتوي BaseEntity (لا حاجة لـ Soft Delete هنا -
    // هو سجل إحصائي/حالة تقدم، لا "محتوى" يُحذف منطقياً).
    public class StudentQuestionProgressRepository : IStudentQuestionProgressRepository
    {
        private readonly AppDbContext _context;

        public StudentQuestionProgressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StudentQuestionProgress?> GetByStudentAndQuestionAsync(int studentId, int questionId)
        {
            return await _context.StudentQuestionProgresses
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.QuestionId == questionId);
        }

        // 🧠 "قلب الـ SRS": الأسئلة المستحقة المراجعة الآن لهذا الطالب
        // (NextReviewDate <= الآن) - هذا الاستعلام هو ما يبني جلسة المراجعة اليومية
        public async Task<IEnumerable<StudentQuestionProgress>> GetDueQuestionsAsync(int studentId, DateTime currentDate)
        {
            return await _context.StudentQuestionProgresses
                .AsNoTracking()
                .Include(p => p.Question) // نحتاج بيانات السؤال نفسه لعرضه بالجلسة
                .Where(p => p.StudentId == studentId && p.NextReviewDate <= currentDate)
                .OrderBy(p => p.NextReviewDate) // الأكثر استحقاقاً (تأخيراً) أولاً
                .ToListAsync();
        }

        // نفس فكرة GetDueQuestionsAsync، بس الفلترة على قائمة أسئلة محددة مسبقاً بدل NextReviewDate
        public async Task<IEnumerable<StudentQuestionProgress>> GetByStudentAndQuestionIdsAsync(int studentId, IEnumerable<int> questionIds)
        {
            return await _context.StudentQuestionProgresses
                .AsNoTracking()
                .Include(p => p.Question) // نفس سبب GetDueQuestionsAsync: نحتاج QuestionText بالـ DTO
                .Where(p => p.StudentId == studentId && questionIds.Contains(p.QuestionId))
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentQuestionProgress>> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentQuestionProgresses
                .AsNoTracking()
                .Where(p => p.StudentId == studentId)
                .ToListAsync();
        }

        public async Task AddAsync(StudentQuestionProgress progress)
        {
            await _context.StudentQuestionProgresses.AddAsync(progress);
        }

        public Task UpdateAsync(StudentQuestionProgress progress)
        {
            _context.StudentQuestionProgresses.Update(progress);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}