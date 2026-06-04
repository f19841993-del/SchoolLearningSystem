using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;

        public ExerciseService(IExerciseRepository exerciseRepository, ILessonRepository lessonRepository, IMapper mapper)
        {
            _exerciseRepository = exerciseRepository;
            _lessonRepository = lessonRepository;
            _mapper = mapper;
        }

        // العمليات الأساسية
        public async Task<IEnumerable<ExerciseDto>> GetAllExercisesAsync()
        {
            var exercises = await _exerciseRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ExerciseDto>>(exercises);
        }

        public async Task<ExerciseDto?> GetExerciseByIdAsync(int id)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id);
            return _mapper.Map<ExerciseDto?>(exercise);
        }

        public async Task AddExerciseAsync(ExerciseDto dto)
        {
            var entity = _mapper.Map<Exercise>(dto);
            await _exerciseRepository.AddAsync(entity);
        }

        public async Task UpdateExerciseAsync(ExerciseDto dto)
        {
            var entity = _mapper.Map<Exercise>(dto);
            await _exerciseRepository.UpdateAsync(entity);
        }

        public async Task DeleteExerciseAsync(int id)
        {
            await _exerciseRepository.DeleteAsync(id);
        }

        // علاقات إضافية
        public async Task<IEnumerable<ExerciseDto>> GetExercisesByLessonIdAsync(int lessonId)
        {
            var exercises = await _exerciseRepository.GetByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<ExerciseDto>>(exercises);
        }

        public async Task<IEnumerable<MemorizeSessionDto>> GetMemorizeSessionsByExerciseIdAsync(int exerciseId)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            if (exercise == null) return Enumerable.Empty<MemorizeSessionDto>();

            return _mapper.Map<IEnumerable<MemorizeSessionDto>>(exercise.MemorizeSessions);
        }

        public async Task<LessonDto?> GetLessonByExerciseIdAsync(int exerciseId)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            if (exercise == null) return null;

            var lesson = await _lessonRepository.GetByIdAsync(exercise.LessonId);
            return _mapper.Map<LessonDto?>(lesson);
        }
    }
}
