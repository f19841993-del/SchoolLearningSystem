using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.Common.Models;
using SchoolLearningSystem.Applicationf.Common.Parameters;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // 🔹 CRUD الأساسي

        /// <summary>كل الكورسات</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول) - تصفح كتالوج الكورسات.</remarks>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseReadDto>>>> GetAllCourses()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        /// <summary>كورس واحد بالتفصيل</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        /// <response code="404">الكورس غير موجود</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CourseReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CourseReadDto>>> GetCourseById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse(404, "Course not found"));

            return Ok(new ApiResponse<CourseReadDto>(200, "Course retrieved successfully", course));
        }

        /// <summary>إنشاء كورس جديد</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط.
        ///
        /// مثال Request:
        /// {
        ///   "title": "رياضيات - الفصل الأول",
        ///   "description": "كورس تجريبي لاختبار محرك SRS",
        ///   "image": "https://example.com/course.jpg",
        ///   "teacherId": 1,
        ///   "curriculumId": 1
        /// }
        /// </remarks>
        /// <response code="400">بيانات غير صالحة (مثلاً image مو رابط صحيح)</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له (مثلاً طالب)</response>
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<CourseReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<CourseReadDto>>> AddCourse([FromBody] CourseCreateDto dto)
        {
            // 🗑️ حُذف فحص ModelState يدوياً: التحقق أصبح كاملاً بمسؤولية FluentValidation
            // داخل الـ Service، وأي خطأ يترجم تلقائياً لـ 400 عبر ExceptionMiddleware.
            var createdCourse = await _courseService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id },
                new ApiResponse<CourseReadDto>(201, "Course created successfully", createdCourse));
        }

        /// <summary>تعديل كورس موجود</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط. كل الحقول اختيارية - ترسل بس الحقل اللي تريد تعدّله.
        ///
        /// مثال Request:
        /// {
        ///   "title": "رياضيات - الفصل الأول (محدّث)"
        /// }
        /// </remarks>
        /// <response code="404">الكورس غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> UpdateCourse(int id, [FromBody] CourseUpdateDto dto)
        {
            await _courseService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Course updated successfully"));
        }

        /// <summary>حذف كورس (Soft Delete)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">الكورس غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> DeleteCourse(int id)
        {
            await _courseService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Course deleted successfully"));
        }

        // 🔹 علاقات إضافية

        /// <summary>دروس كورس معيّن</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        [HttpGet("{id}/lessons")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LessonReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<LessonReadDto>>>> GetLessonsByCourseId(int id)
        {
            var lessons = await _courseService.GetLessonsByCourseIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<LessonReadDto>>(200, "Lessons retrieved successfully", lessons));
        }

        // 🗑️ حُذفت GetStudentsByCourseId نهائياً: كانت تنادي ICourseService.GetStudentsByCourseIdAsync
        // غير الموجودة إطلاقاً (حُذفت من ICourseService ونُقلت لـ ICourseStudentService حصراً).
        // الفرونت يستخدم: GET /api/coursestudent/course/{courseId}/students(/paged)

        /// <summary>امتحانات كورس معيّن</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول). ملاحظة: بيانات الامتحان الوصفية بس، بدون الأسئلة نفسها.</remarks>
        [HttpGet("{id}/exams")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExamReadDto>>>> GetExamsByCourseId(int id)
        {
            var exams = await _courseService.GetExamsByCourseIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ExamReadDto>>(200, "Exams retrieved successfully", exams));
        }

        /// <summary>اسم المعلم المسؤول عن كورس معيّن</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        /// <response code="404">الكورس غير موجود</response>
        [HttpGet("{id}/teacher")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> GetTeacherByCourseId(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse(404, "Course not found"));

            return Ok(new ApiResponse<string>(200, "Teacher retrieved successfully", course.TeacherName));
        }

        /// <summary>اسم المنهج التابع له كورس معيّن</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        /// <response code="404">الكورس غير موجود</response>
        [HttpGet("{id}/curriculum")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> GetCurriculumByCourseId(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse(404, "Course not found"));

            return Ok(new ApiResponse<string>(200, "Curriculum retrieved successfully", course.CurriculumName));
        }

        // 🔹 دالة الترقيم (Pagination)

        /// <summary>الكورسات مع ترقيم صفحات (Pagination)</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        [HttpGet("paged")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<PagedList<CourseReadDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PagedList<CourseReadDto>>>> GetPagedCourses(
            [FromQuery] QueryParameters parameters)
        {
            var pagedCourses = await _courseService.GetPagedAsync(parameters);
            return Ok(new ApiResponse<PagedList<CourseReadDto>>(200, "Courses retrieved successfully", pagedCourses));
        }
    }
}
