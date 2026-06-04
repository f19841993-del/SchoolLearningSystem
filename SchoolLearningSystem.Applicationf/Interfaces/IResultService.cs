using SchoolLearningSystem.Applicationf.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IResultService
    {
        // العمليات الأساسية
        Task<IEnumerable<ResultDto>> GetAllResultsAsync();
        Task<ResultDto?> GetResultByIdAsync(int id);
        Task AddResultAsync(ResultDto dto);
        Task UpdateResultAsync(ResultDto dto);
        Task DeleteResultAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<ResultDto>> GetResultsByStudentIdAsync(int studentId);
        Task<IEnumerable<ResultDto>> GetResultsByLessonIdAsync(int lessonId);
        Task<IEnumerable<ResultDto>> GetResultsByExamIdAsync(int examId);
    }
}
