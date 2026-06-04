using SchoolLearningSystem.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Application.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDto>> GetAllTeachersAsync();
        Task<TeacherDto?> GetTeacherByIdAsync(int id);
        Task AddTeacherAsync(TeacherDto dto);
        Task UpdateTeacherAsync(TeacherDto dto);
        Task DeleteTeacherAsync(int id);
    }
}
