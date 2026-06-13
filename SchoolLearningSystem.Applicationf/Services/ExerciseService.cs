using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class ExerciseService : BaseService<Exercise, ExerciseReadDto, ExerciseCreateDto, ExerciseUpdateDto>, IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILessonRepository _lessonRepository;

        public ExerciseService(
            IExerciseRepository exerciseRepository,
            ILessonRepository lessonRepository,
            IMapper mapper)
            : base(exerciseRepository, mapper) // هنا يتم ربط الـ CRUD الأساسي
        {
            _exerciseRepository = exerciseRepository;
            _lessonRepository = lessonRepository;
        }

        // 🔹 ملاحظة: الـ CRUD الأساسي (GetAll, GetById, Add, Update, Delete) 
        // لا نحتاج كتابتها لأنها موروثة من BaseService.

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