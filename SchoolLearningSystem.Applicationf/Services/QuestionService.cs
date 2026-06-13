using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Domain.Enums; // استخدمنا الـ Enum لضمان النوع

namespace SchoolLearningSystem.Applicationf.Services
{
    public class QuestionService : BaseService<Question, QuestionReadDto, QuestionCreateDto, QuestionUpdateDto>, IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
            : base(questionRepository, mapper) // الأب يدير الـ CRUD
        {
            _questionRepository = questionRepository;
        }

        // 🔹 CRUD الأساسي: موروث من BaseService (لا حاجة لكتابته هنا)

        // 🔹 علاقات إضافية (Logic)
        public async Task<IEnumerable<QuestionReadDto>> GetQuestionsByExamIdAsync(int examId)
        {
            var questions = await _questionRepository.GetByExamIdAsync(examId);
            return _mapper.Map<IEnumerable<QuestionReadDto>>(questions);
        }

        public async Task<IEnumerable<QuestionReadDto>> GetQuestionsByLessonIdAsync(int lessonId)
        {
            var questions = await _questionRepository.GetByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<QuestionReadDto>>(questions);
        }

        // 🔹 إحصائيات إضافية
        public async Task<int> GetQuestionCountByExamIdAsync(int examId)
        {
            return await _questionRepository.CountByExamIdAsync(examId);
        }

        public async Task<int> GetQuestionCountByDifficultyAsync(DifficultyLevel difficultyLevel)
        {
            // تم تعديل المدخل ليصبح Enum بدلاً من string لزيادة الاحترافية (Type-Safety)
            return await _questionRepository.CountByDifficultyAsync(difficultyLevel);
        }
    }
}