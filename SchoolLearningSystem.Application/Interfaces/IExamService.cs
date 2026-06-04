using SchoolLearningSystem.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Application.Interfaces
{
    public interface IExamService
    {
        Task<IEnumerable<ExamDto>> GetAllExamsAsync();
        Task<ExamDto?> GetExamByIdAsync(int id);
        Task AddExamAsync(ExamDto dto);
        Task UpdateExamAsync(ExamDto dto);
        Task DeleteExamAsync(int id);
    }
}
