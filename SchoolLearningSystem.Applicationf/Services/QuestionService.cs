using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    // ==================================================================================
    // 📌 دور هذا الـ Service:
    // يدير "بنك الأسئلة" بالكامل - وهو أهم مصدر بيانات للذكاء الاصطناعي بالمشروع.
    // الأسئلة إما مرتبطة بامتحان محدد (ExamId != null) أو أسئلة عامة للتدريب الحر
    // على مستوى الدرس فقط (ExamId == null).
    // ==================================================================================
    public class QuestionService
        : BaseService<Question, QuestionReadDto, QuestionCreateDto, QuestionUpdateDto>, IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IExamRepository _examRepository;

        public QuestionService(
            IQuestionRepository questionRepository,
            ILessonRepository lessonRepository,
            IExamRepository examRepository,
            IMapper mapper)
            : base(questionRepository, mapper)
        {
            _questionRepository = questionRepository;
            _lessonRepository = lessonRepository;
            _examRepository = examRepository;
        }

        // 🔹 CRUD الأساسي (GetAll, GetById, Create, Update, Delete, GetPaged)
        // موروث تلقائياً من BaseService. ملاحظة: لو أردت لاحقاً override لـ CreateAsync
        // للتحقق من وجود Lesson/Exam قبل الإنشاء (نفس أسلوب ExamService.CreateAsync)،
        // استخدم نفس نمط الربط بالـ FK مباشرة (entity.LessonId, entity.ExamId).

        // ============================================================================
        // 🎯 Use Case: "الطالب يبدأ امتحاناً معيّناً، والنظام يجيب كل أسئلة هذا الامتحان
        //              بالتحديد ليعرضها له واحداً تلو الآخر"
        //
        // مين يستدعيها: ExamService.GetQuestionsByExamIdAsync (هذا هو المصدر الحقيقي
        //               للبيانات، بينما ExamService يتحقق فقط من وجود الامتحان نفسه
        //               قبل ما ينادي هذي الدالة).
        // ============================================================================
        public async Task<IEnumerable<QuestionReadDto>> GetQuestionsByExamIdAsync(int examId)
        {
            var examExists = await _examRepository.GetByIdAsync(examId)
                ?? throw new NotFoundException($"الامتحان برقم {examId} غير موجود.");

            var questions = await _questionRepository.GetByExamIdAsync(examId);
            return _mapper.Map<IEnumerable<QuestionReadDto>>(questions);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يفتح درساً ويريد يتدرب بأسئلة حرة (بدون ضغط وقت أو
        //              درجات) لمراجعة فهمه للدرس قبل الامتحان الرسمي"
        //
        // ⚠️ ملاحظة: هذي تجيب كل أسئلة الدرس (سواء مرتبطة بامتحان أو أسئلة عامة).
        //    لو تبي تجيب بس الأسئلة "العامة" (ExamId == null) للتدريب الحر تحديداً،
        //    فكر تضيف فلتر إضافي بالريبو (مثلاً GetGeneralQuestionsByLessonIdAsync).
        // ============================================================================
        public async Task<IEnumerable<QuestionReadDto>> GetQuestionsByLessonIdAsync(int lessonId)
        {
            var lessonExists = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new NotFoundException($"الدرس برقم {lessonId} غير موجود.");

            var questions = await _questionRepository.GetQuestionsByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<QuestionReadDto>>(questions);
        }

        // ============================================================================
        // 🎯 Use Case: "محرك الذكاء الاصطناعي يبني اختباراً تكيفياً (Adaptive Test):
        //              يبدأ بأسئلة سهلة، ولو الطالب متفوق يرفع مستوى الصعوبة تلقائياً"
        //
        // مين يستدعيها: خدمة الـ AI الداخلية (مثلاً AdaptiveLearningService مستقبلاً)
        //               لسحب مجموعة أسئلة بمستوى صعوبة محدد بناءً على أداء الطالب.
        // ============================================================================
        public async Task<IEnumerable<QuestionReadDto>> GetQuestionsByDifficultyAsync(DifficultyLevel difficulty)
        {
            var questions = await _questionRepository.GetByDifficultyAsync(difficulty);
            return _mapper.Map<IEnumerable<QuestionReadDto>>(questions);
        }

        // ============================================================================
        // 🎯 Use Case: "عداد بلوحة تحكم المعلم: (هذا الامتحان فيه 15 سؤال)"
        // ============================================================================
        public async Task<int> CountByExamIdAsync(int examId)
        {
            var examExists = await _examRepository.GetByIdAsync(examId)
                ?? throw new NotFoundException($"الامتحان برقم {examId} غير موجود.");

            return await _questionRepository.CountByExamIdAsync(examId);
        }

        // ============================================================================
        // 🎯 Use Case: "تقرير إداري: كم عدد الأسئلة (السهلة/المتوسطة/الصعبة) ببنك
        //              الأسئلة بالكامل؟ يفيد بتقييم توازن صعوبة المحتوى التعليمي"
        // ============================================================================
        public async Task<int> CountByDifficultyAsync(DifficultyLevel difficulty)
        {
            return await _questionRepository.CountByDifficultyAsync(difficulty);
        }

        // ============================================================================
        // 🎯 Use Case: "عداد بلوحة تحكم المعلم: (هذا الدرس فيه 20 سؤال بالكامل)"
        // ============================================================================
        public async Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId)
        {
            var lessonExists = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new NotFoundException($"الدرس برقم {lessonId} غير موجود.");

            return await _questionRepository.GetTotalQuestionsByLessonIdAsync(lessonId);
        }
    }
}