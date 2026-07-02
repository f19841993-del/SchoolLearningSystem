using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.Exceptions;
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
        private readonly IMemorizeRepository _memorizeRepository; // 👈 لا تنسَ إضافة هذا

        public ExerciseService(
            IExerciseRepository exerciseRepository,
            ILessonRepository lessonRepository,
            IMemorizeRepository memorizeRepository,
            IMapper mapper)
            : base(exerciseRepository, mapper) // هنا يتم ربط الـ CRUD الأساسي
        {
            _exerciseRepository = exerciseRepository;
            _lessonRepository = lessonRepository;
            _memorizeRepository = memorizeRepository; // 👈 لا تنسَ إضافة هذا
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
            // 1. التأكد من وجود التمرين (اختياري، يمكنك الاستغناء عنه إذا كان الـ Repository يقوم بالفحص)
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId)
                ?? throw new NotFoundException($"Exercise with ID {exerciseId} not found.");

            // 2. جلب الجلسات مباشرة من مستودع الجلسات (يجب إضافة هذه الدالة في الـ Repository)
            var sessions = await _memorizeRepository.GetByExerciseIdAsync(exerciseId);

            return _mapper.Map<IEnumerable<MemorizeSessionReadDto>>(sessions);
        }

        public async Task<LessonReadDto?> GetLessonByExerciseIdAsync(int exerciseId)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId)
                ?? throw new Exception("Exercise not found");

            return _mapper.Map<LessonReadDto?>(exercise.Lesson);
        }
    }
}