using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IQuestionRepository
    {
        // 🔹 CRUD الأساسي
        Task<Question?> GetByIdAsync(int id);
        Task<IEnumerable<Question>> GetAllAsync();
        Task AddAsync(Question question);
        Task UpdateAsync(Question question);
        Task DeleteAsync(Question question);

        // 🔹 علاقات إضافية
        Task<IEnumerable<Question>> GetByExamIdAsync(int examId);
        Task<IEnumerable<Question>> GetByLessonIdAsync(int lessonId);

        // 🔹 إحصائيات إضافية
        Task<int> CountByExamIdAsync(int examId);
        Task<int> CountByDifficultyAsync(string difficultyLevel);
    }
}
