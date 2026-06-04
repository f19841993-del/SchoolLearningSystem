using SchoolLearningSystem.Domain.Entities;

namespace WebApiTemplate.Domain.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> GetByIdAsync(int id);
        Task<IEnumerable<Student>> GetAllAsync();
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(int id);
    }
}
