using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;

public class QuestionRepository : IQuestionRepository
{
    private readonly AppDbContext _context;

    public QuestionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Question?> GetByIdAsync(int id)
    {
        return await _context.Questions
            .Include(q => q.Exam)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<IEnumerable<Question>> GetAllAsync()
    {
        return await _context.Questions
            .Include(q => q.Exam)
            .ToListAsync();
    }

    public async Task AddAsync(Question question)
    {
        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Question question)
    {
        _context.Questions.Update(question);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Question question)
    {
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Question>> GetByExamIdAsync(int examId)
    {
        return await _context.Questions
            .Include(q => q.Exam)
            .Where(q => q.ExamId == examId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByLessonIdAsync(int lessonId)
    {
        return await _context.Questions
            .Include(q => q.Exam)
            .Where(q => q.Exam.LessonId == lessonId) // لازم يكون عندك LessonId بالـ Exam
            .ToListAsync();
    }

    public async Task<int> CountByExamIdAsync(int examId)
    {
        return await _context.Questions.CountAsync(q => q.ExamId == examId);
    }

    public async Task<int> CountByDifficultyAsync(string difficultyLevel)
    {
        return await _context.Questions.CountAsync(q => q.DifficultyLevel.ToString() == difficultyLevel);
    }
}
