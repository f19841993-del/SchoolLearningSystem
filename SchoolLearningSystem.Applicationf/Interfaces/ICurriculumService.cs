using SchoolLearningSystem.Applicationf.DTOs.CourseDto; // نحتاجها للدالة الخاصة (GetCourses...)
using SchoolLearningSystem.Applicationf.DTOs.CurriculumDto;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolLearningSystem.Domain.Enums; // لا تنسى استيراد الـ Enum للمرحلة الدراسية

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    // لاحظ التغيير هنا: CurriculumDTOs بدلاً من CourseDTOs
    public interface ICurriculumService : IBaseService<CurriculumReadDto, CurriculumCreateDto, CurriculumUpdateDto>
    {
        // 🔹 العمليات الأساسية (تم توريثها من IBaseService وهي الآن تعمل على Curriculum)

        
        // 🔹 علاقات إضافية
        Task<IEnumerable<CourseReadDto>> GetCoursesByCurriculumIdAsync(int curriculumId);

        // 🔹 البحث حسب المرحلة الدراسية
        // ملاحظة: استبدلنا string بـ GradeLevel لضمان الـ Type-Safety
        Task<CurriculumReadDto?> GetCurriculumByGradeLevelAsync(GradeLevel gradeLevel);

        // 🔹 إحصائيات
        Task<int> GetTotalCoursesByCurriculumIdAsync(int curriculumId);
    }
}
