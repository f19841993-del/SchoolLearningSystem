using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IExerciseService : IBaseService<ExerciseReadDto, ExerciseCreateDto, ExerciseUpdateDto>
    {
        // 🔹 العمليات الأساسية (GetAll, GetById, Create, Update, Delete, GetPaged)
        // موجودة مسبقاً بفضل الوراثة من IBaseService

        // ==========================================
        // 🔹 علاقات إضافية
        // ==========================================

        // جلب كل تمارين درس معيّن
        Task<IEnumerable<ExerciseReadDto>> GetExercisesByLessonIdAsync(int lessonId);

        // ==========================================
        // 🔹 دعم الذكاء الاصطناعي (AI Support)
        // ==========================================

        // جلب تمارين حسب مستوى صعوبة معيّن - لبناء مسار تعليمي تصاعدي (سهل ثم متوسط ثم صعب)
        Task<IEnumerable<ExerciseReadDto>> GetExercisesByDifficultyAsync(DifficultyLevel difficulty);
    }
}