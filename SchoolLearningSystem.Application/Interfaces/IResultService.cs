using SchoolLearningSystem.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Application.Interfaces
{
    public interface IResultService
    {
        Task<IEnumerable<ResultDto>> GetAllResultsAsync();
        Task<ResultDto?> GetResultByIdAsync(int id);
        Task AddResultAsync(ResultDto dto);
        Task UpdateResultAsync(ResultDto dto);
        Task DeleteResultAsync(int id);
    }
}
