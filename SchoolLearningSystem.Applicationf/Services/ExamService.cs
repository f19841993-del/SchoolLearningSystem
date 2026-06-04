using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;

        public ExamService(IExamRepository examRepository, ILessonRepository lessonRepository, IMapper mapper)
        {
            _examRepository = examRepository;
            _lessonRepository = lessonRepository;
            _mapper = mapper;
        }

        // العمليات الأساسية
        public async Task<IEnumerable<ExamDto>> GetAllExamsAsync()
        {
            var exams = await _examRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ExamDto>>(exams);
        }

        public async Task<ExamDto?> GetExamByIdAsync(int id)
        {
            var exam = await _examRepository.GetByIdAsync(id);
            return _mapper.Map<ExamDto?>(exam);
        }

        public async Task AddExamAsync(ExamDto dto)
        {
            var entity = _mapper.Map<Exam>(dto);
            await _examRepository.AddAsync(entity);
        }

        public async Task UpdateExamAsync(ExamDto dto)
        {
            var entity = _mapper.Map<Exam>(dto);
            await _examRepository.UpdateAsync(entity);
        }

        public async Task DeleteExamAsync(int id)
        {
            await _examRepository.DeleteAsync(id);
        }

        // علاقات إضافية
        public async Task<IEnumerable<QuestionDto>> GetQuestionsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null) return Enumerable.Empty<QuestionDto>();

            return _mapper.Map<IEnumerable<QuestionDto>>(exam.Questions);
        }

        public async Task<IEnumerable<ResultDto>> GetResultsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null) return Enumerable.Empty<ResultDto>();

            return _mapper.Map<IEnumerable<ResultDto>>(exam.Results);
        }

        // ربط الامتحان بالدرس
        public async Task<IEnumerable<LessonDto>> GetLessonsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null) return Enumerable.Empty<LessonDto>();

            var lesson = await _lessonRepository.GetByIdAsync(exam.LessonId);
            if (lesson == null) return Enumerable.Empty<LessonDto>();

            return new List<LessonDto> { _mapper.Map<LessonDto>(lesson) };
        }
    }
}
