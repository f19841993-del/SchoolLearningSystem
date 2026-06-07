
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Curriculum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ICurriculumService
    {
        // العمليات الأساسية
        Task<IEnumerable<CurriculumReadDto>> GetAllCurriculumsAsync();
        Task<CurriculumReadDto?> GetCurriculumByIdAsync(int id);
        Task AddCurriculumAsync(CurriculumCreateDto dto);
        Task UpdateCurriculumAsync(int id, CurriculumUpdateDto dto);
        Task DeleteCurriculumAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<CourseReadDto>> GetCoursesByCurriculumIdAsync(int curriculumId);

        // البحث حسب المرحلة الدراسية
        Task<CurriculumReadDto?> GetCurriculumByGradeLevelAsync(string gradeLevel);

        // إحصائيات
        Task<int> GetTotalCoursesByCurriculumIdAsync(int curriculumId);
    }
}
