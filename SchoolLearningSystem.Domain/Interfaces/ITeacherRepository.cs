using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        // CRUD الأساسي يأتينا تلقائياً من IGenericRepository<Teacher>

        // 🔹 استعلام مخصص (Custom Query)
        // جلب الكورسات التي يدرسها هذا المعلم (مهمة جداً للـ Dashboard الخاصة بالمعلم)
        Task<IEnumerable<Course>> GetCoursesByTeacherIdAsync(int teacherId);
    }
}