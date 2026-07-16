using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // 🔹 CRUD الأساسي

        /// <summary>كل المعلمين</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول) - بروفايل عام، جزء من تصفح الكورسات.</remarks>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TeacherReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TeacherReadDto>>>> GetAll()
        {
            var data = await _teacherService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<TeacherReadDto>>(200, "Teachers retrieved successfully", data));
        }

        /// <summary>بروفايل معلم واحد</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        /// <response code="404">المعلم غير موجود</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<TeacherReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<TeacherReadDto>>> GetById(int id)
        {
            var data = await _teacherService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Teacher not found"));

            return Ok(new ApiResponse<TeacherReadDto>(200, "Teacher retrieved successfully", data));
        }

        /// <summary>إنشاء معلم (مسار إداري قديم)</summary>
        /// <remarks>
        /// الصلاحيات: Admin فقط. إنشاء معلم فعلياً (بحساب دخول كامل) يصير عبر
        /// POST /api/auth/register/teacher - هذا الـ Endpoint مسار إداري قديم مقيّد لـ Admin
        /// حتى ما يُستخدم كثغرة تسجيل بدون حساب.
        ///
        /// مثال Request:
        /// { "name": "أ. أحمد الرياضي", "bio": "معلم رياضيات" }
        /// </remarks>
        /// <response code="400">بيانات غير صالحة</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له (غير Admin)</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<TeacherReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<TeacherReadDto>>> Add([FromBody] TeacherCreateDto dto)
        {
            var createdTeacher = await _teacherService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdTeacher.Id },
                new ApiResponse<TeacherReadDto>(201, "Teacher created successfully", createdTeacher));
        }

        /// <summary>تعديل بروفايل معلم</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher. كل الحقول اختيارية.
        ///
        /// مثال Request:
        /// { "bio": "نبذة محدّثة" }
        /// </remarks>
        /// <response code="404">المعلم غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] TeacherUpdateDto dto)
        {
            await _teacherService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Teacher updated successfully"));
        }

        /// <summary>حذف معلم (Soft Delete)</summary>
        /// <remarks>الصلاحيات: Admin فقط.</remarks>
        /// <response code="404">المعلم غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له (غير Admin)</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _teacherService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Teacher deleted successfully"));
        }

        // 🔹 علاقات إضافية — مطابقة حرفياً لـ ITeacherService
        // ⚠️ حُذفت: GetLessonsByTeacherId / GetTotalCourses / GetTotalLessons — كانت
        // تنادي دوال (GetLessonsByTeacherIdAsync, GetTotalCoursesByTeacherIdAsync,
        // GetTotalLessonsByTeacherIdAsync) غير موجودة إطلاقاً بـ ITeacherService.
        // إذا احتجتها فعلياً، أضفها أولاً بالـ Interface والـ Service.

        /// <summary>كورسات معلم معيّن</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        [HttpGet("{id}/courses")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseReadDto>>>> GetCoursesByTeacherId(int id)
        {
            var courses = await _teacherService.GetCoursesByTeacherIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }
    }
}
