using SchoolLearningSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IMemorizeRepository
    {
        Task<IEnumerable<MemorizeSession>> GetAllAsync();
        Task<MemorizeSession?> GetByIdAsync(int id);
        Task AddAsync(MemorizeSession entity);
        Task UpdateAsync(MemorizeSession entity);
        Task DeleteAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<MemorizeSession>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<MemorizeSession>> GetByLessonIdAsync(int lessonId);
        Task<IEnumerable<MemorizeSession>> GetByExerciseIdAsync(int exerciseId);
    }
}
