
using SchoolLearningSystem.Application.Common.Models;
using SchoolLearningSystem.Application.Common.Parameters;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CourseStudent;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ICourseStudentService
    {
        // العمليات الأساسية
        Task<IEnumerable<CourseStudentReadDto>> GetAllCourseStudentsAsync();
        Task<CourseStudentReadDto?> GetCourseStudentByIdAsync(int courseId, int studentId);
        Task AddCourseStudentAsync(CourseStudentCreateDto dto);
        Task UpdateCourseStudentAsync(int courseId, int studentId, CourseStudentUpdateDto dto);
        Task DeleteCourseStudentAsync(int courseId, int studentId);

        // علاقات إضافية
        Task<IEnumerable<StudentReadDto>> GetStudentsByCourseIdAsync(int courseId);
        Task<IEnumerable<CourseReadDto>> GetCoursesByStudentIdAsync(int studentId);

        // عمليات التسجيل والإزالة (اختصار للإنشاء والحذف)
        Task EnrollStudentAsync(int courseId, int studentId);
        Task RemoveStudentAsync(int courseId, int studentId);

        // إحصائيات
        Task<int> GetTotalStudentsByCourseIdAsync(int courseId);
        Task<int> GetTotalCoursesByStudentIdAsync(int studentId);

        // جلب طلاب كورس معين مع الترقيم
        Task<PagedList<StudentReadDto>> GetPagedStudentsByCourseIdAsync(int courseId, QueryParameters parameters);

        // جلب كورسات طالب معين مع الترقيم
        Task<PagedList<CourseReadDto>> GetPagedCoursesByStudentIdAsync(int studentId, QueryParameters parameters);
    }
}
