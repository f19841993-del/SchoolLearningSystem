using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Srs;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class SrsService : ISrsService
    {
        private readonly IStudentQuestionProgressRepository _progressRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IStudentAnswerDetailRepository _answerRepository;   // 🆕
        private readonly IMemorizeRepository _memorizeRepository;             // 🆕
        private readonly IMapper _mapper;

        private const double MinimumEaseFactor = 1.3;

        public SrsService(
            IStudentQuestionProgressRepository progressRepository,
            IStudentRepository studentRepository,
            IQuestionRepository questionRepository,
            IStudentAnswerDetailRepository answerRepository,   // 🆕
            IMemorizeRepository memorizeRepository,             // 🆕
            IMapper mapper)
        {
            _progressRepository = progressRepository;
            _studentRepository = studentRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _memorizeRepository = memorizeRepository;
            _mapper = mapper;
        }

        // ============================================================================
        // 🎯 Use Case: الطالب يجاوب على سؤال بجلسة المراجعة - الآن ينشئ StudentAnswerDetail
        //              + يحدّث SM-2 + يحدّث إحصائيات الجلسة، كل هذا بنداء واحد.
        // ============================================================================
        public async Task ProcessAnswerAsync(AnswerSubmissionDto dto)
        {
            if (dto.Quality < 0 || dto.Quality > 5)
                throw new BadRequestException("قيمة الجودة (Quality) يجب أن تكون بين 0 و 5.");

            var studentExists = await _studentRepository.GetByIdAsync(dto.StudentId)
                ?? throw new NotFoundException($"الطالب برقم {dto.StudentId} غير موجود.");

            var questionExists = await _questionRepository.GetByIdAsync(dto.QuestionId)
                ?? throw new NotFoundException($"السؤال برقم {dto.QuestionId} غير موجود.");

            // 🆕 تحقق كان مفقوداً بالكامل من قبل
            var session = await _memorizeRepository.GetByIdAsync(dto.MemorizeSessionId)
                ?? throw new NotFoundException($"جلسة المراجعة برقم {dto.MemorizeSessionId} غير موجودة.");

            bool isCorrect = dto.Quality >= 3;   // 🆕 محسوبة من السيرفر، مو من الفرونت

            // --- 1) SM-2 (نفس المنطق الأصلي حرفياً، بدون أي تغيير) ---
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
            if (isCorrect)
                progress.CorrectAttempts++;

            if (isNewProgress)
                await _progressRepository.AddAsync(progress);   // 💡 لا يحفظ فوراً (نفس نمط GenericRepository)
            else
                await _progressRepository.UpdateAsync(progress);

            // --- 2) 🆕 إنشاء سجل الإجابة التفصيلية ---
            var answerDetail = new StudentAnswerDetail
            {
                StudentId = dto.StudentId,
                QuestionId = dto.QuestionId,
                MemorizeSessionId = dto.MemorizeSessionId,
                SelectedAnswer = dto.SelectedAnswer,
                IsCorrect = isCorrect,
                Quality = dto.Quality,
                TimeTakenInSeconds = dto.TimeTakenInSeconds ?? 0
            };
            await _answerRepository.AddAsync(answerDetail);   // 💡 لا يحفظ فوراً (GenericRepository)

            // --- 3) 🆕 تحديث إحصائيات الجلسة تراكمياً ---
            session.TotalAttempts++;
            var correctSoFar = (int)Math.Round(session.SuccessRate * (session.TotalAttempts - 1) / 100.0)
                + (isCorrect ? 1 : 0);
            session.SuccessRate = (double)correctSoFar / session.TotalAttempts * 100;

            await _memorizeRepository.UpdateAsync(session);   // 💡 لا يحفظ فوراً (GenericRepository)

            // ✅ حفظ وحيد يغطي progress + answerDetail + session سوا (نفس الـ DbContext الـ Scoped = معاملة واحدة ذرية)
            await _progressRepository.SaveChangesAsync();
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

        // ============================================================================
        // 🎯 Use Case: "النظام يبني جلسة تدريب مكثف لدرس تعثّر فيه الطالب — يحتاج يعرف
        //              بالضبط أي أسئلة أخطأ فيها سابقاً بهذا الدرس تحديداً"
        // ============================================================================
        public async Task<IEnumerable<StudentQuestionProgressReadDto>> GetWeakQuestionsForLessonAsync(int studentId, int lessonId)
        {
            var studentExists = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            var incorrectAnswers = await _answerRepository.GetIncorrectAnswersByStudentIdAsync(studentId, lessonId);
            var weakQuestionIds = incorrectAnswers.Select(a => a.QuestionId).Distinct().ToList();

            if (!weakQuestionIds.Any())
                return Enumerable.Empty<StudentQuestionProgressReadDto>();

            var progress = await _progressRepository.GetByStudentAndQuestionIdsAsync(studentId, weakQuestionIds);
            return _mapper.Map<IEnumerable<StudentQuestionProgressReadDto>>(progress);
        }
    }
}