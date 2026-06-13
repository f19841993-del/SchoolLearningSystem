using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class SrsService : ISrsService
    {
        private readonly IStudentQuestionProgressRepository _progressRepository;
        private readonly IMapper _mapper;

        public SrsService(IStudentQuestionProgressRepository progressRepository, IMapper mapper)
        {
            _progressRepository = progressRepository;
            _mapper = mapper;
        }

        public async Task ProcessAnswerAsync(int studentId, int questionId, bool isCorrect, int timeTakenInSeconds)
        {
            // 1. جلب سجل التقدم الحالي
            var progress = await _progressRepository.GetByStudentAndQuestionAsync(studentId, questionId)
                ?? throw new Exception("Progress record not found.");

            // 2. تحديث الإحصائيات الأساسية
            progress.TotalAttempts++;
            if (isCorrect) progress.CorrectAttempts++;
            progress.LastReviewedAt = DateTime.UtcNow;

            // 3. هنا سيتم وضع خوارزمية SRS (مثلاً SM-2) 
            // TODO: استبدل هذا المنطق البسيط بمعادلة حسابية متقدمة لاحقاً
            progress.RepetitionLevel = isCorrect ? progress.RepetitionLevel + 1 : 0;
            progress.NextReviewDate = DateTime.UtcNow.AddDays(isCorrect ? Math.Pow(2, progress.RepetitionLevel) : 1);

            // 4. حفظ التغييرات
            await _progressRepository.UpdateAsync(progress);
        }

        public async Task<IEnumerable<StudentQuestionProgressReadDto>> GetDueQuestionsForSessionAsync(int studentId)
        {
            // جلب الأسئلة من الريبو التي تاريخ مراجعتها قد حل
            var dueQuestions = await _progressRepository.GetDueQuestionsAsync(studentId);

            // تحويلها لـ DTOs للعرض
            return _mapper.Map<IEnumerable<StudentQuestionProgressReadDto>>(dueQuestions);
        }
    }
}