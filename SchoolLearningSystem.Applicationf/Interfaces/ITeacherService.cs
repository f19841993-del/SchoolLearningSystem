using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ITeacherService
    {
        // العمليات الأساسية
        Task<IEnumerable<TeacherDto>> GetAllTeachersAsync();
        Task<TeacherDto?> GetTeacherByIdAsync(int id);
        Task AddTeacherAsync(TeacherDto dto);
        Task UpdateTeacherAsync(TeacherDto dto);
        Task DeleteTeacherAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<CourseDto>> GetCoursesByTeacherIdAsync(int teacherId);
        Task<IEnumerable<LessonDto>> GetLessonsByTeacherIdAsync(int teacherId);

        // إحصائيات
        Task<int> GetTotalCoursesByTeacherIdAsync(int teacherId);
        Task<int> GetTotalLessonsByTeacherIdAsync(int teacherId);
    }
}
