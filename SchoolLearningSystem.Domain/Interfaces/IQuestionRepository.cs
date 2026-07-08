using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        // استعلامات مخصصة لخدمة الـ AI والـ Exam System
        Task<IEnumerable<Question>> GetByExamIdAsync(int examId);
        Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(int lessonId);

        // 🆕 جلب الأسئلة حسب مستوى الصعوبة (كانت ناقصة - يحتاجها QuestionService)
        Task<IEnumerable<Question>> GetByDifficultyAsync(DifficultyLevel difficultyLevel);

        // 🔹 إحصائيات
        Task<int> CountByExamIdAsync(int examId);
        Task<int> CountByDifficultyAsync(DifficultyLevel difficultyLevel);
        Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId);
    }
}