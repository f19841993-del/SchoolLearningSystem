using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ILessonRepository : IGenericRepository<Lesson>
    {
        // جلب كل دروس كورس معيّن (مرتبة حسب Order)
        Task<IEnumerable<Lesson>> GetByCourseIdAsync(int courseId);

        // جلب الدرس التالي بالتسلسل
        Task<Lesson?> GetNextLessonAsync(int courseId, int currentLessonOrder);

        // 🆕 جلب الدرس المرتبط بتمرين معيّن (Exercise)
        Task<Lesson?> GetByExerciseIdAsync(int exerciseId);
    }
}