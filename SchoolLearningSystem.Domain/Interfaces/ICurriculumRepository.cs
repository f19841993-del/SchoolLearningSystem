using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;
using SchoolLearningSystem.Domain.Enums; // لاستخدام الـ GradeLevel

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ICurriculumRepository : IGenericRepository<Curriculum>
    {
        // استخدام الـ Enum هنا يضمن دقة البيانات (Type-Safety)
        Task<Curriculum?> GetByGradeLevelAsync(GradeLevel gradeLevel);
    }
}