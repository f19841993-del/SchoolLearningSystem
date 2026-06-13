using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using SchoolLearningSystem.Domain.Enums; // استخدم الـ Enum لضمان النوع
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IQuestionService : IBaseService<QuestionReadDto, QuestionCreateDto, QuestionUpdateDto>
    {
        // 🔹 CRUD الأساسي: موروث من IBaseService

        // 🔹 علاقات إضافية (Business Logic)
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByExamIdAsync(int examId);
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByLessonIdAsync(int lessonId);

        // 🔹 إحصائيات
        Task<int> GetQuestionCountByExamIdAsync(int examId);

        // عدلنا هنا لاستخدام الـ Enum لضمان الـ Type-Safety
        Task<int> GetQuestionCountByDifficultyAsync(DifficultyLevel difficultyLevel);
    }
}