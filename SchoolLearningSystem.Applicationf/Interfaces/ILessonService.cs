using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ILessonService : IBaseService<LessonReadDto, LessonCreateDto, LessonUpdateDto>
    {
        // 🔹 العمليات الأساسية (GetAll, GetById, Add, Update, Delete) 
        // أصبحت الآن موروثة من IBaseService.

        // 🔹 علاقات إضافية (Business Logic)
        Task<IEnumerable<ExamReadDto>> GetExamsByLessonIdAsync(int lessonId);
        Task<IEnumerable<ExerciseReadDto>> GetExercisesByLessonIdAsync(int lessonId);
        Task<IEnumerable<ResultReadDto>> GetResultsByLessonIdAsync(int lessonId);
        Task<IEnumerable<MemorizeSessionReadDto>> GetMemorizeSessionsByLessonIdAsync(int lessonId);
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByLessonIdAsync(int lessonId);

        // 🔹 إحصائيات
        Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId);
        Task<int> GetTotalExamsByLessonIdAsync(int lessonId);
    }
}