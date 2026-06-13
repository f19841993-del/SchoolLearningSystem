using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class StudentQuestionProgressRepository : IStudentQuestionProgressRepository
    {
        private readonly AppDbContext _context;

        public StudentQuestionProgressRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. جلب سجل التقدم الخاص بسؤال معين لطالب معين
        // نستخدم FindAsync مع المفاتيح المركبة، وهي الطريقة الأسرع والأصح
        public async Task<StudentQuestionProgress?> GetByStudentAndQuestionAsync(int studentId, int questionId)
        {
            return await _context.StudentQuestionProgresses.FindAsync(studentId, questionId);
        }

        // 2. "قلب الـ SRS": الأسئلة التي حان موعد مراجعتها للطالب
        public async Task<IEnumerable<StudentQuestionProgress>> GetDueQuestionsAsync(int studentId)
        {
            var now = DateTime.UtcNow;
            return await _context.StudentQuestionProgresses
                .Include(s => s.Question) // مهم جداً لجلب محتوى السؤال مع التقدم
                .Where(s => s.StudentId == studentId && s.NextReviewDate <= now)
                .ToListAsync();
        }

        // 3. جلب كل سجلات التقدم لطالب معين
        public async Task<IEnumerable<StudentQuestionProgress>> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentQuestionProgresses
                .Where(s => s.StudentId == studentId)
                .ToListAsync();
        }

        // العمليات الأساسية (CRUD) الخاصة بـ IStudentQuestionProgressRepository
        public async Task AddAsync(StudentQuestionProgress entity)
        {
            await _context.StudentQuestionProgresses.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentQuestionProgress entity)
        {
            _context.StudentQuestionProgresses.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}