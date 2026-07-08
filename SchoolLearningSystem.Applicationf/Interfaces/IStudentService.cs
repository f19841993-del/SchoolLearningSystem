using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IStudentService : IBaseService<StudentReadDto, StudentCreateDto, StudentUpdateDto>
    {
        // 🔹 العمليات الأساسية (GetAll, GetById, Create, Update, Delete, GetPaged)
        // موجودة مسبقاً بفضل الوراثة من IBaseService

        // ==========================================
        // 🔹 علاقات إضافية
        // ==========================================

        // جلب طلاب مرحلة دراسية معيّنة
        Task<IEnumerable<StudentReadDto>> GetStudentsByGradeLevelAsync(GradeLevel gradeLevel);

        // جلب الطالب مع كل بيانات تقدمه (لمحرك التكرار المتباعد / لوحة الأداء الشخصية)
        Task<StudentReadDto> GetStudentWithProgressAsync(int studentId);

        // 💡 ملاحظة: "طلاب كورس معيّن" موجودة بـ ICourseStudentService.GetStudentsByCourseIdAsync
        // ولا تُكرر هنا حفاظاً على مصدر واحد للحقيقة (Single Source of Truth)
    }
}