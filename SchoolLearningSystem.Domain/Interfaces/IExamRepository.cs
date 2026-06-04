using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IExamRepository
    {
        Task<Exam> GetByIdAsync(int id);
        Task<IEnumerable<Exam>> GetAllAsync();
        Task AddAsync(Exam exam);
        Task UpdateAsync(Exam exam);
        Task DeleteAsync(int id);
        Task<IEnumerable<Exam>> GetByCourseIdAsync(int courseId);

    }
}
