using SchoolLearningSystem.Applicationf.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ICurriculumService
    {
        // العمليات الأساسية
        Task<IEnumerable<CurriculumDto>> GetAllCurriculumsAsync();
        Task<CurriculumDto?> GetCurriculumByIdAsync(int id);
        Task AddCurriculumAsync(CurriculumDto dto);
        Task UpdateCurriculumAsync(CurriculumDto dto);
        Task DeleteCurriculumAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<CourseDto>> GetCoursesByCurriculumIdAsync(int curriculumId);

        // البحث حسب المرحلة الدراسية
        Task<CurriculumDto?> GetCurriculumByGradeLevelAsync(string gradeLevel);

        // إحصائيات
        Task<int> GetTotalCoursesByCurriculumIdAsync(int curriculumId);
    }
}
