using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ITeacherRepository
    {
        Task<Teacher> GetByIdAsync(int id);
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task AddAsync(Teacher teacher);
        Task UpdateAsync(Teacher teacher);
        Task DeleteAsync(int id);
    }
}
