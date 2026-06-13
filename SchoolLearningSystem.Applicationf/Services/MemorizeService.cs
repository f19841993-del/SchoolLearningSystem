using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class MemorizeService : BaseService<MemorizeSession, MemorizeSessionReadDto, MemorizeSessionCreateDto, MemorizeSessionUpdateDto>, IMemorizeService
    {
        private readonly IMemorizeRepository _memorizeRepository;

        public MemorizeService(IMemorizeRepository memorizeRepository, IMapper mapper)
            : base(memorizeRepository, mapper) // الأب يدير الـ CRUD
        {
            _memorizeRepository = memorizeRepository;
        }

        // 🔹 CRUD الأساسي: موروث من BaseService (لا حاجة لكتابته هنا)

        // 🔹 علاقات إضافية (Logic)
        public async Task<IEnumerable<MemorizeSessionReadDto>> GetSessionsByStudentIdAsync(int studentId)
        {
            var sessions = await _memorizeRepository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<MemorizeSessionReadDto>>(sessions);
        }

        public async Task<IEnumerable<MemorizeSessionReadDto>> GetSessionsByLessonIdAsync(int lessonId)
        {
            var sessions = await _memorizeRepository.GetByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<MemorizeSessionReadDto>>(sessions);
        }

        public async Task<IEnumerable<MemorizeSessionReadDto>> GetSessionsByExerciseIdAsync(int exerciseId)
        {
            var sessions = await _memorizeRepository.GetByExerciseIdAsync(exerciseId);
            return _mapper.Map<IEnumerable<MemorizeSessionReadDto>>(sessions);
        }
    
        // 🔹 استعلامات الربط (Orchestration)
        public async Task<string> GetStudentNameBySessionIdAsync(int sessionId)
        {
            var session = await _memorizeRepository.GetByIdAsync(sessionId)
                ?? throw new Exception("Session not found");

            return session.Student?.Name ?? string.Empty;
        }

        public async Task<string> GetLessonTitleBySessionIdAsync(int sessionId)
        {
            var session = await _memorizeRepository.GetByIdAsync(sessionId)
                ?? throw new Exception("Session not found");

            return session.Lesson?.Title ?? string.Empty;
        }

        public async Task<string> GetExerciseQuestionBySessionIdAsync(int sessionId)
        {
            var session = await _memorizeRepository.GetByIdAsync(sessionId)
                ?? throw new Exception("Session not found");

            return session.Exercise?.Question ?? "No Exercise Linked";
        }

      
    }
}