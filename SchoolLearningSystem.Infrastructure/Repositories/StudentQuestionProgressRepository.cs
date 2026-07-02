using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class StudentQuestionProgressRepository : IStudentQuestionProgressRepository
    {
        private readonly AppDbContext _context;

        public StudentQuestionProgressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StudentQuestionProgress?> GetByStudentAndQuestionAsync(int studentId, int questionId)
        {
            // نستخدم FindAsync للمفاتيح المركبة
            return await _context.StudentQuestionProgresses.FindAsync(studentId, questionId);
        }

        public async Task<IEnumerable<StudentQuestionProgress>> GetDueQuestionsAsync(int studentId, DateTime currentDate)
        {
            return await _context.StudentQuestionProgresses
                .AsNoTracking() // 🚀 تحسين الأداء للقراءة
                .Include(p => p.Question)
                .Where(p => p.StudentId == studentId && p.NextReviewDate <= currentDate)
                .OrderBy(p => p.NextReviewDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentQuestionProgress>> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentQuestionProgresses
                .AsNoTracking() // 🚀 تحسين الأداء
                .Where(s => s.StudentId == studentId)
                .ToListAsync();
        }

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