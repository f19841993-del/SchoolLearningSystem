using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ICourseStudentRepository
    {
        Task<IEnumerable<CourseStudent>> GetAllAsync();
        // 🌟 الدالة التي أضفناها لحل المشكلة (الترقيم)
        Task<(IEnumerable<CourseStudent> Items, int TotalCount)> GetPagedByCourseIdAsync(int courseId, int pageNumber, int pageSize);
        Task<(IEnumerable<CourseStudent> Items, int TotalCount)> GetPagedByStudentIdAsync(int studentId, int pageNumber, int pageSize);
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
