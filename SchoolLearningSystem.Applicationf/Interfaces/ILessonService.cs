
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Exercise;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ILessonService
    {
        // العمليات الأساسية
        Task<IEnumerable<LessonReadDto>> GetAllLessonsAsync();
        Task<LessonReadDto?> GetLessonByIdAsync(int id);
        Task AddLessonAsync(LessonCreateDto dto);
        Task UpdateLessonAsync(int id, LessonUpdateDto dto);
        Task DeleteLessonAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<ExamReadDto>> GetExamsByLessonIdAsync(int lessonId);
        Task<IEnumerable<ExerciseReadDto>> GetExercisesByLessonIdAsync(int lessonId);
        Task<IEnumerable<ResultReadDto>> GetResultsByLessonIdAsync(int lessonId);
        Task<IEnumerable<MemorizeSessionReadDto>> GetMemorizeSessionsByLessonIdAsync(int lessonId);
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByLessonIdAsync(int lessonId);

        // إحصائيات
        Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId);
        Task<int> GetTotalExamsByLessonIdAsync(int lessonId);
    }
}
