using SchoolLearningSystem.Applicationf.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ICourseStudentService
    {
        // العمليات الأساسية
        Task<IEnumerable<CourseStudentDto>> GetAllCourseStudentsAsync();
        Task<CourseStudentDto?> GetCourseStudentByIdAsync(int courseId, int studentId);
        Task AddCourseStudentAsync(CourseStudentDto dto);
        Task UpdateCourseStudentAsync(CourseStudentDto dto);
        Task DeleteCourseStudentAsync(int courseId, int studentId);

        // علاقات إضافية
        Task<IEnumerable<StudentDto>> GetStudentsByCourseIdAsync(int courseId);
        Task<IEnumerable<CourseDto>> GetCoursesByStudentIdAsync(int studentId);

        // عمليات التسجيل والإزالة
        Task EnrollStudentAsync(int courseId, int studentId);
        Task RemoveStudentAsync(int courseId, int studentId);

        // إحصائيات
        Task<int> GetTotalStudentsByCourseIdAsync(int courseId);
        Task<int> GetTotalCoursesByStudentIdAsync(int studentId);
    }
}
