using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Question;
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
    // يدير الامتحانات (Exam) وعلاقاتها بالأسئلة والنتائج والدروس. بما أن Exam مرتبط
    // بـ 3 كيانات مختلفة (Course, Lesson, Question/Result)، نحتاج حقن ريبوهات متعددة.
    // ==================================================================================
    public class ExamService : BaseService<Exam, ExamReadDto, ExamCreateDto, ExamUpdateDto>, IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IResultRepository _resultRepository;

        public ExamService(
            IExamRepository examRepository,
            ICourseRepository courseRepository,
            ILessonRepository lessonRepository,
            IQuestionRepository questionRepository,
            IResultRepository resultRepository,
            IMapper mapper)
            : base(examRepository, mapper)
        {
            _examRepository = examRepository;
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
            _questionRepository = questionRepository;
            _resultRepository = resultRepository;
        }

        // ============================================================================
        // 🎯 Use Case: "المعلم ينشئ امتحاناً جديداً لكورسه، إما شامل للفصل أو خاص بدرس معيّن"
        // ============================================================================
        public override async Task<ExamReadDto> CreateAsync(ExamCreateDto dto)
        {
            // 1. تحقق من وجود الكورس (علاقة إجبارية دائماً) - تحقق فقط، بدون الاحتفاظ بالكيان
            var courseExists = await _courseRepository.GetByIdAsync(dto.CourseId)
                ?? throw new NotFoundException($"الكورس برقم {dto.CourseId} غير موجود.");

            // 2. تحقق من وجود الدرس فقط إذا تم تحديده (علاقة اختيارية - LessonId هو int?)
            if (dto.LessonId.HasValue)
            {
                var lessonExists = await _lessonRepository.GetByIdAsync(dto.LessonId.Value)
                    ?? throw new NotFoundException($"الدرس برقم {dto.LessonId.Value} غير موجود.");
            }

            // 3. التحويل من DTO إلى Entity
            var entity = _mapper.Map<Exam>(dto);

            // 4. 💡 نربط بالـ Foreign Key ID مباشرة (وليس بالكيان الكامل Navigation Property)
            //    هذا يتجنب خطر Duplicate Insert الناتج عن حالة Detached للكيانات المجلوبة
            //    عبر AsNoTracking من ريبوهات أخرى.
            entity.CourseId = dto.CourseId;
            entity.LessonId = dto.LessonId;

            // 5. الحفظ + تثبيت التغييرات
            //    💡 Override كامل (لا يستدعي base.CreateAsync) - لازم نستدعي
            //       SaveChangesAsync بأنفسنا هنا وإلا التغييرات لا تُحفظ فعلياً.
            await _examRepository.AddAsync(entity);
            await _examRepository.SaveChangesAsync();

            return _mapper.Map<ExamReadDto>(entity);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يبدأ امتحاناً فيحتاج النظام يجيب له كل أسئلته"
        // نستخدم IQuestionRepository المخصص بدل exam.Questions (Navigation Property
        // فاضية دائماً لأن GetByIdAsync لا يستخدم Include).
        // ============================================================================
        public async Task<IEnumerable<QuestionReadDto>> GetQuestionsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId)
                ?? throw new NotFoundException($"الامتحان برقم {examId} غير موجود.");

            var questions = await _questionRepository.GetByExamIdAsync(examId);
            return _mapper.Map<IEnumerable<QuestionReadDto>>(questions);
        }

        // ============================================================================
        // 🎯 Use Case: "المعلم يفتح تقرير امتحان معيّن ليشوف نتائج كل الطلاب اللي أدّوه"
        // ============================================================================
        public async Task<IEnumerable<ResultReadDto>> GetResultsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId)
                ?? throw new NotFoundException($"الامتحان برقم {examId} غير موجود.");

            var results = await _resultRepository.GetByExamIdAsync(examId);
            return _mapper.Map<IEnumerable<ResultReadDto>>(results);
        }

        // ============================================================================
        // 🎯 Use Case: "من صفحة الامتحان، نبي نعرض للطالب اسم الدرس المرتبط فيه (إن وُجد)"
        //
        // 💡 نرجع null (وليس استثناء) إذا كان الامتحان "شاملاً" وغير مرتبط بدرس -
        //    هذه حالة عمل طبيعية. الاستثناء يُرمى فقط إذا examId نفسه غير موجود.
        // ============================================================================
        public async Task<LessonReadDto?> GetLessonByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId)
                ?? throw new NotFoundException($"الامتحان برقم {examId} غير موجود.");

            if (!exam.LessonId.HasValue)
                return null;

            var lesson = await _lessonRepository.GetByIdAsync(exam.LessonId.Value);
            return _mapper.Map<LessonReadDto?>(lesson);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يفتح درساً معيّناً فيشوف كل الامتحانات/الاختبارات
        //              القصيرة (Quiz) المرتبطة بهذا الدرس تحديداً"
        //
        // مين يستدعيها: صفحة الدرس بواجهة الطالب.
        // ============================================================================
        public async Task<IEnumerable<ExamReadDto>> GetExamsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new NotFoundException($"الدرس برقم {lessonId} غير موجود.");

            var exams = await _examRepository.GetByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<ExamReadDto>>(exams);
        }

        // ============================================================================
        // 🎯 Use Case: "عداد بلوحة تحكم المعلم: (هذا الدرس فيه 3 اختبارات قصيرة)"
        // نستخدم COUNT مباشر بقاعدة البيانات بدل جلب القائمة كاملة وعدّها.
        // ============================================================================
        public async Task<int> GetTotalExamsByLessonIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new NotFoundException($"الدرس برقم {lessonId} غير موجود.");

            return await _examRepository.GetTotalExamsByLessonIdAsync(lessonId);
        }
    }
}