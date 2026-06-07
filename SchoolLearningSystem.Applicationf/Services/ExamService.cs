using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;

        public ExamService(
            IExamRepository examRepository,
            ICourseRepository courseRepository,
            ILessonRepository lessonRepository,
            IMapper mapper)
        {
            _examRepository = examRepository;
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
            _mapper = mapper;
        }

        // 🔹 العمليات الأساسية
        public async Task<IEnumerable<ExamReadDto>> GetAllExamsAsync()
        {
            var entities = await _examRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ExamReadDto>>(entities);
        }

        public async Task<ExamReadDto?> GetExamByIdAsync(int id)
        {
            var entity = await _examRepository.GetByIdAsync(id);
            return _mapper.Map<ExamReadDto?>(entity);
        }

        public async Task AddExamAsync(ExamCreateDto dto)
        {
            var course = await _courseRepository.GetByIdAsync(dto.CourseId)
                ?? throw new Exception("Course not found");
            var lesson = await _lessonRepository.GetByIdAsync(dto.LessonId)
                ?? throw new Exception("Lesson not found");

            var entity = _mapper.Map<Exam>(dto);
            entity.Course = course;
            entity.Lesson = lesson;

            await _examRepository.AddAsync(entity);
        }

        public async Task UpdateExamAsync(int id, ExamUpdateDto dto)
        {
            var entity = await _examRepository.GetByIdAsync(id)
                ?? throw new Exception("Exam not found");

            _mapper.Map(dto, entity);
            await _examRepository.UpdateAsync(entity);
        }

        public async Task DeleteExamAsync(int id)
        {
            await _examRepository.DeleteAsync(id);
        }

        // 🔹 علاقات إضافية
        public async Task<IEnumerable<QuestionReadDto>> GetQuestionsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId)
                ?? throw new Exception("Exam not found");

            return _mapper.Map<IEnumerable<QuestionReadDto>>(exam.Questions);
        }

        public async Task<IEnumerable<ResultReadDto>> GetResultsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId)
                ?? throw new Exception("Exam not found");

            return _mapper.Map<IEnumerable<ResultReadDto>>(exam.Results);
        }

        public async Task<IEnumerable<LessonReadDto>> GetLessonsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId)
                ?? throw new Exception("Exam not found");

            return _mapper.Map<IEnumerable<LessonReadDto>>(new List<Lesson> { exam.Lesson });
        }
    }
}
