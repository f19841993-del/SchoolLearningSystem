using AutoMapper;
using FluentValidation;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    // ==================================================================================
    // 📌 دور هذا الـ Service:
    // يدير بيانات "الكورس" نفسه (إنشاء، تعديل، حذف منطقي) بالإضافة لعمليات تجمع بينه
    // وبين الدروس/الامتحانات التابعة له. تسجيل/إزالة الطلاب مسؤولية ICourseStudentService
    // حصرياً (مصدر واحد للحقيقة، لا نكررها هنا).
    // ==================================================================================
    public class CourseService : BaseService<Course, CourseReadDto, CourseCreateDto, CourseUpdateDto>, ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IExamRepository _examRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly IValidator<CourseCreateDto> _createValidator;
        private readonly IValidator<CourseUpdateDto> _updateValidator;

        public CourseService(
            ICourseRepository courseRepository,
            ILessonRepository lessonRepository,
            IExamRepository examRepository,
            ICourseStudentRepository courseStudentRepository,
            IMapper mapper,
            IValidator<CourseUpdateDto> updateValidator,
            IValidator<CourseCreateDto> createValidator)
            : base(courseRepository, mapper)
        {
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
            _examRepository = examRepository;
            _courseStudentRepository = courseStudentRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        #region تجاوز العمليات الأساسية (Overriding Base Operations)

        // ============================================================================
        // 🎯 Use Case: "المعلم أو الإدارة تنشئ كورساً رياضياً جديداً بالمنصة، والنظام
        //              يتحقق من صحة البيانات المدخلة قبل الحفظ (عنوان غير فاضي، Order
        //              صحيح، TeacherId/CurriculumId موجودين... حسب قواعد الـ Validator)"
        //
        // مين يستدعيها: CourseController عند استقبال طلب POST لإنشاء كورس جديد.
        //
        // 💡 Override كامل لـ base.CreateAsync لإضافة خطوة التحقق (FluentValidation)
        //    قبل تفويض باقي العملية (Mapping + Add + SaveChanges) للأب.
        // ============================================================================
        public override async Task<CourseReadDto> CreateAsync(CourseCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new CustomValidationException(validationResult.Errors);

            return await base.CreateAsync(dto);
        }

        // ============================================================================
        // 🎯 Use Case: "الإدارة تقرر أرشفة/حذف كورس بسبب انتهاء صلاحيته أو وجود خطأ"
        //
        // القصة الكاملة خطوة بخطوة:
        //   1. نتأكد أن الكورس موجود أصلاً (مو Id وهمي).
        //   2. نتحقق: هل يوجد طلاب مسجلين فعلياً بهذا الكورس؟ إذا نعم، نمنع الحذف
        //      (قرار عمل: لا نحذف كورساً فيه طلاب نشطون بدون معالجة اشتراكاتهم أولاً).
        //   3. إذا الفحص نجح، نخفي الكورس منطقياً (Soft Delete). الدروس والامتحانات
        //      التابعة له تُخفى تلقائياً عبر Global Query Filters (بدون تعديل يدوي).
        //
        // مين يستدعيها: الأدمن من لوحة تحكم إدارة المحتوى.
        // ============================================================================
        public override async Task DeleteAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
                throw new NotFoundException("الكورس غير موجود.");

            var enrolledCount = await _courseStudentRepository.CountByCourseIdAsync(id);
            if (enrolledCount > 0)
                throw new BadRequestException("لا يمكن حذف الكورس لوجود طلاب مسجلين.");

            course.IsDeleted = true;
            await _courseRepository.UpdateAsync(course);
            await _courseRepository.SaveChangesAsync();
        }

        // ============================================================================
        // 🎯 Use Case: "المعلم يقوم بتصحيح خطأ إملائي بعنوان الكورس أو تعديل وصفه،
        //              والنظام يتحقق من صحة التعديلات قبل حفظها"
        //
        // مين يستدعيها: CourseController عند استقبال طلب PUT/PATCH لتعديل كورس قائم.
        // ============================================================================
        public override async Task UpdateAsync(int id, CourseUpdateDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new CustomValidationException(validationResult.Errors);

            await base.UpdateAsync(id, dto);
        }

        #endregion

        #region العمليات المخصصة للكورس (Specific Course Operations)
        // 💡 ملاحظة: تسجيل/إزالة الطلاب وجلب قائمتهم أصبحت مسؤولية ICourseStudentService
        // حصرياً، لتفادي تكرار المنطق بمكانين (راجع النقاش الخاص بـ CourseStudentService).

        // ============================================================================
        // 🎯 Use Case: "الطالب يفتح صفحة الكورس ليرى قائمة الدروس المتاحة للتعلم،
        //              مرتبة بتسلسلها الصحيح"
        //
        // مين يستدعيها: صفحة تفاصيل الكورس بواجهة الطالب.
        // ============================================================================
        public async Task<IEnumerable<LessonReadDto>> GetLessonsByCourseIdAsync(int courseId)
        {
            var lessons = await _lessonRepository.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<LessonReadDto>>(lessons);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يبحث عن الاختبارات المتاحة داخل الكورس لاختبار مستواه
        //              قبل أو بعد إنهاء الدروس"
        //
        // مين يستدعيها: صفحة تفاصيل الكورس بواجهة الطالب (تبويب "الاختبارات").
        // ============================================================================
        public async Task<IEnumerable<ExamReadDto>> GetExamsByCourseIdAsync(int courseId)
        {
            var exams = await _examRepository.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<ExamReadDto>>(exams);
        }

        #endregion
    }
}