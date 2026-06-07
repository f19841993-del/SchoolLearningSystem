using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ITeacherService
    {
        // العمليات الأساسية
        Task<IEnumerable<TeacherReadDto>> GetAllTeachersAsync();
        Task<TeacherReadDto?> GetTeacherByIdAsync(int id);
        Task AddTeacherAsync(TeacherCreateDto dto);
        Task UpdateTeacherAsync(int id, TeacherUpdateDto dto);
        Task DeleteTeacherAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<CourseReadDto>> GetCoursesByTeacherIdAsync(int teacherId);
        Task<IEnumerable<LessonReadDto>> GetLessonsByTeacherIdAsync(int teacherId);

        // إحصائيات
        Task<int> GetTotalCoursesByTeacherIdAsync(int teacherId);
        Task<int> GetTotalLessonsByTeacherIdAsync(int teacherId);
    }
}
