using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IResultRepository
    {
        Task<Result> GetByIdAsync(int id);
        Task<IEnumerable<Result>> GetAllAsync();
        Task AddAsync(Result result);
        Task UpdateAsync(Result result);
        Task DeleteAsync(int id);
    }
}
