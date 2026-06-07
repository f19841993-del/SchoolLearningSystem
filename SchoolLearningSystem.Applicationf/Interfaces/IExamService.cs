
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IExamService
    {
        // العمليات الأساسية
        Task<IEnumerable<ExamReadDto>> GetAllExamsAsync();
        Task<ExamReadDto?> GetExamByIdAsync(int id);
        Task AddExamAsync(ExamCreateDto dto);
        Task UpdateExamAsync(int id, ExamUpdateDto dto);
        Task DeleteExamAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByExamIdAsync(int examId);
        Task<IEnumerable<ResultReadDto>> GetResultsByExamIdAsync(int examId);

        // ربط الامتحان بالدرس
        Task<IEnumerable<LessonReadDto>> GetLessonsByExamIdAsync(int examId);
    }
}
