using SchoolLearningSystem.Applicationf.DTOs.Question;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IQuestionService
    {
        // 🔹 العمليات الأساسية
        Task<IEnumerable<QuestionReadDto>> GetAllQuestionsAsync();
        Task<QuestionReadDto?> GetQuestionByIdAsync(int id);
        Task AddQuestionAsync(QuestionCreateDto dto);
        Task UpdateQuestionAsync(int id, QuestionUpdateDto dto);
        Task DeleteQuestionAsync(int id);

        // 🔹 علاقات إضافية
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByExamIdAsync(int examId);
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByLessonIdAsync(int lessonId);

        // 🔹 إحصائيات إضافية (اختياري)
        Task<int> GetQuestionCountByExamIdAsync(int examId);
        Task<int> GetQuestionCountByDifficultyAsync(string difficultyLevel);
    }
}
