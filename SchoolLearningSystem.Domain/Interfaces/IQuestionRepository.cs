using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> GetByIdAsync(int id);
        Task<IEnumerable<Question>> GetAllAsync();
        Task AddAsync(Question question);
        Task UpdateAsync(Question question);
        Task DeleteAsync(int id);
        Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(int lessonId);

    }
}
