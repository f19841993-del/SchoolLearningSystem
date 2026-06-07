using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Exercise;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
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

        // 🔹 CRUD الأساسي
        public async Task<IEnumerable<ExerciseReadDto>> GetAllExercisesAsync()
        {
            var entities = await _exerciseRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ExerciseReadDto>>(entities);
        }

        public async Task<ExerciseReadDto?> GetExerciseByIdAsync(int id)
        {
            var entity = await _exerciseRepository.GetByIdAsync(id);
            return _mapper.Map<ExerciseReadDto?>(entity);
        }

        public async Task AddExerciseAsync(ExerciseCreateDto dto)
        {
            var lesson = await _lessonRepository.GetByIdAsync(dto.LessonId)
                ?? throw new Exception("Lesson not found");

            var entity = _mapper.Map<Exercise>(dto);
            entity.Lesson = lesson;

            await _exerciseRepository.AddAsync(entity);
        }

        public async Task UpdateExerciseAsync(int id, ExerciseUpdateDto dto)
        {
            var entity = await _exerciseRepository.GetByIdAsync(id)
                ?? throw new Exception("Exercise not found");

            _mapper.Map(dto, entity);
            await _exerciseRepository.UpdateAsync(entity);
        }

        public async Task DeleteExerciseAsync(int id)
        {
            await _exerciseRepository.DeleteAsync(id);
        }

        // 🔹 علاقات إضافية
        public async Task<IEnumerable<ExerciseReadDto>> GetExercisesByLessonIdAsync(int lessonId)
        {
            var exercises = await _exerciseRepository.GetByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<ExerciseReadDto>>(exercises);
        }

        public async Task<IEnumerable<MemorizeSessionReadDto>> GetMemorizeSessionsByExerciseIdAsync(int exerciseId)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId)
                ?? throw new Exception("Exercise not found");

            return _mapper.Map<IEnumerable<MemorizeSessionReadDto>>(exercise.MemorizeSessions);
        }

        public async Task<LessonReadDto?> GetLessonByExerciseIdAsync(int exerciseId)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId)
                ?? throw new Exception("Exercise not found");

            return _mapper.Map<LessonReadDto?>(exercise.Lesson);
        }
    }
}
