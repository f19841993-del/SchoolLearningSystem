using SchoolLearningSystem.Applicationf.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IQuestionService
    {
        // العمليات الأساسية
        Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync();
        Task<QuestionDto?> GetQuestionByIdAsync(int id);
        Task AddQuestionAsync(QuestionDto dto);
        Task UpdateQuestionAsync(QuestionDto dto);
        Task DeleteQuestionAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<QuestionDto>> GetQuestionsByExamIdAsync(int examId);
        Task<IEnumerable<QuestionDto>> GetQuestionsByLessonIdAsync(int lessonId);
    }
}
