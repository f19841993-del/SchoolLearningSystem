using SchoolLearningSystem.Applicationf.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IExamService
    {
        // العمليات الأساسية
        Task<IEnumerable<ExamDto>> GetAllExamsAsync();
        Task<ExamDto?> GetExamByIdAsync(int id);
        Task AddExamAsync(ExamDto dto);
        Task UpdateExamAsync(ExamDto dto);
        Task DeleteExamAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<QuestionDto>> GetQuestionsByExamIdAsync(int examId);
        Task<IEnumerable<ResultDto>> GetResultsByExamIdAsync(int examId);

        // ربط الامتحان بالدرس
        Task<IEnumerable<LessonDto>> GetLessonsByExamIdAsync(int examId);
    }
}
