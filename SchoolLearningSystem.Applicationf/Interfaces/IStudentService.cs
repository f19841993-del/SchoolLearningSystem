using SchoolLearningSystem.Applicationf.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IStudentService
    {
        // العمليات الأساسية
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto?> GetStudentByIdAsync(int id);
        Task AddStudentAsync(StudentDto dto);
        Task UpdateStudentAsync(StudentDto dto);
        Task DeleteStudentAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<CourseDto>> GetCoursesByStudentIdAsync(int studentId);
        Task<IEnumerable<ResultDto>> GetResultsByStudentIdAsync(int studentId);
        Task<IEnumerable<MemorizeSessionDto>> GetMemorizeSessionsByStudentIdAsync(int studentId);

        // إحصائيات
        Task<double> GetAverageScoreByStudentIdAsync(int studentId);
        Task<int> GetTotalCoursesByStudentIdAsync(int studentId);
    }
}
