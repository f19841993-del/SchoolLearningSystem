using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    // ==================================================================================
    // 📌 دور هذا الـ Service:
    // يدير أدق مستوى من بيانات أداء الطالب - "كل إجابة على كل سؤال" مع جودتها (Quality)
    // والوقت المستغرق. هذا الجدول هو "الوقود الخام" الذي تُبنى منه كل تحليلات الذكاء
    // الاصطناعي (خوارزمية SM-2 للتكرار المتباعد، تحديد نقاط الضعف، تصنيف صعوبة الأسئلة).
    // ==================================================================================
    public class StudentAnswerDetailService
        : BaseService<StudentAnswerDetail, StudentAnswerDetailReadDto, StudentAnswerDetailCreateDto, StudentAnswerDetailUpdateDto>,
          IStudentAnswerDetailService
    {
        private readonly IStudentAnswerDetailRepository _answerRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IQuestionRepository _questionRepository;

        public StudentAnswerDetailService(
            IStudentAnswerDetailRepository answerRepository,
            IStudentRepository studentRepository,
            IQuestionRepository questionRepository,
            IMapper mapper)
            : base(answerRepository, mapper)
        {
            _answerRepository = answerRepository;
            _studentRepository = studentRepository;
            _questionRepository = questionRepository;
        }

        // 🔹 CRUD الأساسي موروث من BaseService
        // 💡 تسجيل إجابة جديدة (بعد كل سؤال بجلسة المراجعة) يتم عبر CreateAsync
        //    الموروثة مباشرة - لا حاجة لدالة "RecordAnswerAsync" مخصصة هنا، لأن ما فيه
        //    منطق تحقق إضافي يستدعي تجاوزها (الفحوصات الأساسية تكفي).

        // ============================================================================
        // 🎯 Use Case: "محرك الذكاء الاصطناعي يبني (ملف تعلّم) كامل للطالب بتحليل
        //              كل إجابة سبق وأعطاها، عشان يحدد نقاط قوته وضعفه بدقة"
        // ============================================================================
        public async Task<IEnumerable<StudentAnswerDetailReadDto>> GetAnswersByStudentIdAsync(int studentId)
        {
            var studentExists = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            var answers = await _answerRepository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<StudentAnswerDetailReadDto>>(answers);
        }

        // ============================================================================
        // 🎯 Use Case: "الأدمن أو المعلم يحلل سؤالاً معيّناً: هل أغلب الطلاب يجاوبون
        //              عليه خطأ؟ ربما السؤال مصاغ بشكل غامض ويحتاج مراجعة"
        // ============================================================================
        public async Task<IEnumerable<StudentAnswerDetailReadDto>> GetAnswersByQuestionIdAsync(int questionId)
        {
            var questionExists = await _questionRepository.GetByIdAsync(questionId)
                ?? throw new NotFoundException($"السؤال برقم {questionId} غير موجود.");

            var answers = await _answerRepository.GetByQuestionIdAsync(questionId);
            return _mapper.Map<IEnumerable<StudentAnswerDetailReadDto>>(answers);
        }

        // ============================================================================
        // 🎯 Use Case: "لوحة تحكم الطالب تعرض (آخر 10 إجابات) بشكل رسم بياني صاعد
        //              أو نازل، يوضح للطالب هل هو يتحسن أو يتراجع مؤخراً"
        // ============================================================================
        public async Task<IEnumerable<StudentAnswerDetailReadDto>> GetRecentAnswersByStudentIdAsync(int studentId, int count)
        {
            var studentExists = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            var answers = await _answerRepository.GetRecentAnswersAsync(studentId, count);
            return _mapper.Map<IEnumerable<StudentAnswerDetailReadDto>>(answers);
        }

        // ============================================================================
        // 🎯 Use Case: "النظام الذكي يبني (جلسة تدريب مكثف) للطالب على الدرس اللي
        //              يتعثر فيه، بجمع كل الأسئلة اللي جاوب عليها خطأ سابقاً فيه"
        //
        // مين يستدعيها: محرك الـ AI عند بناء جلسة مراجعة مخصصة (Remedial Session).
        // ============================================================================
        public async Task<IEnumerable<StudentAnswerDetailReadDto>> GetIncorrectAnswersByStudentIdAsync(int studentId, int lessonId)
        {
            var studentExists = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            var answers = await _answerRepository.GetIncorrectAnswersByStudentIdAsync(studentId, lessonId);
            return _mapper.Map<IEnumerable<StudentAnswerDetailReadDto>>(answers);
        }
    }
}