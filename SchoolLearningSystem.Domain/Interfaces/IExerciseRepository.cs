using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IExerciseRepository
    {
        Task<IEnumerable<Exercise>> GetAllAsync();
        Task<Exercise?> GetByIdAsync(int id);
        Task AddAsync(Exercise exercise);
        Task UpdateAsync(Exercise exercise);
        Task DeleteAsync(int id);

        // دوال إضافية حسب الحاجة
        Task<IEnumerable<Exercise>> GetByLessonIdAsync(int lessonId);
    }
}
