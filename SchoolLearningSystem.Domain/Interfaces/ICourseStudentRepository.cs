using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ICourseStudentRepository
    {
        Task<IEnumerable<CourseStudent>> GetAllAsync();
        Task<CourseStudent?> GetByIdAsync(int courseId, int studentId);
        Task AddAsync(CourseStudent courseStudent);
        Task UpdateAsync(CourseStudent courseStudent);
        Task DeleteAsync(int courseId, int studentId);

        // دوال إضافية ممكن تحتاجها مستقبلاً
        Task<IEnumerable<CourseStudent>> GetByCourseIdAsync(int courseId);
        Task<IEnumerable<CourseStudent>> GetByStudentIdAsync(int studentId);
        Task<int> CountByCourseIdAsync(int courseId);
        Task<int> CountByStudentIdAsync(int studentId);
    }
}
