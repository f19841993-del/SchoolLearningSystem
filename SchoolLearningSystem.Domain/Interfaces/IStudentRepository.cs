using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        // 🔹 الـ CRUD الأساسي يأتينا تلقائياً من IGenericRepository<Student>

        // لجلب طلاب مرحلة معينة (يساعد الـ AI في تصفية المحتوى التعليمي)
        Task<IEnumerable<Student>> GetByGradeLevelAsync(GradeLevel gradeLevel);

        // لجلب الطالب مع بيانات تقدمه (مهم جداً لمحرك التكرار المتباعد SRS)
        Task<Student?> GetStudentWithProgressAsync(int studentId);

        // 💡 ملاحظة: تم حذف GetStudentsByCourseIdAsync من هنا عن قصد.
        // "جلب طلاب كورس معيّن" مسؤولية ICourseStudentService حصرياً
        // (عبر ICourseStudentRepository.GetByCourseIdAsync التي تحتوي أصلاً
        // Include(cs => cs.Student))، لتفادي تكرار نفس المنطق بمصدرين مختلفين.
    }
}