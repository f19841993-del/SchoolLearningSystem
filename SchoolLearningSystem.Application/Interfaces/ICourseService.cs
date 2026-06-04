using SchoolLearningSystem.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Application.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(int id);
        Task AddCourseAsync(CourseDto dto, int teacherId);
        Task UpdateCourseAsync(CourseDto dto);
        Task DeleteCourseAsync(int id);
    }
}
