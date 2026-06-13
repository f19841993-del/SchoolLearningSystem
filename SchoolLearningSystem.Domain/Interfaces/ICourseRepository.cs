using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    // أضف الوراثة هنا:
    public interface ICourseRepository : IGenericRepository<Course>
    {
        // العمليات الخاصة فقط بالكورس
        Task<IEnumerable<Student>> GetStudentsByCourseIdAsync(int courseId);
        Task<IEnumerable<Lesson>> GetLessonsByCourseIdAsync(int courseId);
        Task<IEnumerable<Exam>> GetExamsByCourseIdAsync(int courseId);
        Task EnrollStudentAsync(int courseId, int studentId);
        Task RemoveStudentAsync(int courseId, int studentId);
    }
}