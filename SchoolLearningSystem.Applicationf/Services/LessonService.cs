using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Exercise;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;

        public LessonService(ILessonRepository lessonRepository, IMapper mapper)
        {
            _lessonRepository = lessonRepository;
            _mapper = mapper;
        }

        // 🔹 CRUD الأساسي
        public async Task<IEnumerable<LessonReadDto>> GetAllLessonsAsync()
        {
            var lessons = await _lessonRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LessonReadDto>>(lessons);
        }

        public async Task<LessonReadDto?> GetLessonByIdAsync(int id)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);
            return _mapper.Map<LessonReadDto?>(lesson);
        }

        public async Task AddLessonAsync(LessonCreateDto dto)
        {
            var entity = _mapper.Map<Lesson>(dto);
            await _lessonRepository.AddAsync(entity);
        }

        public async Task UpdateLessonAsync(int id, LessonUpdateDto dto)
        {
            var entity = await _lessonRepository.GetByIdAsync(id)
                ?? throw new Exception("Lesson not found");

            _mapper.Map(dto, entity);
            await _lessonRepository.UpdateAsync(entity);
        }

        public async Task DeleteLessonAsync(int id)
        {
            await _lessonRepository.DeleteAsync(id);
        }

        // 🔹 علاقات إضافية
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
        public async Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new Exception("Lesson not found");

            return lesson.Questions.Count;
        }

        public async Task<int> GetTotalExamsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new Exception("Lesson not found");

            return lesson.Exams.Count;
        }
    }
}
