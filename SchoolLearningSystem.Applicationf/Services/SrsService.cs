



using AutoMapper;
using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.Srs;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Domain.Services; // مسار خوارزمية SrsAlgorithm
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Services
{
    // خدمة ذكاء الأعمال الخاصة بالتكرار المتباعد (SRS)
    public class SrsService : ISrsService
    {
        private readonly IStudentQuestionProgressRepository _progressRepository;
        private readonly IStudentAnswerDetailService _answerDetailService;
        private readonly IMapper _mapper;
        private readonly IValidator<AnswerSubmissionDto> _answerValidator;

        public SrsService(
            IStudentQuestionProgressRepository progressRepository,
            IStudentAnswerDetailService answerDetailService,
            IMapper mapper,
            IValidator<AnswerSubmissionDto> answerValidator)
        {
            _progressRepository = progressRepository;
            _answerDetailService = answerDetailService;
            _mapper = mapper;
            _answerValidator = answerValidator;
        }

        public async Task ProcessAnswerAsync(AnswerSubmissionDto dto)
        {
            // 🛡️ 1. الفحص المسبق (Validation) لحماية الخوارزمية
            var validationResult = await _answerValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                // إذا فشل الفحص، نرمي خطأ يلتقطه الـ Middleware
                throw new CustomValidationException(validationResult.Errors);
            }

            // 🔍 2. البحث عن سجل تقدم الطالب لهذا السؤال (هل أجاب عليه سابقاً؟)
            var progress = await _progressRepository.GetByStudentAndQuestionAsync(dto.StudentId, dto.QuestionId);

            bool isNewProgress = false;

            // إذا كان هذا السؤال يظهر للطالب لأول مرة
            if (progress == null)
            {
                isNewProgress = true;
                progress = new StudentQuestionProgress
                {
                    StudentId = dto.StudentId,
                    QuestionId = dto.QuestionId,
                    RepetitionLevel = 0,
                    EaseFactor = 2.5, // القيمة الافتراضية للخوارزمية
                    Interval = 0
                };
            }

            // لحساب الفاصل الزمني الحالي (إذا كان موجوداً مسبقاً)
            int currentInterval = progress.RepetitionLevel == 0 ? 0 : progress.Interval;

            // 🧠 3. تشغيل خوارزمية الذكاء الاصطناعي (SM-2)
            var nextState = SrsAlgorithm.CalculateNextState(
                currentRepetitions: progress.RepetitionLevel,
                currentEaseFactor: progress.EaseFactor,
                currentInterval: currentInterval,
                quality: dto.Quality // التقييم الحقيقي من 0 إلى 5
            );

            // 💾 4. تحديث سجل التقدم بالنتائج الجديدة
            progress.RepetitionLevel = nextState.Repetitions;
            progress.EaseFactor = nextState.EaseFactor;
            progress.Interval = nextState.Interval;
            progress.NextReviewDate = nextState.NextReviewDate;
            progress.LastReviewedAt = DateTime.UtcNow;
            progress.TotalAttempts += 1;

            // جودة 3 أو أكثر تعني إجابة صحيحة/مقبولة
            if (dto.Quality >= 3)
            {
                progress.CorrectAttempts += 1;
            }

            // 🗄️ 5. حفظ التقدم في قاعدة البيانات
            if (isNewProgress)
            {
                await _progressRepository.AddAsync(progress);
            }
            else
            {
                await _progressRepository.UpdateAsync(progress);
            }

            // 📊 6. توثيق الإجابة كبيانات تدريب للمستقبل (Machine Learning Logs)
            var answerLog = new StudentAnswerDetailCreateDto
            {
                StudentId = dto.StudentId,
                QuestionId = dto.QuestionId,
                SelectedAnswer = dto.SelectedAnswer ?? string.Empty,
                IsCorrect = dto.Quality >= 3,
                Quality = dto.Quality,
                TimeTakenInSeconds = dto.TimeTakenInSeconds
            };

            await _answerDetailService.CreateAsync(answerLog);
        }

        public async Task<IEnumerable<StudentQuestionProgressReadDto>> GetDueQuestionsForSessionAsync(int studentId, DateTime? currentDate)
        {
            // جلب الأسئلة التي موعد مراجعتها هو اليوم أو في الماضي
            // نفترض أن الـ Repository يحتوي على دالة GetDueQuestionsAsync
            // 🌟 السطر السحري:
            // إذا أرسل المبرمج (currentDate) من الـ Swagger، نستخدمه.
            // وإذا لم يرسله (كان null)، نستخدم وقت السيرفر الحقيقي لحماية النظام.
            var targetDate = currentDate ?? DateTime.UtcNow;
            var dueQuestions = await _progressRepository.GetDueQuestionsAsync(studentId, targetDate);

            return _mapper.Map<IEnumerable<StudentQuestionProgressReadDto>>(dueQuestions);
        }
    }
}







//using AutoMapper;
//using FluentValidation; // 👈 1. إضافة مسار الـ Validation
//using SchoolLearningSystem.Applicationf.DTOs.Srs;
//using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
//using SchoolLearningSystem.Applicationf.Exceptions;
//using SchoolLearningSystem.Applicationf.Interfaces;
//using SchoolLearningSystem.Domain.Interfaces;
//using SchoolLearningSystem.Domain.Services;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace SchoolLearningSystem.Applicationf.Services
//{
//    // لاحظ: لا ترث من BaseService، فهي خدمة Business Logic بحتة
//    public class SrsService : ISrsService
//    {
//        private readonly IStudentQuestionProgressRepository _progressRepository;
//        private readonly IMapper _mapper;
//        private readonly IValidator<AnswerSubmissionDto> _answerValidator; // 👈 2. تعريف الـ Validator

//        public SrsService(
//            IStudentQuestionProgressRepository progressRepository,
//            IMapper mapper,
//            IValidator<AnswerSubmissionDto> answerValidator) // 👈 3. حقن الـ Validator
//        {
//            _progressRepository = progressRepository;
//            _mapper = mapper;
//            _answerValidator = answerValidator;
//        }

//        // تستقبل DTO التفاعل الخاص بالطالب
//        public async Task ProcessAnswerAsync(AnswerSubmissionDto dto)
//        {
//            // 👈 4. إجراء الفحص (Validation) قبل فعل أي شيء
//            var validationResult = await _answerValidator.ValidateAsync(dto);
//            if (!validationResult.IsValid)
//            {
//                // إذا فشل الفحص، نرمي خطأ يلتقطه الـ Middleware
//                throw new CustomValidationException(validationResult.Errors);
//            }

//            // 1. جلب السجل
//            var progress = await _progressRepository.GetByStudentAndQuestionAsync(dto.StudentId, dto.QuestionId)
//                ?? throw new NotFoundException($"Progress record for Student {dto.StudentId} and Question {dto.QuestionId} not found.");

//            // 2. تحديث الإحصائيات الأولية
//            progress.TotalAttempts++;
//            if (dto.IsCorrect) progress.CorrectAttempts++;
//            progress.LastReviewedAt = DateTime.UtcNow;

//            // 3. تطبيق خوارزمية التكرار المتباعد
//            var newState = SrsAlgorithm.CalculateNextState(progress.RepetitionLevel, progress.EaseFactor, progress.Interval, dto.IsCorrect ? 4 : 0); // تعديل بسيط ليتوافق مع SM-2

//            progress.RepetitionLevel = newState.Repetitions;
//            progress.EaseFactor = newState.EaseFactor;
//            progress.Interval = newState.Interval;
//            progress.NextReviewDate = newState.NextReviewDate;

//            // 4. الحفظ
//            await _progressRepository.UpdateAsync(progress);
//            //await _progressRepository.SaveChangesAsync(); // في حال كان الريبو لا يحفظ تلقائياً
//        }

//        public async Task<IEnumerable<StudentQuestionProgressReadDto>> GetDueQuestionsForSessionAsync(int studentId)
//        {
//            var dueQuestions = await _progressRepository.GetDueQuestionsAsync(studentId);
//            return _mapper.Map<IEnumerable<StudentQuestionProgressReadDto>>(dueQuestions);
//        }
//    }
//}





