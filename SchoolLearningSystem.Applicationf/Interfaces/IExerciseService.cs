using SchoolLearningSystem.Applicationf.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IExerciseService
    {
        // العمليات الأساسية
        Task<IEnumerable<ExerciseDto>> GetAllExercisesAsync();
        Task<ExerciseDto?> GetExerciseByIdAsync(int id);
        Task AddExerciseAsync(ExerciseDto dto);
        Task UpdateExerciseAsync(ExerciseDto dto);
        Task DeleteExerciseAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<ExerciseDto>> GetExercisesByLessonIdAsync(int lessonId);
        Task<IEnumerable<MemorizeSessionDto>> GetMemorizeSessionsByExerciseIdAsync(int exerciseId);
        Task<LessonDto?> GetLessonByExerciseIdAsync(int exerciseId);
    }
}
