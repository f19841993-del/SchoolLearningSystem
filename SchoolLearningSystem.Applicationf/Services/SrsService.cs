using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Srs;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    // ==================================================================================
    // 📌 المصدر الوحيد بكل المشروع لتطبيق خوارزمية SM-2 (Spaced Repetition System).
    // يستبدل StudentQuestionProgressService القديمة بالكامل - احذفها من مشروعك.
    // ==================================================================================
    public class SrsService : ISrsService
    {
        private readonly IStudentQuestionProgressRepository _progressRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        private const double MinimumEaseFactor = 1.3;

        public SrsService(
            IStudentQuestionProgressRepository progressRepository,
            IStudentRepository studentRepository,
            IQuestionRepository questionRepository,
            IMapper mapper)
        {
            _progressRepository = progressRepository;
            _studentRepository = studentRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        // ============================================================================
        // 🎯 Use Case: الطالب يجاوب على سؤال بجلسة المراجعة، والنظام يحسب تلقائياً
        //              موعد مراجعته القادم حسب خوارزمية SM-2
        // ============================================================================
        public async Task ProcessAnswerAsync(AnswerSubmissionDto dto)
        {
            if (dto.Quality < 0 || dto.Quality > 5)
                throw new BadRequestException("قيمة الجودة (Quality) يجب أن تكون بين 0 و 5.");

            var studentExists = await _studentRepository.GetByIdAsync(dto.StudentId)
                ?? throw new NotFoundException($"الطالب برقم {dto.StudentId} غير موجود.");

            var questionExists = await _questionRepository.GetByIdAsync(dto.QuestionId)
                ?? throw new NotFoundException($"السؤال برقم {dto.QuestionId} غير موجود.");

            var progress = await _progressRepository.GetByStudentAndQuestionAsync(dto.StudentId, dto.QuestionId);
            var isNewProgress = progress == null;

            progress ??= new StudentQuestionProgress
            {
                StudentId = dto.StudentId,
                QuestionId = dto.QuestionId,
                RepetitionLevel = 0,
                EaseFactor = 2.5,
                Interval = 0
            };

            if (dto.Quality < 3)
            {
                progress.RepetitionLevel = 0;
                progress.Interval = 1;
            }
            else
            {
                progress.Interval = progress.RepetitionLevel switch
                {
                    0 => 1,
                    1 => 6,
                    _ => (int)Math.Round(progress.Interval * progress.EaseFactor)
                };
                progress.RepetitionLevel++;
            }

            var newEaseFactor = progress.EaseFactor
                + (0.1 - (5 - dto.Quality) * (0.08 + (5 - dto.Quality) * 0.02));
            progress.EaseFactor = Math.Max(newEaseFactor, MinimumEaseFactor);

            progress.NextReviewDate = DateTime.UtcNow.AddDays(progress.Interval);
            progress.LastReviewedAt = DateTime.UtcNow;
            progress.TotalAttempts++;
            if (dto.Quality >= 3)
                progress.CorrectAttempts++;

            if (isNewProgress)
                await _progressRepository.AddAsync(progress);
            else
                await _progressRepository.UpdateAsync(progress);
        }

        // ============================================================================
        // 🎯 Use Case: بناء جلسة مراجعة جديدة - جلب كل الأسئلة المستحقة الآن
        // ============================================================================
        public async Task<IEnumerable<StudentQuestionProgressReadDto>> GetDueQuestionsForSessionAsync(
            int studentId, DateTime? currentDate)
        {
            var studentExists = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            var effectiveDate = currentDate ?? DateTime.UtcNow;
            var dueQuestions = await _progressRepository.GetDueQuestionsAsync(studentId, effectiveDate);
            return _mapper.Map<IEnumerable<StudentQuestionProgressReadDto>>(dueQuestions);
        }

        // ============================================================================
        // 🎯 Use Case: تقرير شامل لمستوى الطالب بكل سؤال سبق وتدرّب عليه
        // ============================================================================
        public async Task<IEnumerable<StudentQuestionProgressReadDto>> GetProgressByStudentIdAsync(int studentId)
        {
            var studentExists = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            var progress = await _progressRepository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<StudentQuestionProgressReadDto>>(progress);
        }

        // ============================================================================
        // 🎯 Use Case: معرفة مستوى إتقان الطالب لسؤال معيّن بالتحديد
        // ============================================================================
        public async Task<StudentQuestionProgressReadDto?> GetProgressByStudentAndQuestionAsync(int studentId, int questionId)
        {
            var progress = await _progressRepository.GetByStudentAndQuestionAsync(studentId, questionId);
            return _mapper.Map<StudentQuestionProgressReadDto?>(progress);
        }
    }
}