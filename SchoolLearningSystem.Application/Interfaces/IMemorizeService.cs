using SchoolLearningSystem.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Application.Interfaces
{
    public interface IMemorizeService
    {
        Task<IEnumerable<MemorizeSessionDto>> GetAllSessionsAsync();
        Task<MemorizeSessionDto?> GetSessionByIdAsync(int id);
        Task AddSessionAsync(MemorizeSessionDto dto);
        Task UpdateSessionAsync(MemorizeSessionDto dto);
        Task DeleteSessionAsync(int id);
    }
}
