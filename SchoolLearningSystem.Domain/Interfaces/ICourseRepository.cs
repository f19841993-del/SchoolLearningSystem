using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        // جلب كل كورسات مدرّس معين
        Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId);

        // جلب كورسات منهج معين (مرتبة حسب Order لأنها فصول متسلسلة)
        Task<IEnumerable<Course>> GetByCurriculumIdAsync(int curriculumId);

        // جلب كورس مع تفاصيله الكاملة (دروس + امتحانات + طلاب) بطلب واحد لتفادي N+1
        Task<Course?> GetWithDetailsAsync(int courseId);

        // عدد الطلاب المسجلين بكورس (يفيد صفحة تفاصيل الكورس / الداشبورد)
        Task<int> GetEnrolledStudentsCountAsync(int courseId);
    }
}