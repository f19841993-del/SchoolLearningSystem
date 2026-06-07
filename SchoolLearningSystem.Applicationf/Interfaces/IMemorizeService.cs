using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IMemorizeService
    {
        // العمليات الأساسية
        Task<IEnumerable<MemorizeSessionReadDto>> GetAllSessionsAsync();
        Task<MemorizeSessionReadDto?> GetSessionByIdAsync(int id);
        Task AddSessionAsync(MemorizeSessionCreateDto dto);
        Task UpdateSessionAsync(int id, MemorizeSessionUpdateDto dto);
        Task DeleteSessionAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<MemorizeSessionReadDto>> GetSessionsByStudentIdAsync(int studentId);
        Task<IEnumerable<MemorizeSessionReadDto>> GetSessionsByLessonIdAsync(int lessonId);
        Task<IEnumerable<MemorizeSessionReadDto>> GetSessionsByExerciseIdAsync(int exerciseId);

        Task<string> GetStudentNameBySessionIdAsync(int sessionId);
        Task<string> GetLessonTitleBySessionIdAsync(int sessionId);
        Task<string> GetExerciseQuestionBySessionIdAsync(int sessionId);
    }
}
