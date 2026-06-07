using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class MemorizeService : IMemorizeService
    {
        private readonly IMemorizeRepository _memorizeRepository;
        private readonly IMapper _mapper;

        public MemorizeService(IMemorizeRepository memorizeRepository, IMapper mapper)
        {
            _memorizeRepository = memorizeRepository;
            _mapper = mapper;
        }

        // 🔹 العمليات الأساسية
        public async Task<IEnumerable<MemorizeSessionReadDto>> GetAllSessionsAsync()
        {
            var sessions = await _memorizeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MemorizeSessionReadDto>>(sessions);
        }

        public async Task<MemorizeSessionReadDto?> GetSessionByIdAsync(int id)
        {
            var session = await _memorizeRepository.GetByIdAsync(id);
            return _mapper.Map<MemorizeSessionReadDto?>(session);
        }

        public async Task AddSessionAsync(MemorizeSessionCreateDto dto)
        {
            var entity = _mapper.Map<MemorizeSession>(dto);
            await _memorizeRepository.AddAsync(entity);
        }

        public async Task UpdateSessionAsync(int id, MemorizeSessionUpdateDto dto)
        {
            var entity = await _memorizeRepository.GetByIdAsync(id)
                ?? throw new Exception("Session not found");

            _mapper.Map(dto, entity);
            await _memorizeRepository.UpdateAsync(entity);
        }

        public async Task DeleteSessionAsync(int id)
        {
            await _memorizeRepository.DeleteAsync(id);
        }

        // 🔹 علاقات إضافية
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
