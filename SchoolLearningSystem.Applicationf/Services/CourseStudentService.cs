using AutoMapper;
using SchoolLearningSystem.Applicationf.Common.Models;
using SchoolLearningSystem.Applicationf.Common.Parameters;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CourseStudent;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    // ==================================================================================
    // 📌 دور هذا الـ Service بشكل عام:
    // يدير كل شيء متعلق بـ "العلاقة" بين الطالب والكورس (جدول الربط CourseStudent).
    // هو المكان الوحيد بالمشروع اللي يحتوي منطق التسجيل/الإزالة/التحقق (Business Rules).
    // الريبو (CourseStudentRepository) لا يحتوي أي قرار عمل، هو فقط "ينفذ" ما يطلبه الـ Service.
    // ==================================================================================
    public class CourseStudentService : ICourseStudentService
    {
        // نحتاج 3 ريبوهات مختلفة لأن التسجيل يتطلب التحقق من 3 جداول مرتبطة ببعضها
        private readonly ICourseStudentRepository _courseStudentRepository; // جدول الربط نفسه
        private readonly ICourseRepository _courseRepository;               // للتأكد أن الكورس موجود فعلاً
        private readonly IStudentRepository _studentRepository;             // للتأكد أن الطالب موجود فعلاً
        private readonly IMapper _mapper;                                   // لتحويل Entity <-> DTO
        private readonly ILessonRepository _lessonRepository;
        private readonly IResultRepository _resultRepository;
        public CourseStudentService(
            ICourseStudentRepository courseStudentRepository,
            ICourseRepository courseRepository,
            IStudentRepository studentRepository,
            ILessonRepository lessonRepository,
            IResultRepository resultRepository,
            IMapper mapper)
        {
            _courseStudentRepository = courseStudentRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _lessonRepository = lessonRepository;
            _resultRepository = resultRepository;
        }

        #region 1. العمليات الأساسية والتسجيل (Business Rules & CRUD)

        // ============================================================================
        // 🎯 Use Case: "طالب يضغط على زر (اشترك الآن) للانضمام إلى كورس رياضيات"
        //
        // مين يستدعيها: CourseController أو StudentController، لما الطالب (أو ولي أمره)
        //               يطلب الانضمام لكورس معيّن من واجهة الموقع/التطبيق.
        //
        // القصة الكاملة خطوة بخطوة:
        //   1. نتأكد الطالب "مو مسجل أصلاً" بنفس الكورس (نمنع التسجيل المكرر).
        //   2. نتأكد "الكورس نفسه موجود" بقاعدة البيانات (مو Id وهمي أو محذوف).
        //   3. نتأكد "الطالب نفسه موجود" بقاعدة البيانات (نفس السبب).
        //   4. إذا كل الفحوصات نجحت، ننشئ سجل ربط جديد (CourseStudent) ونحفظه.
        //
        // ليش كل هذا التحقق هنا بالـ Service وليس بالـ Repository؟
        //   لأن الـ Repository "غبي بقصد" (Dumb by design) — وظيفته فقط تنفيذ أوامر
        //   قاعدة البيانات (Add/Update/Delete) بدون أي قرار. أما "هل يجوز التسجيل أو لا؟"
        //   فهذا قرار عمل (Business Decision)، ومكانه الصحيح دائماً هو طبقة الـ Service.
        // ============================================================================
        public async Task EnrollStudentAsync(int courseId, int studentId)
        {
            // أ. فحص التكرار: هل يوجد سجل ربط بنفس (courseId + studentId) مسبقاً؟
            var exists = await _courseStudentRepository.GetByIdAsync(courseId, studentId);
            if (exists != null)
                throw new BadRequestException("عذراً، هذا الطالب مسجل في الكورس بالفعل.");

            // ب. فحص وجود الكورس فعلياً (مو مجرد رقم عشوائي من الـ Client)
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                throw new NotFoundException($"الكورس برقم {courseId} غير موجود.");

            // ج. فحص وجود الطالب فعلياً (نفس المنطق)
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null)
                throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            // د. كل الفحوصات نجحت -> ننشئ سجل التسجيل الفعلي
            var relation = new CourseStudent
            {
                CourseId = courseId,
                StudentId = studentId,
                EnrolledAt = DateTime.UtcNow, // تاريخ التسجيل (يفيد لاحقاً بتحليلات الـ AI)
                IsActive = true                // الاشتراك يبدأ "نشطاً" افتراضياً
            };

            await _courseStudentRepository.AddAsync(relation);
        }

        // ============================================================================
        // 🎯 Use Case: "طالب ينسحب من الكورس، أو الإدارة تطرده/تلغي اشتراكه"
        //
        // مين يستدعيها: الطالب نفسه (زر "إلغاء الاشتراك")، أو الأدمن من لوحة التحكم.
        //
        // القصة: نتأكد أولاً أن سجل الاشتراك موجود أصلاً (ما نقدر نحذف شي غير موجود)،
        //        وبعدين نحذفه فعلياً (Hard Delete من جدول الربط، مو Soft Delete،
        //        لأن جدول الربط لا يحتاج أرشفة — إما الطالب مسجل أو لا).
        // ============================================================================
        public async Task RemoveStudentAsync(int courseId, int studentId)
        {
            var entity = await _courseStudentRepository.GetByIdAsync(courseId, studentId);
            if (entity == null)
                throw new NotFoundException("لا يوجد سجل اشتراك لهذا الطالب في هذا الكورس.");

            await _courseStudentRepository.DeleteAsync(courseId, studentId);
        }

        // ============================================================================
        // 🎯 Use Case: "تعديل تفاصيل حالة الاشتراك"
        // مثال عملي: تجميد اشتراك طالب مؤقتاً (IsActive = false) بدون حذفه بالكامل،
        //            أو تحديث نسبة تقدمه (ProgressPercentage) بعد إنهاء درس.
        //
        // مين يستدعيها: غالباً نظام داخلي (مثل بعد ما الطالب ينهي درساً، النظام
        //               يحدّث ProgressPercentage تلقائياً)، أو الأدمن يدوياً.
        // ============================================================================
        public async Task UpdateCourseStudentAsync(int courseId, int studentId, CourseStudentUpdateDto dto)
        {
            var entity = await _courseStudentRepository.GetByIdAsync(courseId, studentId);
            if (entity == null)
                throw new NotFoundException("سجل الاشتراك المطلوب تعديله غير موجود.");

            // AutoMapper ينسخ فقط القيم الموجودة بالـ dto إلى الـ entity المتتبَّع (Tracked)
            _mapper.Map(dto, entity);
            await _courseStudentRepository.UpdateAsync(entity);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يسجّل نتيجة على درس (اختبار/واجب)، فيحتاج النظام يحدّث
        //              تلقائياً: كم % من الكورس أنهى، ومتى آخر مرة تفاعل معه"
        //
        // مين يستدعيها: ResultService.CreateAsync بعد ما يتأكد النتيجة مرتبطة بدرس صحيح.
        // ============================================================================
        public async Task UpdateProgressAsync(int studentId, int courseId)
        {
            var entity = await _courseStudentRepository.GetByIdAsync(courseId, studentId);
            if (entity == null)
                return; // الطالب مو مسجّل بهذا الكورس أصلاً — لا شيء نحدّثه

            var totalPublishedLessons = (await _lessonRepository.GetByCourseIdAsync(courseId))
                .Count(l => l.IsPublished);

            if (totalPublishedLessons > 0)
            {
                var completedLessons = await _resultRepository.CountDistinctCompletedLessonsAsync(studentId, courseId);
                entity.ProgressPercentage = Math.Min(100.0, (double)completedLessons / totalPublishedLessons * 100.0);
            }

            entity.LastAccessedAt = DateTime.UtcNow;
            await _courseStudentRepository.UpdateAsync(entity);
        }

        // ============================================================================
        // 🎯 Use Case: "الأدمن يستعرض قائمة بكل عمليات التسجيل الخام بالنظام"
        // يفيد هذا بلوحة تحكم إدارية عامة أو تقارير تدقيق شاملة (Audit).
        // ⚠️ ملاحظة: بدون ترقيم (Pagination) — استخدمها بحذر لو عدد السجلات كبير جداً،
        //            وفكر تستخدم GetPagedStudentsByCourseIdAsync بدلها في الواجهات الفعلية.
        // ============================================================================
        public async Task<IEnumerable<CourseStudentReadDto>> GetAllCourseStudentsAsync()
        {
            var entities = await _courseStudentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseStudentReadDto>>(entities);
        }

        // ============================================================================
        // 🎯 Use Case: "جلب تفاصيل سجل اشتراك محدد" (مثلاً: متى انضم الطالب X للكورس Y؟)
        //
        // 💡 نمط Fail Fast: هذي الدالة ترمي NotFoundException دائماً لو السجل غير موجود،
        //    بدل ما ترجع null وتخلي الـ Controller يتحقق بنفسه. هذا يخلي سلوك كل
        //    دوال الـ Service متسقاً (كلها تفشل بنفس الطريقة عند عدم الوجود).
        // ============================================================================
        public async Task<CourseStudentReadDto> GetCourseStudentByIdAsync(int courseId, int studentId)
        {
            var entity = await _courseStudentRepository.GetByIdAsync(courseId, studentId);
            if (entity == null)
                throw new NotFoundException("سجل الاشتراك غير موجود.");

            return _mapper.Map<CourseStudentReadDto>(entity);
        }

        #endregion

        #region 2. علاقات استعلام المحتوى والطلاب (Query Logic)

        // ============================================================================
        // 🎯 Use Case: "المعلم يفتح صفحة كورسه ليشوف مين الطلاب المسجلين فيه"
        //
        // كيف تشتغل: نجيب كل سجلات الربط الخاصة بهذا الكورس (CourseStudent)،
        //            وبعدين نستخرج بس بيانات الطالب (cs.Student) من كل سجل،
        //            لأن الـ Controller يهمه "بيانات الطالب" مو "بيانات الربط نفسها".
        // ============================================================================
        public async Task<IEnumerable<StudentReadDto>> GetStudentsByCourseIdAsync(int courseId)
        {
            var relations = await _courseStudentRepository.GetByCourseIdAsync(courseId);
            var students = relations.Select(cs => cs.Student);
            return _mapper.Map<IEnumerable<StudentReadDto>>(students);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يفتح بروفايله ليرى قائمة كل الكورسات اللي يدرسها حالياً"
        // نفس فكرة الدالة اللي فوق، بس بالاتجاه المعاكس (من الطالب إلى الكورسات).
        // ============================================================================
        public async Task<IEnumerable<CourseReadDto>> GetCoursesByStudentIdAsync(int studentId)
        {
            var relations = await _courseStudentRepository.GetByStudentIdAsync(studentId);
            var courses = relations.Select(cs => cs.Course);
            return _mapper.Map<IEnumerable<CourseReadDto>>(courses);
        }

        #endregion

        #region 3. الترقيم والتحميل الذكي (Advanced Pagination)

        // ============================================================================
        // 🎯 Use Case: "لوحة تحكم المعلم تعرض طلاب الكورس صفحة صفحة (10 بكل صفحة مثلاً)"
        //
        // ليش نحتاجها مع وجود GetStudentsByCourseIdAsync فوق؟
        //   لأن لو الكورس فيه 500 طالب، جلبهم كلهم دفعة وحدة (بدون ترقيم) بطيء جداً
        //   ويرهق الذاكرة والشبكة. هذي الدالة تجيب صفحة واحدة بس (Skip/Take بقاعدة
        //   البيانات مباشرة)، مع رجوع "العدد الكلي" لبناء أزرار الصفحات بالواجهة.
        // ============================================================================
        public async Task<PagedList<StudentReadDto>> GetPagedStudentsByCourseIdAsync(int courseId, QueryParameters parameters)
        {
            var result = await _courseStudentRepository.GetPagedByCourseIdAsync(
                courseId,
                parameters.PageNumber,
                parameters.PageSize);

            var students = result.Items.Select(cs => cs.Student);
            var itemsDto = _mapper.Map<IEnumerable<StudentReadDto>>(students);

            return new PagedList<StudentReadDto>(
                itemsDto,
                result.TotalCount,
                parameters.PageNumber,
                parameters.PageSize);
        }

        // ============================================================================
        // 🎯 Use Case: "الشاشة الرئيسية للطالب تعرض كورساته مرقّمة لضمان سرعة التحميل"
        // نفس منطق الدالة اللي فوق، بس من اتجاه الطالب.
        // ============================================================================
        public async Task<PagedList<CourseReadDto>> GetPagedCoursesByStudentIdAsync(int studentId, QueryParameters parameters)
        {
            var result = await _courseStudentRepository.GetPagedByStudentIdAsync(
                studentId,
                parameters.PageNumber,
                parameters.PageSize);

            var courses = result.Items.Select(cs => cs.Course);
            var itemsDto = _mapper.Map<IEnumerable<CourseReadDto>>(courses);

            return new PagedList<CourseReadDto>(
                itemsDto,
                result.TotalCount,
                parameters.PageNumber,
                parameters.PageSize);
        }

        #endregion

        #region 4. الإحصائيات (Statistics)

        // ============================================================================
        // 🎯 Use Case: "عداد بسيط بواجهة المعلم: (25 طالب مسجل)"
        // نستخدم COUNT مباشر بقاعدة البيانات (أسرع بكثير من جلب كل السجلات وعدّها بالذاكرة).
        // ============================================================================
        public async Task<int> GetTotalStudentsByCourseIdAsync(int courseId)
        {
            return await _courseStudentRepository.CountByCourseIdAsync(courseId);
        }

        // ============================================================================
        // 🎯 Use Case: "النظام يحسب عدد الكورسات النشطة للطالب لتقييم طاقته الاستيعابية"
        // مثال: لو طالب مسجل بـ 5 كورسات بنفس الوقت، ممكن الـ AI يحذر إنه "محمّل فوق طاقته"
        // ويقترح عليه يخفف بدل ما يسجل كورس سادس.
        // ============================================================================
        public async Task<int> GetTotalCoursesByStudentIdAsync(int studentId)
        {
            return await _courseStudentRepository.CountByStudentIdAsync(studentId);
        }

        #endregion
    }
}