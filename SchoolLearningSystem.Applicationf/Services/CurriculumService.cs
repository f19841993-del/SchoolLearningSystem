using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CurriculumDto;
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
    // يدير بيانات "المنهج" (Curriculum) نفسه، بالإضافة لعمليات تجمع بينه وبين الكورسات
    // التابعة له. العمليات الأساسية (Create/Update/Delete/GetById) تجيه جاهزة من
    // BaseService، وهنا نضيف فقط المنطق الخاص بالمنهج.
    // ==================================================================================
    public class CurriculumService
        : BaseService<Curriculum, CurriculumReadDto, CurriculumCreateDto, CurriculumUpdateDto>, ICurriculumService
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly ICourseRepository _courseRepository;

        // نمرر _curriculumRepository إلى base لأن BaseService يحتاجه لتنفيذ CRUD العام
        public CurriculumService(
            ICurriculumRepository curriculumRepository,
            ICourseRepository courseRepository,
            IMapper mapper)
            : base(curriculumRepository, mapper)
        {
            _curriculumRepository = curriculumRepository;
            _courseRepository = courseRepository;
        }

        // 🔹 الدوال الأساسية (GetAll, GetById, Create, Update, Delete, GetPaged)
        // لا نحتاج كتابتها هنا لأنها موجودة ومطبقة في BaseService

        // ============================================================================
        // 🎯 Use Case: "الأدمن يفتح صفحة منهج معيّن (مثلاً: رياضيات صف ثالث) ليشوف
        //              كل الكورسات/الفصول المسجلة تحت هذا المنهج"
        //
        // مين يستدعيها: CurriculumController، غالباً بصفحة "تفاصيل المنهج" بلوحة الإدارة
        //               أو بواجهة اختيار المنهج قبل ما الطالب يسجل بكورس.
        //
        // ⚠️ ملاحظة تقنية مهمة: لا نعتمد على curriculum.Courses (Navigation Property)
        //    لأن GetByIdAsync لا يستخدم .Include(c => c.Courses)، فهذي الخاصية
        //    ترجع دائماً مجموعة فاضية بصمت (خطأ صامت خطير). لذلك نستخدم
        //    _courseRepository.GetByCurriculumIdAsync مباشرة، وهي مصممة أصلاً
        //    لهذا الغرض بالضبط.
        // ============================================================================
        public async Task<IEnumerable<CourseReadDto>> GetCoursesByCurriculumIdAsync(int curriculumId)
        {
            // أ. نتأكد أولاً أن المنهج نفسه موجود (منع الاستعلام عن منهج وهمي)
            var curriculum = await _curriculumRepository.GetByIdAsync(curriculumId)
                ?? throw new NotFoundException($"المنهج برقم {curriculumId} غير موجود.");

            // ب. نجيب الكورسات من الريبو المختص بها مباشرة (مو من Navigation Property)
            var courses = await _courseRepository.GetByCurriculumIdAsync(curriculumId);
            return _mapper.Map<IEnumerable<CourseReadDto>>(courses);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يختار صفّه الدراسي (مثلاً: الصف الرابع) فيعرض له النظام
        //              المنهج المخصص لهذا الصف تلقائياً"
        //
        // مين يستدعيها: صفحة "اختيار الصف" وقت التسجيل أو بداية العام الدراسي.
        //
        // 💡 ترجع Nullable لأنه من الممكن (نظرياً) عدم وجود منهج بعد لصف معيّن
        //    (مثلاً لو الإدارة لسا ما أضافت منهج الصف السادس). الـ Controller
        //    يقرر شكل الرسالة المناسبة للمستخدم في هذي الحالة (بدل ما نرمي استثناء).
        // ============================================================================
        public async Task<CurriculumReadDto?> GetCurriculumByGradeLevelAsync(GradeLevel gradeLevel)
        {
            var entity = await _curriculumRepository.GetByGradeLevelAsync(gradeLevel);
            return _mapper.Map<CurriculumReadDto?>(entity);
        }

        // ============================================================================
        // 🎯 Use Case: "عداد بلوحة تحكم الأدمن: (منهج رياضيات صف ثالث - 8 كورسات)"
        //
        // نفس منطق GetCoursesByCurriculumIdAsync، بس بدل ما نرجع القائمة كاملة
        // (وهي أثقل لأنها تحمّل كل بيانات الكورسات)، نكتفي بالعدد فقط.
        // ============================================================================
        public async Task<int> GetTotalCoursesByCurriculumIdAsync(int curriculumId)
        {
            var curriculumExists = await _curriculumRepository.GetByIdAsync(curriculumId);
            if (curriculumExists == null)
                throw new NotFoundException($"المنهج برقم {curriculumId} غير موجود.");

            var courses = await _courseRepository.GetByCurriculumIdAsync(curriculumId);
            return courses.Count();
        }
    }
}