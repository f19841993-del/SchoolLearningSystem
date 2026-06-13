using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IStudentService : IBaseService<StudentReadDto, StudentCreateDto, StudentUpdateDto>
    {
        // 🔹 CRUD الأساسي: موروث من IBaseService، لا حاجة لكتابته هنا!

        // 🔹 علاقات إضافية (Business Logic)
        Task<IEnumerable<CourseReadDto>> GetCoursesByStudentIdAsync(int studentId);
        Task<IEnumerable<ResultReadDto>> GetResultsByStudentIdAsync(int studentId);
        Task<IEnumerable<MemorizeSessionReadDto>> GetMemorizeSessionsByStudentIdAsync(int studentId);

        // 🔹 إحصائيات
        Task<double> GetAverageScoreByStudentIdAsync(int studentId);
        Task<int> GetTotalCoursesByStudentIdAsync(int studentId);
    }
}