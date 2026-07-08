using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(AppDbContext context) : base(context)
        {
        }

        // 💡 ملاحظة: لا تضف GetCoursesByTeacherIdAsync هنا. تم حذفها عن قصد لأن
        // ICourseRepository.GetByTeacherIdAsync هي المصدر الوحيد لهذي البيانات
        // (مبدأ "مصدر واحد للحقيقة" - راجع النقاش الخاص بـ ITeacherRepository).
        // TeacherService.GetCoursesByTeacherIdAsync تستدعي ICourseRepository مباشرة.
    }
}