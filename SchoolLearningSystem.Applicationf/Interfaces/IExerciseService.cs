using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IExerciseService : IBaseService<ExerciseReadDto, ExerciseCreateDto, ExerciseUpdateDto>
    {
        // 🔹 ملاحظة: الـ CRUD الأساسية (GetAll, GetById, Create, Update, Delete) 
        // أصبحت الآن موجودة وموروثة من IBaseService.

        // 🔹 علاقات إضافية (Specific Business Logic)
        Task<IEnumerable<ExerciseReadDto>> GetExercisesByLessonIdAsync(int lessonId);

        Task<IEnumerable<MemorizeSessionReadDto>> GetMemorizeSessionsByExerciseIdAsync(int exerciseId);

        Task<LessonReadDto?> GetLessonByExerciseIdAsync(int exerciseId);
    }
}