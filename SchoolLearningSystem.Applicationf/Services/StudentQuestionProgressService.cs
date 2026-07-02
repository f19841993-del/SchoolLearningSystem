using AutoMapper;
using FluentValidation; // 👈 1. استدعاء مكتبة التحقق
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class StudentQuestionProgressService : IStudentQuestionProgressService
    {
        private readonly IStudentQuestionProgressRepository _repository;
        private readonly IMapper _mapper;

        // 👈 2. إضافة حراس الحماية (Validators)
        private readonly IValidator<StudentQuestionProgressCreateDto> _createValidator;
        private readonly IValidator<StudentQuestionProgressUpdateDto> _updateValidator;

        public StudentQuestionProgressService(
            IStudentQuestionProgressRepository repository,
            IMapper mapper,
            IValidator<StudentQuestionProgressCreateDto> createValidator,
            IValidator<StudentQuestionProgressUpdateDto> updateValidator)
        {
            _repository = repository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<StudentQuestionProgressReadDto?> GetByStudentAndQuestionAsync(int studentId, int questionId)
        {
            var entity = await _repository.GetByStudentAndQuestionAsync(studentId, questionId);
            return _mapper.Map<StudentQuestionProgressReadDto?>(entity);
        }

        public async Task<IEnumerable<StudentQuestionProgressReadDto>> GetDueQuestionsAsync(int studentId)
        {
            // 🌟 التعديل هنا: الخدمة تحسب الوقت الحالي، وتمرره للريبو (الذي أصبح لديه دالة واحدة فقط تستقبل التاريخ)
            var currentTime = DateTime.UtcNow;
            var dueEntities = await _repository.GetDueQuestionsAsync(studentId, currentTime);

            return _mapper.Map<IEnumerable<StudentQuestionProgressReadDto>>(dueEntities);
        }

        public async Task<IEnumerable<StudentQuestionProgressReadDto>> GetByStudentIdAsync(int studentId)
        {
            var entities = await _repository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<StudentQuestionProgressReadDto>>(entities);
        }

        public async Task AddProgressAsync(StudentQuestionProgressCreateDto dto)
        {
            // 👈 3. فحص البيانات قبل الإضافة
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new CustomValidationException(validationResult.Errors);

            var entity = _mapper.Map<StudentQuestionProgress>(dto);
            await _repository.AddAsync(entity);
            //await _repository.SaveChangesAsync(); // التأكد من الحفظ
        }

        public async Task UpdateProgressAsync(StudentQuestionProgressUpdateDto dto)
        {
            // 👈 4. فحص البيانات قبل التعديل
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new CustomValidationException(validationResult.Errors);

            // استرجاع الكيان بناءً على المفاتيح المركبة
            var existing = await _repository.GetByStudentAndQuestionAsync(dto.StudentId, dto.QuestionId)
                ?? throw new NotFoundException($"Progress record for Student {dto.StudentId} and Question {dto.QuestionId} not found.");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
            //await _repository.SaveChangesAsync(); // التأكد من الحفظ
        }
    }
}