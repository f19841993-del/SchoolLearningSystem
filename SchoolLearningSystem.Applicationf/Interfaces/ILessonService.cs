using SchoolLearningSystem.Applicationf.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ILessonService
    {
        // العمليات الأساسية
        Task<IEnumerable<LessonDto>> GetAllLessonsAsync();
        Task<LessonDto?> GetLessonByIdAsync(int id);
        Task AddLessonAsync(LessonDto dto);
        Task UpdateLessonAsync(LessonDto dto);
        Task DeleteLessonAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<ExamDto>> GetExamsByLessonIdAsync(int lessonId);
        Task<IEnumerable<ExerciseDto>> GetExercisesByLessonIdAsync(int lessonId);
        Task<IEnumerable<ResultDto>> GetResultsByLessonIdAsync(int lessonId);
        Task<IEnumerable<MemorizeSessionDto>> GetMemorizeSessionsByLessonIdAsync(int lessonId);

        // علاقات جديدة حسب الـ Controller
        Task<IEnumerable<QuestionDto>> GetQuestionsByLessonIdAsync(int lessonId);

        // إحصائيات
        Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId);
        Task<int> GetTotalExamsByLessonIdAsync(int lessonId);
    }
}
