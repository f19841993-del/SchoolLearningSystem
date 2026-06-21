using AutoMapper;
using FluentValidation; // 1. أضفنا مكتبة التحقق
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class CourseService : BaseService<Course, CourseReadDto, CourseCreateDto, CourseUpdateDto>, ICourseService
    {
        // نحتفظ بنسخة خاصة من الـ Repository للوصول للعمليات المخصصة للكورس (مثل جلب طلاب الكورس)
        private readonly ICourseRepository _courseRepository;

        // 2. أضفنا الـ Validator الخاص بعملية الإنشاء لحماية السيرفيس
        private readonly IValidator<CourseCreateDto> _createValidator;
        // 1. إضافة حارس التعديل
        private readonly IValidator<CourseUpdateDto> _updateValidator;


        // التصميم الاحترافي: نمرر الـ Repository العام والـ Mapper إلى الكلاس الأب (BaseService)
        // ونحتفظ بالنسخ الخاصة بنا هنا
        public CourseService(
            ICourseRepository courseRepository,
            IMapper mapper,
             IValidator<CourseUpdateDto> updateValidator, // حقن الـ Validator الخاص بالتعديل
            IValidator<CourseCreateDto> createValidator) // حقن الـ Validator
            : base(courseRepository, mapper)
        {
            _courseRepository = courseRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        #region تجاوز العمليات الأساسية (Overriding Base Operations)

        // 🔹 تجاوز دالة الإضافة (CreateAsync) لتطبيق التحقق (Validation) قبل الحفظ
        public override async Task<CourseReadDto> CreateAsync(CourseCreateDto dto)
        {
            // أ. فحص البيانات القادمة باستخدام القواعد التي كتبناها في كلاس CourseCreateDtoValidator
            var validationResult = await _createValidator.ValidateAsync(dto);

            // ب. إذا كانت البيانات غير صالحة (مثلاً العنوان فارغ)، نوقف العملية فوراً
            if (!validationResult.IsValid)
            {
                // رمي الاستثناء الذي سيلتقطه الـ Global Middleware ويرجعه كـ 400 BadRequest
                throw new CustomValidationException(validationResult.Errors);
            }

            // ج. إذا كانت البيانات صحيحة، نترك الكلاس الأب (BaseService) يقوم بعملية الـ Mapping والحفظ
            return await base.CreateAsync(dto);
        }

        // 🔹 تجاوز دالة الحذف (DeleteAsync) لتطبيق قواعد العمل (Business Logic) والحذف المنطقي (Soft Delete)
        public override async Task DeleteAsync(int id)
        {
            // 1. التحقق من وجود الكورس أصلاً
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                throw new NotFoundException($"الكورس برقم {id} غير موجود.");
            }

            // 2. حماية البيانات: التحقق من عدم وجود طلاب مسجلين قبل الحذف
            var students = await _courseRepository.GetStudentsByCourseIdAsync(id);
            if (students != null && students.Any())
            {
                // رمي خطأ منطقي (Business Rule Exception)
                throw new BadRequestException("عذراً، لا يمكن حذف هذا الكورس لوجود طلاب مسجلين فيه.");
            }

            // 3. التنفيذ: تطبيق الحذف المنطقي (Soft Delete) بدلاً من الحذف النهائي
            course.IsDeleted = true;

            // 4. حفظ التعديل (Update) لتثبيت حالة IsDeleted = true
            await _courseRepository.UpdateAsync(course);
            await _courseRepository.SaveChangesAsync();

            // ملاحظة: لم نستدعِ base.DeleteAsync(id) لأننا لا نريد مسح السطر من قاعدة البيانات.
        }

        // 4. تجاوز دالة التعديل لفحص البيانات قبل حفظها
        public override async Task UpdateAsync(int id, CourseUpdateDto dto)
        {
            // أ. فحص بيانات التعديل
            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                // ب. رمي الخطأ المخصص إذا كانت البيانات خاطئة
                throw new CustomValidationException(validationResult.Errors);
            }

            // ج. إذا كانت البيانات سليمة، نترك الكلاس الأب يقوم بالبحث والتحديث
            await base.UpdateAsync(id, dto);
        }
        #endregion

        #region العمليات المخصصة للكورس (Specific Course Operations)

        public async Task<IEnumerable<StudentReadDto>> GetStudentsByCourseIdAsync(int courseId)
        {
            var students = await _courseRepository.GetStudentsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<StudentReadDto>>(students);
        }

        public async Task<IEnumerable<LessonReadDto>> GetLessonsByCourseIdAsync(int courseId)
        {
            var lessons = await _courseRepository.GetLessonsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<LessonReadDto>>(lessons);
        }

        public async Task<IEnumerable<ExamReadDto>> GetExamsByCourseIdAsync(int courseId)
        {
            var exams = await _courseRepository.GetExamsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<ExamReadDto>>(exams);
        }

        public async Task EnrollStudentAsync(int courseId, int studentId)
        {
            // يمكن مستقبلاً إضافة فحص هنا: هل الطالب مسجل بالفعل؟
            await _courseRepository.EnrollStudentAsync(courseId, studentId);
        }

        public async Task RemoveStudentAsync(int courseId, int studentId)
        {
            await _courseRepository.RemoveStudentAsync(courseId, studentId);
        }

        #endregion
    }
}