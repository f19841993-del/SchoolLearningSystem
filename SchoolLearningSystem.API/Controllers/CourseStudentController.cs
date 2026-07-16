using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.Common.Models;
using SchoolLearningSystem.Applicationf.Common.Parameters;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CourseStudent;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CourseStudentController : ControllerBase
    {
        private readonly ICourseStudentService _courseStudentService;

        public CourseStudentController(ICourseStudentService courseStudentService)
        {
            _courseStudentService = courseStudentService;
        }

        #region 0. CRUD الأساسي (كانت ناقصة كاملة من الكونترولر رغم وجودها بالـ Service)

        /// <summary>كل سجلات تسجيل الطلاب بالكورسات</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("CourseStudent - الأساسيات (CRUD)")]
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseStudentReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseStudentReadDto>>>> GetAll()
        {
            var data = await _courseStudentService.GetAllCourseStudentsAsync();
            return Ok(new ApiResponse<IEnumerable<CourseStudentReadDto>>(200, "Course-Student records retrieved successfully", data));
        }

        /// <summary>سجل تسجيل واحد (طالب بكورس معيّن)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">لا يوجد تسجيل لهذا الطالب بهذا الكورس</response>
        [Tags("CourseStudent - الأساسيات (CRUD)")]
        [HttpGet("{courseId}/{studentId}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<CourseStudentReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CourseStudentReadDto>>> GetById(int courseId, int studentId)
        {
            // ملاحظة: الـ Service ترمي NotFoundException دائماً (Fail Fast) — لا نحتاج null check هنا
            var data = await _courseStudentService.GetCourseStudentByIdAsync(courseId, studentId);
            return Ok(new ApiResponse<CourseStudentReadDto>(200, "Course-Student record retrieved successfully", data));
        }

        /// <summary>تعديل سجل تسجيل (تفعيل/إلغاء تفعيل)</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط.
        ///
        /// مثال Request:
        /// { "isActive": false }
        /// </remarks>
        /// <response code="404">لا يوجد تسجيل لهذا الطالب بهذا الكورس</response>
        [Tags("CourseStudent - الأساسيات (CRUD)")]
        [HttpPatch("{courseId}/{studentId}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Update(int courseId, int studentId, [FromBody] CourseStudentUpdateDto dto)
        {
            await _courseStudentService.UpdateCourseStudentAsync(courseId, studentId, dto);
            return Ok(new ApiResponse(200, "Course-Student record updated successfully"));
        }

        #endregion

        #region 1. عمليات التسجيل (Enrollment)

        /// <summary>تسجيل طالب في كورس معين</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط.
        ///
        /// مثال Request:
        /// { "courseId": 1, "studentId": 1 }
        /// </remarks>
        /// <response code="400">الطالب مسجّل أصلاً بهذا الكورس، أو بيانات غير صالحة</response>
        [Tags("CourseStudent - إدارة التسجيل (Enrollment)")]
        [HttpPost("enroll")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> EnrollStudent([FromBody] CourseStudentCreateDto dto)
        {
            await _courseStudentService.EnrollStudentAsync(dto.CourseId, dto.StudentId);
            return StatusCode(201, new ApiResponse(201, "Student enrolled successfully"));
        }

        /// <summary>إلغاء تسجيل طالب من كورس</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">الطالب غير مسجّل بهذا الكورس أصلاً</response>
        [Tags("CourseStudent - إدارة التسجيل (Enrollment)")]
        [HttpDelete("remove/{courseId}/{studentId}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> RemoveStudent(int courseId, int studentId)
        {
            await _courseStudentService.RemoveStudentAsync(courseId, studentId);
            return Ok(new ApiResponse(200, "Student removed from course successfully"));
        }

        #endregion

        #region 2. الاستعلام عبر العلاقات (غير مرقّمة + مرقّمة)

        /// <summary>كل الطلاب المسجلين بكورس معيّن (بدون ترقيم — استخدمها فقط لقوائم صغيرة)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("CourseStudent - الاستعلام والعلاقات (Queries)")]
        [HttpGet("course/{courseId}/students")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentReadDto>>>> GetStudentsByCourse(int courseId)
        {
            var students = await _courseStudentService.GetStudentsByCourseIdAsync(courseId);
            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", students));
        }

        /// <summary>كل الكورسات التي سجل فيها طالب معيّن (بدون ترقيم)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس كورساته هو.</remarks>
        /// <response code="403">طالب يحاول يشوف كورسات طالب ثاني</response>
        [Tags("CourseStudent - الاستعلام والعلاقات (Queries)")]
        [HttpGet("student/{studentId}/courses")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseReadDto>>>> GetCoursesByStudent(int studentId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var courses = await _courseStudentService.GetCoursesByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        /// <summary>جلب قائمة الطلاب المسجلين في كورس معين (مرقمة)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("CourseStudent - الاستعلام والعلاقات (Queries)")]
        [HttpGet("course/{courseId}/students/paged")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<PagedList<StudentReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<PagedList<StudentReadDto>>>> GetPagedStudentsByCourse(
            int courseId, [FromQuery] QueryParameters parameters)
        {
            var pagedStudents = await _courseStudentService.GetPagedStudentsByCourseIdAsync(courseId, parameters);
            return Ok(new ApiResponse<PagedList<StudentReadDto>>(200, "Students retrieved successfully", pagedStudents));
        }

        /// <summary>جلب قائمة الكورسات التي سجل فيها طالب معين (مرقمة)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس كورساته هو.</remarks>
        /// <response code="403">طالب يحاول يشوف كورسات طالب ثاني</response>
        [Tags("CourseStudent - الاستعلام والعلاقات (Queries)")]
        [HttpGet("student/{studentId}/courses/paged")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [ProducesResponseType(typeof(ApiResponse<PagedList<CourseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<PagedList<CourseReadDto>>>> GetPagedCoursesByStudent(
            int studentId, [FromQuery] QueryParameters parameters)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var pagedCourses = await _courseStudentService.GetPagedCoursesByStudentIdAsync(studentId, parameters);
            return Ok(new ApiResponse<PagedList<CourseReadDto>>(200, "Courses retrieved successfully", pagedCourses));
        }

        #endregion

        #region 3. الإحصائيات (Statistics)

        /// <summary>عدد الطلاب المسجلين بكورس معيّن</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("CourseStudent - الإحصائيات (Statistics)")]
        [HttpGet("course/{courseId}/students/count")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalStudentsByCourseId(int courseId)
        {
            var count = await _courseStudentService.GetTotalStudentsByCourseIdAsync(courseId);
            return Ok(new ApiResponse<int>(200, "Total students count retrieved successfully", count));
        }

        /// <summary>عدد الكورسات التي سجل فيها طالب معيّن</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس عدد كورساته هو.</remarks>
        /// <response code="403">طالب يحاول يشوف عدد كورسات طالب ثاني</response>
        [Tags("CourseStudent - الإحصائيات (Statistics)")]
        [HttpGet("student/{studentId}/courses/count")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalCoursesByStudentId(int studentId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var count = await _courseStudentService.GetTotalCoursesByStudentIdAsync(studentId);
            return Ok(new ApiResponse<int>(200, "Total courses count retrieved successfully", count));
        }

        #endregion
    }
}
