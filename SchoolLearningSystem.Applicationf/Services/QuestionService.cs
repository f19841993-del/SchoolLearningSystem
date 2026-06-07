using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    // 🔹 العمليات الأساسية

    public async Task<IEnumerable<QuestionReadDto>> GetAllQuestionsAsync()
    {
        var questions = await _questionRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<QuestionReadDto>>(questions);
    }

    public async Task<QuestionReadDto?> GetQuestionByIdAsync(int id)
    {
        var question = await _questionRepository.GetByIdAsync(id);
        return question == null ? null : _mapper.Map<QuestionReadDto>(question);
    }

    public async Task AddQuestionAsync(QuestionCreateDto dto)
    {
        var question = _mapper.Map<Question>(dto);
        await _questionRepository.AddAsync(question);
    }

    public async Task UpdateQuestionAsync(int id, QuestionUpdateDto dto)
    {
        var existing = await _questionRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException("Question not found");

        _mapper.Map(dto, existing);
        await _questionRepository.UpdateAsync(existing);
    }

    public async Task DeleteQuestionAsync(int id)
    {
        var existing = await _questionRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException("Question not found");

        await _questionRepository.DeleteAsync(existing);
    }

    // 🔹 علاقات إضافية

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

    // 🔹 إحصائيات إضافية (اختياري)

    public async Task<int> GetQuestionCountByExamIdAsync(int examId)
    {
        return await _questionRepository.CountByExamIdAsync(examId);
    }

    public async Task<int> GetQuestionCountByDifficultyAsync(string difficultyLevel)
    {
        return await _questionRepository.CountByDifficultyAsync(difficultyLevel);
    }
}
