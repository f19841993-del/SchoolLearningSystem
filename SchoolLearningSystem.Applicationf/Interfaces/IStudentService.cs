using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IStudentService
    {
        // العمليات الأساسية
        Task<IEnumerable<StudentReadDto>> GetAllStudentsAsync();
        Task<StudentReadDto?> GetStudentByIdAsync(int id);
        Task AddStudentAsync(StudentCreateDto dto);
        Task UpdateStudentAsync(int id, StudentUpdateDto dto);
        Task DeleteStudentAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<CourseDto>> GetCoursesByStudentIdAsync(int studentId);
        Task<IEnumerable<ResultReadDto>> GetResultsByStudentIdAsync(int studentId);
        Task<IEnumerable<MemorizeSessionReadDto>> GetMemorizeSessionsByStudentIdAsync(int studentId);

        // إحصائيات
        Task<double> GetAverageScoreByStudentIdAsync(int studentId);
        Task<int> GetTotalCoursesByStudentIdAsync(int studentId);
    }
}
