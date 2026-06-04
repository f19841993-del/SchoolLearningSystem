using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
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

        // العمليات الأساسية
        public async Task<IEnumerable<MemorizeSessionDto>> GetAllSessionsAsync()
        {
            var sessions = await _memorizeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MemorizeSessionDto>>(sessions);
        }

        public async Task<MemorizeSessionDto?> GetSessionByIdAsync(int id)
        {
            var session = await _memorizeRepository.GetByIdAsync(id);
            return _mapper.Map<MemorizeSessionDto?>(session);
        }

        public async Task AddSessionAsync(MemorizeSessionDto dto)
        {
            var entity = _mapper.Map<MemorizeSession>(dto);
            await _memorizeRepository.AddAsync(entity);
        }

        public async Task UpdateSessionAsync(MemorizeSessionDto dto)
        {
            var entity = _mapper.Map<MemorizeSession>(dto);
            await _memorizeRepository.UpdateAsync(entity);
        }

        public async Task DeleteSessionAsync(int id)
        {
            await _memorizeRepository.DeleteAsync(id);
        }




        public async Task<IEnumerable<MemorizeSessionDto>> GetSessionsByStudentIdAsync(int studentId)
        {
            var sessions = await _memorizeRepository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<MemorizeSessionDto>>(sessions);
        }

        public async Task<IEnumerable<MemorizeSessionDto>> GetSessionsByLessonIdAsync(int lessonId)
        {
            var sessions = await _memorizeRepository.GetByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<MemorizeSessionDto>>(sessions);
        }

        public async Task<IEnumerable<MemorizeSessionDto>> GetSessionsByExerciseIdAsync(int exerciseId)
        {
            var sessions = await _memorizeRepository.GetByExerciseIdAsync(exerciseId);
            return _mapper.Map<IEnumerable<MemorizeSessionDto>>(sessions);
        }


        // علاقات إضافية
        public async Task<string> GetStudentNameBySessionIdAsync(int sessionId)
        {
            var session = await _memorizeRepository.GetByIdAsync(sessionId);
            return session?.Student?.Name ?? string.Empty;
        }

        public async Task<string> GetLessonTitleBySessionIdAsync(int sessionId)
        {
            var session = await _memorizeRepository.GetByIdAsync(sessionId);
            return session?.Lesson?.Title ?? string.Empty;
        }

        public async Task<string> GetExerciseTitleBySessionIdAsync(int sessionId)
        {
            var session = await _memorizeRepository.GetByIdAsync(sessionId);
            return session?.Exercise?.Question ?? string.Empty;
        }
    }
}
