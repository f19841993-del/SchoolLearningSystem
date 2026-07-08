using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;
using SchoolLearningSystem.Domain.Enums; // لاستخدام الـ GradeLevel

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ICurriculumRepository : IGenericRepository<Curriculum>
    {
        Task<Curriculum?> GetByGradeLevelAsync(GradeLevel gradeLevel);
    }                                                                                   // أو لو بقيت مادة وحدة بس:
                                                                                        // Task<Curriculum?> GetByGradeAndSubjectAsync(GradeLevel gradeLevel, string subject);
}
