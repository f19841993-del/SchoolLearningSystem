using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IExamRepository _examRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IExamRepository examRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _examRepository = examRepository;
            _mapper = mapper;
        }

        // العمليات الأساسية
        public async Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync()
        {
            var questions = await _questionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<QuestionDto>>(questions);
        }

        public async Task<QuestionDto?> GetQuestionByIdAsync(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            return _mapper.Map<QuestionDto?>(question);
        }

        public async Task AddQuestionAsync(QuestionDto dto)
        {
            var entity = _mapper.Map<Question>(dto);
            await _questionRepository.AddAsync(entity);
        }

        public async Task UpdateQuestionAsync(QuestionDto dto)
        {
            var entity = _mapper.Map<Question>(dto);
            await _questionRepository.UpdateAsync(entity);
        }

        public async Task DeleteQuestionAsync(int id)
        {
            await _questionRepository.DeleteAsync(id);
        }

        // علاقات إضافية
        public async Task<IEnumerable<QuestionDto>> GetQuestionsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null) return Enumerable.Empty<QuestionDto>();

            return _mapper.Map<IEnumerable<QuestionDto>>(exam.Questions);
        }
        public async Task<IEnumerable<QuestionDto>> GetQuestionsByLessonIdAsync(int lessonId)
        {
            var questions = await _questionRepository.GetQuestionsByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<QuestionDto>>(questions);
        }

    }
}
