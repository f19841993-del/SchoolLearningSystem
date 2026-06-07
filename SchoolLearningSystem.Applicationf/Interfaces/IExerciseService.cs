using SchoolLearningSystem.Applicationf.DTOs.Exercise;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IExerciseService
    {
        // العمليات الأساسية
        Task<IEnumerable<ExerciseReadDto>> GetAllExercisesAsync();
        Task<ExerciseReadDto?> GetExerciseByIdAsync(int id);
        Task AddExerciseAsync(ExerciseCreateDto dto);
        Task UpdateExerciseAsync(int id, ExerciseUpdateDto dto);
        Task DeleteExerciseAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<ExerciseReadDto>> GetExercisesByLessonIdAsync(int lessonId);
        Task<IEnumerable<MemorizeSessionReadDto>> GetMemorizeSessionsByExerciseIdAsync(int exerciseId);
        Task<LessonReadDto?> GetLessonByExerciseIdAsync(int exerciseId);
    }
}
