using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CurriculumDto;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ICurriculumService : IBaseService<CurriculumReadDto, CurriculumCreateDto, CurriculumUpdateDto>
    {
        // 🔹 العمليات الأساسية (GetAll, GetById, Create, Update, Delete, GetPaged)
        // موجودة مسبقاً بفضل الوراثة من IBaseService

        // ==========================================
        // 🔹 علاقات إضافية
        // ==========================================

        // جلب كورسات منهج معيّن
        Task<IEnumerable<CourseReadDto>> GetCoursesByCurriculumIdAsync(int curriculumId);

        // ==========================================
        // 🔹 البحث حسب المرحلة الدراسية
        // ==========================================

        // استخدام GradeLevel (Enum) بدل string يضمن Type-Safety
        // ترجع منهج واحد فقط لأن العلاقة صف↔منهج حالياً 1:1 (مادة الرياضيات فقط)
        Task<CurriculumReadDto?> GetCurriculumByGradeLevelAsync(GradeLevel gradeLevel);

        // ==========================================
        // 🔹 إحصائيات
        // ==========================================

        Task<int> GetTotalCoursesByCurriculumIdAsync(int curriculumId);
    }
}