using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    // 💡 لا يرث من IGenericRepository لأن CourseStudent له مفتاح مركّب (CourseId + StudentId)
    // 💡 منطق الأعمال (التحقق من التكرار، وجود الكورس/الطالب) أصبح مسؤولية CourseStudentService حصرياً
    // الريبو هنا يقدّم فقط عمليات وصول بيانات بسيطة (Data Access)
    public interface ICourseStudentRepository
    {
        Task<IEnumerable<CourseStudent>> GetAllAsync();
        Task<CourseStudent?> GetByIdAsync(int courseId, int studentId);
        Task AddAsync(CourseStudent courseStudent);
        Task UpdateAsync(CourseStudent courseStudent);
        Task DeleteAsync(int courseId, int studentId);

        Task<IEnumerable<CourseStudent>> GetByCourseIdAsync(int courseId);
        Task<IEnumerable<CourseStudent>> GetByStudentIdAsync(int studentId);

        Task<int> CountByCourseIdAsync(int courseId);
        Task<int> CountByStudentIdAsync(int studentId);

        Task<(IEnumerable<CourseStudent> Items, int TotalCount)> GetPagedByCourseIdAsync(int courseId, int pageNumber, int pageSize);
        Task<(IEnumerable<CourseStudent> Items, int TotalCount)> GetPagedByStudentIdAsync(int studentId, int pageNumber, int pageSize);
    }
}