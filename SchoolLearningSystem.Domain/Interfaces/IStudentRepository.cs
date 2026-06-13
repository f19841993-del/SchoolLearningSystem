using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;
using SchoolLearningSystem.Domain.Enums; // نحتاجها لاستعلام المرحلة الدراسية

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        // 🔹 الـ CRUD الأساسي يأتينا تلقائياً من IGenericRepository<Student>

        // 🔹 استعلامات مخصصة تخدم "الذكاء الاصطناعي" و "نظام التكرار المتباعد":

        // لجلب طلاب مرحلة معينة (يساعد الـ AI في تصفية المحتوى التعليمي)
        Task<IEnumerable<Student>> GetByGradeLevelAsync(GradeLevel gradeLevel);

        // لجلب الطالب مع بيانات تقدمه (مهم جداً للـ SrsService)
        Task<Student?> GetStudentWithProgressAsync(int studentId);
    }
}