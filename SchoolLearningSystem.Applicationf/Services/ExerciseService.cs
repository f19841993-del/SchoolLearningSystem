using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    // ==================================================================================
    // 📌 دور هذا الـ Service:
    // يدير "التمارين" (Exercise) - وهي أنشطة تدريبية غير رسمية داخل الدرس (بعكس
    // الامتحان/Exam الذي يُحسب رسمياً بالدرجات). تُستخدم للتدريب الفوري أثناء الدرس.
    // ==================================================================================
    public class ExerciseService
        : BaseService<Exercise, ExerciseReadDto, ExerciseCreateDto, ExerciseUpdateDto>, IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILessonRepository _lessonRepository;

        public ExerciseService(
            IExerciseRepository exerciseRepository,
            ILessonRepository lessonRepository,
            IMapper mapper)
            : base(exerciseRepository, mapper)
        {
            _exerciseRepository = exerciseRepository;
            _lessonRepository = lessonRepository;
        }

        // 🔹 CRUD الأساسي موروث من BaseService

        // ============================================================================
        // 🎯 Use Case: "الطالب يفتح درساً ويريد يتدرب فوراً بتمارين قصيرة بعد شرح
        //              كل فكرة، قبل ما ينتقل للامتحان الرسمي على الدرس كامل"
        //
        // مين يستدعيها: صفحة الدرس بواجهة الطالب (Controller الخاص بـ LessonDetails).
        // ============================================================================
        public async Task<IEnumerable<ExerciseReadDto>> GetExercisesByLessonIdAsync(int lessonId)
        {
            var lessonExists = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new NotFoundException($"الدرس برقم {lessonId} غير موجود.");

            var exercises = await _exerciseRepository.GetByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<ExerciseReadDto>>(exercises);
        }

        // ============================================================================
        // 🎯 Use Case: "محرك الذكاء الاصطناعي يبني مسار تدريب تصاعدي: يبدأ بتمارين
        //              سهلة، وكل ما الطالب ينجح يرفع له مستوى الصعوبة تدريجياً"
        //
        // مين يستدعيها: خدمة الـ AI الداخلية (نفس فكرة GetQuestionsByDifficultyAsync
        //               بـ QuestionService، بس هنا على مستوى التمارين غير الرسمية).
        // ============================================================================
        public async Task<IEnumerable<ExerciseReadDto>> GetExercisesByDifficultyAsync(DifficultyLevel difficulty)
        {
            var exercises = await _exerciseRepository.GetByDifficultyAsync(difficulty);
            return _mapper.Map<IEnumerable<ExerciseReadDto>>(exercises);
        }
    }
}