using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    // ==================================================================================
    // 📌 دور هذا الـ Service:
    // يدير "نتائج" الطلاب (Result) - وهي سجل كل درجة يحصل عليها الطالب سواء من
    // امتحان رسمي (ExamId) أو نشاط ضمن درس (LessonId). هذا الكيان هو المصدر الرئيسي
    // لتغذية الذكاء الاصطناعي بأداء الطالب عبر الزمن (Progress Tracking).
    // ==================================================================================
    public class ResultService
        : BaseService<Result, ResultReadDto, ResultCreateDto, ResultUpdateDto>, IResultService
    {
        private readonly IResultRepository _resultRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IExamRepository _examRepository;
        private readonly ICourseStudentService _courseStudentService;
        private readonly ILogger<ResultService> _logger;

        public ResultService(
            IResultRepository resultRepository,
            IStudentRepository studentRepository,
            ILessonRepository lessonRepository,
            IExamRepository examRepository,
            ICourseStudentService courseStudentService,
            ILogger<ResultService> logger,
            IMapper mapper)
            : base(resultRepository, mapper)
        {
            _resultRepository = resultRepository;
            _studentRepository = studentRepository;
            _lessonRepository = lessonRepository;
            _examRepository = examRepository;
            _courseStudentService = courseStudentService;
            _logger = logger;

        }

        // ============================================================================
        // 🎯 Use Case: "النظام يسجّل نتيجة جديدة لطالب - إما نتيجة درس أو نتيجة امتحان
        //              رسمي، لكن ليس نتيجة (معلّقة) بلا أي سياق تعليمي"
        //
        // 💡 Override كامل لإضافة قاعدة عمل: يجب أن ترتبط أي نتيجة بدرس أو امتحان
        //    (أو الاثنين معاً) على الأقل - قاعدة البيانات وحدها لا تفرض هذا لأن
        //    كلا الحقلين Nullable. بعد التحقق، نفوّض الباقي (Mapping + Add + Save) للأب.
        // ============================================================================
        public override async Task<ResultReadDto> CreateAsync(ResultCreateDto dto)
        {
            // 1. قاعدة العمل الأساسية
            if (!dto.LessonId.HasValue && !dto.ExamId.HasValue)
                throw new BadRequestException("يجب أن ترتبط النتيجة بدرس أو امتحان على الأقل.");

            // 2. التحقق من وجود الطالب
            var student = await _studentRepository.GetByIdAsync(dto.StudentId)
                ?? throw new NotFoundException($"الطالب برقم {dto.StudentId} غير موجود.");

            // 3. التحقق من وجود الدرس (إذا أُرسل)
            if (dto.LessonId.HasValue)
            {
                var lesson = await _lessonRepository.GetByIdAsync(dto.LessonId.Value)
                    ?? throw new NotFoundException($"الدرس برقم {dto.LessonId.Value} غير موجود.");
            }

            // 4. التحقق من وجود الامتحان (إذا أُرسل)
            if (dto.ExamId.HasValue)
            {
                var exam = await _examRepository.GetByIdAsync(dto.ExamId.Value)
                    ?? throw new NotFoundException($"الامتحان برقم {dto.ExamId.Value} غير موجود.");
            }

            // 5. التنفيذ
            var created = await base.CreateAsync(dto);

            // 6. لو النتيجة مرتبطة بدرس، حدّث تقدّم الطالب — عملية ثانوية مشتقة (Derived)
            //    لا يجوز تفشيل حفظ الدرجة الأساسية بسببها. لو صار خطأ هنا، التقدّم يبقى
            //    "متأخر شوي" مؤقتاً ويتصحح تلقائياً بأول Result جاي لنفس الكورس
            //    (لأن الحساب يُعاد من الصفر كل مرة، مو تراكمي).
            if (dto.LessonId.HasValue)
            {
                try
                {
                    var lesson = await _lessonRepository.GetByIdAsync(dto.LessonId.Value);
                    if (lesson != null)
                        await _courseStudentService.UpdateProgressAsync(dto.StudentId, lesson.CourseId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "فشل تحديث تقدّم الطالب {StudentId} بالكورس بعد تسجيل نتيجة الدرس {LessonId}", dto.StudentId, dto.LessonId);
                }
            }

            return created;
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يفتح صفحة (سجلي الدراسي) ليشوف كل درجاته السابقة
        //              بكل الامتحانات والدروس، مرتبة من الأحدث للأقدم"
        // ============================================================================
        public async Task<IEnumerable<ResultReadDto>> GetResultsByStudentIdAsync(int studentId)
        {
            var studentExists = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            var results = await _resultRepository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<ResultReadDto>>(results);
        }

        // ============================================================================
        // 🎯 Use Case: "المعلم يفتح تقرير درس معيّن ليشوف درجات كل الطلاب فيه،
        //              عشان يعرف هل الدرس كان سهلاً أو صعبًا على أغلبهم"
        // ============================================================================
        public async Task<IEnumerable<ResultReadDto>> GetResultsByLessonIdAsync(int lessonId)
        {
            var lessonExists = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new NotFoundException($"الدرس برقم {lessonId} غير موجود.");

            var results = await _resultRepository.GetByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<ResultReadDto>>(results);
        }

        // ============================================================================
        // 🎯 Use Case: "المعلم يفتح تقرير امتحان رسمي ليشوف درجات كل الطلاب اللي أدّوه"
        // ============================================================================
        public async Task<IEnumerable<ResultReadDto>> GetResultsByExamIdAsync(int examId)
        {
            var examExists = await _examRepository.GetByIdAsync(examId)
                ?? throw new NotFoundException($"الامتحان برقم {examId} غير موجود.");

            var results = await _resultRepository.GetByExamIdAsync(examId);
            return _mapper.Map<IEnumerable<ResultReadDto>>(results);
        }

        // ============================================================================
        // 🎯 Use Case: "النظام الذكي يحسب متوسط أداء الطالب العام لتحديد مستواه
        //              الإجمالي (مثلاً لعرض شارة (متفوق) أو تنبيه (يحتاج دعم))"
        // ============================================================================
        public async Task<double> GetAverageScoreByStudentIdAsync(int studentId)
        {
            var studentExists = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            return await _resultRepository.GetAverageScoreByStudentIdAsync(studentId);
        }

        // ============================================================================
        // 🎯 Use Case: "تقرير المعلم: (متوسط درجات الطلاب بهذا الدرس هو 72%) - يساعد
        //              المعلم يعرف إذا يحتاج يعيد شرح الدرس أو يبسّطه"
        // ============================================================================
        public async Task<double> GetAverageScoreByLessonIdAsync(int lessonId)
        {
            var lessonExists = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new NotFoundException($"الدرس برقم {lessonId} غير موجود.");

            return await _resultRepository.GetAverageScoreByLessonIdAsync(lessonId);
        }

        // ============================================================================
        // 🎯 Use Case: "تقرير المعلم: (متوسط درجات الامتحان النهائي هو 65%) - يساعد
        //              بتقييم مدى توازن صعوبة أسئلة الامتحان نفسه"
        // ============================================================================
        public async Task<double> GetAverageScoreByExamIdAsync(int examId)
        {
            var examExists = await _examRepository.GetByIdAsync(examId)
                ?? throw new NotFoundException($"الامتحان برقم {examId} غير موجود.");

            return await _resultRepository.GetAverageScoreByExamIdAsync(examId);
        }
    }
}