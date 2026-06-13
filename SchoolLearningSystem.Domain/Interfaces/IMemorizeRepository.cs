using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base; // التأكد من استيراد الواجهة العامة

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IMemorizeRepository : IGenericRepository<MemorizeSession>
    {
        // CRUD الأساسي يأتينا تلقائياً من IGenericRepository<MemorizeSession>

        // استعلامات مخصصة لخدمة الـ AI والـ SRS
        Task<IEnumerable<MemorizeSession>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<MemorizeSession>> GetByLessonIdAsync(int lessonId);
        Task<IEnumerable<MemorizeSession>> GetByExerciseIdAsync(int exerciseId);
    }
}