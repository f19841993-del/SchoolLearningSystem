using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        // CRUD الأساسي يأتينا تلقائياً من IGenericRepository<Teacher>
        // ما نحتاج دوال إضافية حالياً - استخدم ICourseRepository.GetByTeacherIdAsync
        // لجلب كورسات المعلم بدل تكرارها هنا
    }
}