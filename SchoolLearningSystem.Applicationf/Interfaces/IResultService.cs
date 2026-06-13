using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IResultService : IBaseService<ResultReadDto, ResultCreateDto, ResultUpdateDto>
    {
        // 🔹 CRUD الأساسي: موروث من IBaseService (CreateAsync, UpdateAsync, DeleteAsync...)

        // 🔹 علاقات إضافية (Business Logic)
        Task<IEnumerable<ResultReadDto>> GetResultsByStudentIdAsync(int studentId);
        Task<IEnumerable<ResultReadDto>> GetResultsByLessonIdAsync(int lessonId);
        Task<IEnumerable<ResultReadDto>> GetResultsByExamIdAsync(int examId);

        // 🔹 إحصائيات
        Task<double> GetAverageScoreByStudentIdAsync(int studentId);
        Task<double> GetAverageScoreByLessonIdAsync(int lessonId);
        Task<double> GetAverageScoreByExamIdAsync(int examId);
    }
}