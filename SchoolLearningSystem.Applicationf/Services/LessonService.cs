using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using AutoMapper;

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

        // 🔹 العمليات الأساسية
        public async Task<IEnumerable<LessonDto>> GetAllLessonsAsync()
        {
            var lessons = await _lessonRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LessonDto>>(lessons);
        }

        public async Task<LessonDto?> GetLessonByIdAsync(int id)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);
            return _mapper.Map<LessonDto?>(lesson);
        }

        public async Task AddLessonAsync(LessonDto dto)
        {
            var lesson = _mapper.Map<Lesson>(dto);
            await _lessonRepository.AddAsync(lesson);
        }

        public async Task UpdateLessonAsync(LessonDto dto)
        {
            var lesson = _mapper.Map<Lesson>(dto);
            await _lessonRepository.UpdateAsync(lesson);
        }

        public async Task DeleteLessonAsync(int id)
        {
            await _lessonRepository.DeleteAsync(id);
        }

        // 🔹 علاقات إضافية
        public async Task<IEnumerable<ExamDto>> GetExamsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            return _mapper.Map<IEnumerable<ExamDto>>(lesson?.Exams);
        }

        public async Task<IEnumerable<ExerciseDto>> GetExercisesByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            return _mapper.Map<IEnumerable<ExerciseDto>>(lesson?.Exercises);
        }

        public async Task<IEnumerable<ResultDto>> GetResultsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            return _mapper.Map<IEnumerable<ResultDto>>(lesson?.Results);
        }

        public async Task<IEnumerable<MemorizeSessionDto>> GetMemorizeSessionsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            return _mapper.Map<IEnumerable<MemorizeSessionDto>>(lesson?.MemorizeSessions);
        }

        // 🔹 علاقات جديدة
        public async Task<IEnumerable<QuestionDto>> GetQuestionsByLessonIdAsync(int lessonId)
        {
            var questions = await _lessonRepository.GetQuestionsByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<QuestionDto>>(questions);
        }

        // 🔹 إحصائيات
        public async Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId)
        {
            return await _lessonRepository.GetTotalQuestionsByLessonIdAsync(lessonId);
        }

        public async Task<int> GetTotalExamsByLessonIdAsync(int lessonId)
        {
            return await _lessonRepository.GetTotalExamsByLessonIdAsync(lessonId);
        }
    }
}
