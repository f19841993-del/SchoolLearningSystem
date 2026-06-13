using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IMemorizeService : IBaseService<MemorizeSessionReadDto, MemorizeSessionCreateDto, MemorizeSessionUpdateDto>
    {
        // 🔹 CRUD الأساسي: موروث من IBaseService

        // 🔹 علاقات إضافية (Specific Business Logic)
        Task<IEnumerable<MemorizeSessionReadDto>> GetSessionsByStudentIdAsync(int studentId);
        Task<IEnumerable<MemorizeSessionReadDto>> GetSessionsByLessonIdAsync(int lessonId);
         Task<IEnumerable<MemorizeSessionReadDto>> GetSessionsByExerciseIdAsync(int exerciseId);

        // 🔹 استعلامات الربط
        Task<string> GetStudentNameBySessionIdAsync(int sessionId);
        Task<string> GetLessonTitleBySessionIdAsync(int sessionId);
        Task<string> GetExerciseQuestionBySessionIdAsync(int sessionId);
    }
}