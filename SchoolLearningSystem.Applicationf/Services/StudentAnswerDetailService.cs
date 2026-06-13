using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class StudentAnswerDetailService : BaseService<StudentAnswerDetail, StudentAnswerDetailReadDto, StudentAnswerDetailCreateDto, StudentAnswerDetailUpdateDto>, IStudentAnswerDetailService
    {
        private readonly IStudentAnswerDetailRepository _repository;

        public StudentAnswerDetailService(IStudentAnswerDetailRepository repository, IMapper mapper)
            : base(repository, mapper) // الأب يدير الـ CRUD
        {
            _repository = repository;
        }

        // 🔹 CRUD الأساسي: موروث من BaseService (لا حاجة لكتابته هنا!)

        // 🔹 علاقات إضافية (Logic)
        public async Task<IEnumerable<StudentAnswerDetailReadDto>> GetByStudentIdAsync(int studentId)
        {
            var results = await _repository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<StudentAnswerDetailReadDto>>(results);
        }

        public async Task<IEnumerable<StudentAnswerDetailReadDto>> GetByQuestionIdAsync(int questionId)
        {
            var results = await _repository.GetByQuestionIdAsync(questionId);
            return _mapper.Map<IEnumerable<StudentAnswerDetailReadDto>>(results);
        }

        public async Task<IEnumerable<StudentAnswerDetailReadDto>> GetRecentAnswersAsync(int studentId, int count)
        {
            var results = await _repository.GetRecentAnswersAsync(studentId, count);
            return _mapper.Map<IEnumerable<StudentAnswerDetailReadDto>>(results);
        }

        public async Task<IEnumerable<StudentAnswerDetailReadDto>> GetIncorrectAnswersByStudentIdAsync(int studentId, int lessonId)
        {
            var results = await _repository.GetIncorrectAnswersByStudentIdAsync(studentId, lessonId);
            return _mapper.Map<IEnumerable<StudentAnswerDetailReadDto>>(results);
        }
    }
}