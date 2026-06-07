using SchoolLearningSystem.Applicationf.DTOs.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IResultService
    {
        // العمليات الأساسية
        Task<IEnumerable<ResultReadDto>> GetAllResultsAsync();
        Task<ResultReadDto?> GetResultByIdAsync(int id);
        Task AddResultAsync(ResultCreateDto dto);
        Task UpdateResultAsync(int id, ResultUpdateDto dto);
        Task DeleteResultAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<ResultReadDto>> GetResultsByStudentIdAsync(int studentId);
        Task<IEnumerable<ResultReadDto>> GetResultsByLessonIdAsync(int lessonId);
        Task<IEnumerable<ResultReadDto>> GetResultsByExamIdAsync(int examId);

        // إحصائيات إضافية (اختياري)
        Task<double> GetAverageScoreByStudentIdAsync(int studentId);
        Task<double> GetAverageScoreByLessonIdAsync(int lessonId);
        Task<double> GetAverageScoreByExamIdAsync(int examId);
    }
}
