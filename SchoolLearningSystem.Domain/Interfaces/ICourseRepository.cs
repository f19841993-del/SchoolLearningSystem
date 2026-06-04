using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
        Task<IEnumerable<Course>> GetByCurriculumIdAsync(int curriculumId);

    }
}
