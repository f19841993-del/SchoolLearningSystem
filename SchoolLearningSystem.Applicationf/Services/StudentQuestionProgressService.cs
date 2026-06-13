using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class StudentQuestionProgressService : IStudentQuestionProgressService
    {
        private readonly IStudentQuestionProgressRepository _repository;
        private readonly IMapper _mapper;

        public StudentQuestionProgressService(IStudentQuestionProgressRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<StudentQuestionProgressReadDto?> GetByStudentAndQuestionAsync(int studentId, int questionId)
        {
            var entity = await _repository.GetByStudentAndQuestionAsync(studentId, questionId);
            return _mapper.Map<StudentQuestionProgressReadDto?>(entity);
        }

        public async Task<IEnumerable<StudentQuestionProgressReadDto>> GetDueQuestionsAsync(int studentId)
        {
            var dueEntities = await _repository.GetDueQuestionsAsync(studentId);
            return _mapper.Map<IEnumerable<StudentQuestionProgressReadDto>>(dueEntities);
        }

        public async Task<IEnumerable<StudentQuestionProgressReadDto>> GetByStudentIdAsync(int studentId)
        {
            var entities = await _repository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<StudentQuestionProgressReadDto>>(entities);
        }

        public async Task AddProgressAsync(StudentQuestionProgressCreateDto dto)
        {
            var entity = _mapper.Map<StudentQuestionProgress>(dto);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateProgressAsync(StudentQuestionProgressUpdateDto dto)
        {
            // استرجاع الكيان بناءً على المفاتيح المركبة
            var existing = await _repository.GetByStudentAndQuestionAsync(dto.StudentId, dto.QuestionId)
                ?? throw new Exception("Progress record not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }
    }
}