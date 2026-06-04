using SchoolLearningSystem.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Application.Interfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonDto>> GetAllLessonsAsync();
        Task<LessonDto?> GetLessonByIdAsync(int id);
        Task AddLessonAsync(LessonDto dto);
        Task UpdateLessonAsync(LessonDto dto);
        Task DeleteLessonAsync(int id);
    }
}
