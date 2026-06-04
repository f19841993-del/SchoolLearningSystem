using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ICourseService
    {
        // العمليات الأساسية
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(int id);
        Task AddCourseAsync(CourseDto dto, int teacherId);
        Task UpdateCourseAsync(CourseDto dto);
        Task DeleteCourseAsync(int id);



        // علاقات إضافية
        Task<IEnumerable<StudentDto>> GetStudentsByCourseIdAsync(int courseId);
        Task<IEnumerable<LessonDto>> GetLessonsByCourseIdAsync(int courseId);
        Task<IEnumerable<ExamDto>> GetExamsByCourseIdAsync(int courseId);

        // ربط طالب بالكورس
        Task EnrollStudentAsync(int courseId, int studentId);
        Task RemoveStudentAsync(int courseId, int studentId);
    }

}
