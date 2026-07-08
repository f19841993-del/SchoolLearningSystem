using AutoMapper;
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
    // يدير بيانات "الدرس" نفسه فقط (نشر، ترتيب، استرجاع). أي بيانات تخص كيانات أخرى
    // مرتبطة بالدرس (امتحانات، تمارين، أسئلة...) لها Service مستقل مسؤول عنها
    // (مثلاً ExamService.GetExamsByLessonIdAsync موجودة هناك أصلاً - لا نكررها هنا).
    // ==================================================================================
    public class LessonService : BaseService<Lesson, LessonReadDto, LessonCreateDto, LessonUpdateDto>, ILessonService
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonService(ILessonRepository lessonRepository, IMapper mapper)
            : base(lessonRepository, mapper)
        {
            _lessonRepository = lessonRepository;
        }

        // 🔹 CRUD الأساسي (GetAll, GetById, Create, Update, Delete, GetPaged)
        // موروث تلقائياً من BaseService

        // ============================================================================
        // 🎯 Use Case: "المعلم يحذف درساً بالخطأ، ثم يرجع بسرعة يسترجعه بضغطة واحدة"
        //
        // كيف تشتغل: تستخدم RestoreAsync الموروثة من IGenericRepository (تعيد
        // IsDeleted إلى false)، وتُسجَّل هنا كدالة Business واضحة الاسم للمعلم.
        // ============================================================================
        public async Task RestoreLessonAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            // ملاحظة: GetByIdAsync يستبعد المحذوف منطقياً، فلو رجع null هنا فعلاً
            // غير موجود أصلاً (مو بالضرورة محذوف) - نتحقق أيضاً بحالة الحذف مباشرة:
            if (lesson == null)
                throw new NotFoundException($"الدرس برقم {lessonId} غير موجود.");

            await _lessonRepository.RestoreAsync(lessonId);
            await _lessonRepository.SaveChangesAsync();
        }

        // ============================================================================
        // 🎯 Use Case: "المعلم ينهي تحضير الدرس ويضغط (نشر) ليصبح مرئياً للطلاب"
        //
        // مين يستدعيها: المعلم من لوحة تحكم إدارة المحتوى، قبل بدء الفصل الدراسي
        //               أو بعد الانتهاء من كتابة محتوى الدرس.
        // ============================================================================
        public async Task PublishLessonAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new NotFoundException($"الدرس برقم {lessonId} غير موجود.");

            lesson.IsPublished = true;
            await _lessonRepository.UpdateAsync(lesson);
            await _lessonRepository.SaveChangesAsync();
        }

        // ============================================================================
        // 🎯 Use Case: "المعلم يسحب ويرتب الدروس بترتيب مختلف (Drag & Drop) بواجهة الإدارة"
        //
        // مثال: تبديل ترتيب الدرس 3 والدرس 4 داخل نفس الكورس.
        // ============================================================================
        public async Task UpdateLessonOrderAsync(int lessonId, int newOrder)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                ?? throw new NotFoundException($"الدرس برقم {lessonId} غير موجود.");

            lesson.Order = newOrder;
            await _lessonRepository.UpdateAsync(lesson);
            await _lessonRepository.SaveChangesAsync();
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يحل تمريناً (Exercise)، والنظام يحتاج يعرض اسم/رابط
        //              الدرس المرتبط بهذا التمرين (مثلاً لعرض زر (رجوع للدرس))"
        //
        // 💡 Nullable لأنه من الناحية النظرية قد لا يوجد درس مرتبط بهذا التمرين
        //    (حسب تصميم Exercise Entity)، فنترك القرار للـ Controller.
        // ============================================================================
        public async Task<LessonReadDto?> GetLessonByExerciseIdAsync(int exerciseId)
        {
            var lesson = await _lessonRepository.GetByExerciseIdAsync(exerciseId);
            return _mapper.Map<LessonReadDto?>(lesson);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يفتح صفحة الكورس ليرى قائمة الدروس المتاحة للتعلم،
        //              مرتبة بتسلسلها الصحيح (Order)"
        // ============================================================================
        public async Task<IEnumerable<LessonReadDto>> GetLessonsByCourseIdAsync(int courseId)
        {
            var lessons = await _lessonRepository.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<LessonReadDto>>(lessons);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب ينهي الدرس الحالي، فيضغط (التالي) والنظام يوجهه
        //              تلقائياً لأقرب درس تالٍ بالتسلسل ضمن نفس الكورس"
        //
        // 💡 Nullable لأنه من الطبيعي أن يكون الدرس الحالي هو "آخر درس" بالكورس،
        //    وبهذي الحالة ما فيه درس تالٍ (نهاية الكورس)، وهذا ليس خطأ بل حالة طبيعية.
        // ============================================================================
        public async Task<LessonReadDto?> GetNextLessonAsync(int courseId, int currentLessonOrder)
        {
            var nextLesson = await _lessonRepository.GetNextLessonAsync(courseId, currentLessonOrder);
            return _mapper.Map<LessonReadDto?>(nextLesson);
        }
    }
}