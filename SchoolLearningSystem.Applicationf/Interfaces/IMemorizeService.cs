using SchoolLearningSystem.Applicationf.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IMemorizeService
    {
        // العمليات الأساسية
        Task<IEnumerable<MemorizeSessionDto>> GetAllSessionsAsync();
        Task<MemorizeSessionDto?> GetSessionByIdAsync(int id);
        Task AddSessionAsync(MemorizeSessionDto dto);
        Task UpdateSessionAsync(MemorizeSessionDto dto);
        Task DeleteSessionAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<MemorizeSessionDto>> GetSessionsByStudentIdAsync(int studentId);
        Task<IEnumerable<MemorizeSessionDto>> GetSessionsByLessonIdAsync(int lessonId);
        Task<IEnumerable<MemorizeSessionDto>> GetSessionsByExerciseIdAsync(int exerciseId);

        Task<string> GetStudentNameBySessionIdAsync(int sessionId);
        Task<string> GetLessonTitleBySessionIdAsync(int sessionId);
        Task<string> GetExerciseTitleBySessionIdAsync(int sessionId);
    }
}
