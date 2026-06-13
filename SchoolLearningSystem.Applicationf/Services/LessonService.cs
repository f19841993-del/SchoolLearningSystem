using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class LessonService : BaseService<Lesson, LessonReadDto, LessonCreateDto, LessonUpdateDto>, ILessonService
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonService(ILessonRepository lessonRepository, IMapper mapper)
            : base(lessonRepository, mapper)
        {
            _lessonRepository = lessonRepository;
        }

        // 🔹 CRUD الأساسي:
        // أصبح موروثاً تلقائياً من BaseService، لا حاجة لكتابته!

        // 🔹 علاقات إضافية (Business Logic)
        public async Task<IEnumerable<ExamReadDto>> GetExamsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new Exception("Lesson not found");

            return _mapper.Map<IEnumerable<ExamReadDto>>(lesson.Exams);
        }

        public async Task<IEnumerable<ExerciseReadDto>> GetExercisesByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new Exception("Lesson not found");

            return _mapper.Map<IEnumerable<ExerciseReadDto>>(lesson.Exercises);
        }

        public async Task<IEnumerable<ResultReadDto>> GetResultsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new Exception("Lesson not found");

            return _mapper.Map<IEnumerable<ResultReadDto>>(lesson.Results);
        }

        public async Task<IEnumerable<MemorizeSessionReadDto>> GetMemorizeSessionsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new Exception("Lesson not found");

            return _mapper.Map<IEnumerable<MemorizeSessionReadDto>>(lesson.MemorizeSessions);
        }

        public async Task<IEnumerable<QuestionReadDto>> GetQuestionsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new Exception("Lesson not found");

            return _mapper.Map<IEnumerable<QuestionReadDto>>(lesson.Questions);
        }

        // 🔹 إحصائيات
        // نصيحة: إذا كان العدد كبيراً جداً، يفضل عمل Count في الـ Repository مباشرة
        public async Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new Exception("Lesson not found");

            return lesson.Questions?.Count ?? 0;
        }

        public async Task<int> GetTotalExamsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new Exception("Lesson not found");

            return lesson.Exams?.Count ?? 0;
        }
    }
}